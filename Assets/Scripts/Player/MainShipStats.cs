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

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        currentHP = maxHP;

        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
        //else if (collision.gameObject.CompareTag("EnemyBullet"))
        //{
         //   TakeDamage(1);
          //  Destroy(collision.gameObject);
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

    // --- CẬP NHẬT LẠI HIỆU ỨNG NHẤP NHÁY ---
    private IEnumerator BlinkRoutine()
    {
        col.enabled = false;

        for (int i = 0; i < 6; i++)
        {
            // 1. Làm mờ cả Tàu lẫn Động cơ
            Color dimColor = new Color(1f, 1f, 1f, 0.2f);
            spriteRenderer.color = dimColor;
            if (engineRenderer != null) engineRenderer.color = dimColor; // Kiểm tra null đề phòng bạn quên kéo động cơ vào

            yield return new WaitForSeconds(0.1f);

            // 2. Hiện rõ cả Tàu lẫn Động cơ
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