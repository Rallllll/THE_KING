using UnityEngine;
using System.Collections;
using TMPro; // Thêm thư viện để dùng UI Text

public class MainShipStats : MonoBehaviour
{
    [Header("Chỉ số sinh tồn")]
    public int maxHP = 100; // Đã đổi thành số lớn, ông có thể tùy chỉnh
    public int currentHP;

    [Header("UI Hiển thị Máu")]
    public TextMeshProUGUI hpText; // Kéo Text máu trên Canvas vào ô này

    [Header("Hình ảnh Trạng thái")]
    public Sprite[] damageSprites;

    [Header("Bộ phận đi kèm")]
    public SpriteRenderer engineRenderer;

    [Header("Hệ thống Khiên")]
    public int maxShieldHP = 3;
    private int currentShieldHP;
    public SpriteRenderer shieldRenderer;
    private bool hasShield = false;
    private bool isShieldBlinking = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        UpdateHPText(); // Hiển thị máu ngay khi vào game
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
        if (currentHP >= maxHP) return;

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateSprite();
        UpdateHPText(); // Cập nhật chữ khi hồi máu

        Debug.Log("Đã hồi " + amount + " máu. Máu hiện tại: " + currentHP);
    }

    public void TakeDamage(int damage)
    {
        if (hasShield)
        {
            if (isShieldBlinking)
            {
                hasShield = false;
                isShieldBlinking = false;
                shieldRenderer.gameObject.SetActive(false);

                ShipTakeDamage(damage);
            }
            else
            {
                currentShieldHP -= damage;
                if (currentShieldHP <= 0) StartCoroutine(ShieldBlinkRoutine());
            }
            return;
        }

        ShipTakeDamage(damage);
    }

    private void ShipTakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        UpdateSprite();
        UpdateHPText(); // Cập nhật chữ khi mất máu

        if (currentHP <= 0) Die();
        else StartCoroutine(BlinkRoutine());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10); // Đổi số damage tùy ý cho phù hợp với maxHP mới
        }
    }

    // ==========================================
    // LOGIC CẬP NHẬT ẢNH DỰA TRÊN PHẦN TRĂM MÁU
    // ==========================================
    private void UpdateSprite()
    {
        if (damageSprites == null || damageSprites.Length == 0) return;

        // Tính phần trăm máu (từ 0.0 đến 1.0)
        float healthPercent = (float)currentHP / maxHP;

        // Nội suy ra vị trí ảnh: 100% lấy ảnh 0, 0% lấy ảnh cuối cùng
        int index = damageSprites.Length - 1 - Mathf.FloorToInt(healthPercent * (damageSprites.Length - 1));

        // Khóa mốc an toàn để index không văng ra ngoài mảng
        index = Mathf.Clamp(index, 0, damageSprites.Length - 1);

        spriteRenderer.sprite = damageSprites[index];
    }

    // ==========================================
    // LOGIC CẬP NHẬT CHỮ LÊN MÀN HÌNH
    // ==========================================
    private void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = currentHP.ToString();
        }
    }

    private IEnumerator ShieldBlinkRoutine()
    {
        isShieldBlinking = true;
        for (int i = 0; i < 5; i++)
        {
            if (!isShieldBlinking) yield break;

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
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ShowLosePanel();
        }

        gameObject.SetActive(false);
    }
}