using UnityEngine;
using System.Collections;

public class MainShipStats : MonoBehaviour
{
    [Header("Chỉ số sinh tồn")]
    public int maxHP = 4;
    public int currentHP;

    [Header("Hình ảnh Trạng thái")]
    public Sprite[] damageSprites;

    // THÊM DÒNG NÀY: Tạo ổ cắm cho Động cơ trên Inspector
    [Header("Bộ phận đi kèm")]
    public SpriteRenderer engineRenderer;

    [Header("Hệ thống Khiên")]
    public int maxShieldHP = 3;
    private int currentShieldHP;
    public SpriteRenderer shieldRenderer; // Kéo hình ảnh vòng khiên vào đây
    private bool hasShield = false;
    private bool isShieldBlinking = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    //Item
    public void ActivateShield()
    {
        hasShield = true;
        isShieldBlinking = false;
        currentShieldHP = maxShieldHP;

        if (shieldRenderer != null)
        {
            shieldRenderer.gameObject.SetActive(true);
            shieldRenderer.color = new Color(1f, 1f, 1f, 0.7f);
        }
    }

    public void AddHealth(int amount)
    {
        // Nếu máu đã đầy rồi thì không làm gì cả
        if (currentHP >= maxHP) return;

        currentHP += amount;

        // Đảm bảo máu không vượt quá máu tối đa (maxHP)
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        // QUAN TRỌNG: Ăn máu xong phải cập nhật lại hình ảnh con tàu
        // để nó trông "lành lặn" hơn. Hàm UpdateSprite() bạn đã có sẵn rồi.
        UpdateSprite();

        Debug.Log("Đã hồi " + amount + " máu. Máu hiện tại: " + currentHP);
    }

    public void TakeDamage(int damage)
    {
        // 1. NẾU ĐANG CÓ KHIÊN
        if (hasShield)
        {
            if (isShieldBlinking)
            {
                // Khiên đang vỡ mà ăn thêm đạn -> Vỡ nát luôn, trừ máu Tàu
                hasShield = false;
                isShieldBlinking = false;
                shieldRenderer.gameObject.SetActive(false);

                ShipTakeDamage(damage);
            }
            else
            {
                // Khiên khỏe -> Trừ máu Khiên
                currentShieldHP -= damage;
                if (currentShieldHP <= 0) StartCoroutine(ShieldBlinkRoutine());
            }
            return; // Thoát hàm để không chạy lệnh trừ máu Tàu ở dưới
        }

        // 2. NẾU KHÔNG CÓ KHIÊN
        ShipTakeDamage(damage);
    }

    // Hàm phụ: Gom chung logic trừ máu Tàu vào đây cho gọn
    private void ShipTakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        UpdateSprite();

        if (currentHP <= 0) Die();
        else StartCoroutine(BlinkRoutine());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
        //else if (collision.gameObject.CompareTag("EnemyBullet"))
        //{
        //   TakeDamage(1);
        //   Destroy(collision.gameObject);
        //}
    }

    private void UpdateSprite()
    {
        int index = (maxHP - currentHP);
        if (index >= 0 && index < damageSprites.Length)
        {
            spriteRenderer.sprite = damageSprites[index];
        }
    }

    // --- HIỆU ỨNG VỠ KHIÊN ---
    private IEnumerator ShieldBlinkRoutine()
    {
        isShieldBlinking = true;
        for (int i = 0; i < 5; i++)
        {
            if (!isShieldBlinking) yield break; // Dừng nháy nếu bị bắn bồi lúc này

            shieldRenderer.color = new Color(1f, 1f, 1f, 0.2f);
            yield return new WaitForSeconds(0.1f);

            if (!isShieldBlinking) yield break;

            shieldRenderer.color = new Color(1f, 1f, 1f, 0.7f);
            yield return new WaitForSeconds(0.1f);
        }

        if (isShieldBlinking)
        {
            hasShield = false;
            isShieldBlinking = false;
            shieldRenderer.gameObject.SetActive(false);
        }
    }

    // --- HIỆU ỨNG NHẤP NHÁY TÀU ---
    private IEnumerator BlinkRoutine()
    {
        col.enabled = false;

        for (int i = 0; i < 6; i++)
        {
            Color dimColor = new Color(1f, 1f, 1f, 0.2f);
            spriteRenderer.color = dimColor;
            if (engineRenderer != null) engineRenderer.color = dimColor;

            yield return new WaitForSeconds(0.1f);

            Color normalColor = new Color(1f, 1f, 1f, 1f);
            spriteRenderer.color = normalColor;
            if (engineRenderer != null) engineRenderer.color = normalColor;

            yield return new WaitForSeconds(0.1f);
        }

        col.enabled = true;
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}