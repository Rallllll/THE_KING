using UnityEngine;
using System.Collections;

public class MainShipStats : MonoBehaviour
{
    [Header("Chỉ số sinh tồn")]
    public int maxHP = 4;
    public int currentHP;

    [Header("Hình ảnh Trạng thái")]
    public Sprite[] damageSprites;

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    // --- HÀM NHẬN SÁT THƯƠNG ---
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        UpdateSprite();

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BlinkRoutine());
        }
    }

    // --- CÒ SÚNG: XÉT VA CHẠM ĐỂ TRỪ MÁU ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1); // Đâm vào địch -> Gọi hàm trừ 1 máu
        }
        //else if (collision.gameObject.CompareTag("EnemyBullet"))
        //{
         //   TakeDamage(1); // Trúng đạn địch -> Gọi hàm trừ 1 máu
          //  Destroy(collision.gameObject); // Xóa viên đạn đi
        //}
    }

    // --- HÀM TỰ ĐỔI ẢNH ---
    private void UpdateSprite()
    {
        int index = (maxHP - currentHP);

        if (index >= 0 && index < damageSprites.Length)
        {
            spriteRenderer.sprite = damageSprites[index];
        }
    }

    // --- HIỆU ỨNG NHẤP NHÁY BẤT TỬ ---
    private IEnumerator BlinkRoutine()
    {
        col.enabled = false;

        for (int i = 0; i < 6; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.2f);
            yield return new WaitForSeconds(0.1f);

            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);
        }

        col.enabled = true;
    }

    void Die()
    {
        gameObject.SetActive(false);
        // GameManager.Instance.TriggerGameOver(); 
    }
}