using UnityEngine;
using System.Collections;

public class LazerEnemy : MonoBehaviour
{
    [Header("Chỉ số")]
    public int maxHP = 50;
    private int currentHP;
    public float moveDownSpeed = 4f;
    public float stopY = 3f;

    [Header("Cài đặt Laze & Hoạt ảnh")]
    public Transform firePos;         // Kéo Object nòng súng (Fire Pos) vào đây
    public GameObject laserObject;    // Kéo tia Laze vào đây
    public float attackCooldown = 3f;

    [Header("Hiệu ứng Nổ")]
    public GameObject engineObject;
    public float explosionDuration = 0.5f;

    private bool isArrived = false;
    private bool isDead = false;

    private Animator anim;
    private Collider2D col;

    public int scoreValue = 10;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        if (laserObject != null)
        {
            laserObject.SetActive(false); // Tắt laze lúc mới đẻ ra

            // Tự động gắn chặt tia laze vào đúng tọa độ của nòng súng (firePos)
            
        }
    }

    void Update()
    {
        if (isDead) return;

        if (!isArrived)
        {
            transform.Translate(Vector3.down * moveDownSpeed * Time.deltaTime, Space.World);
            if (transform.position.y <= stopY)
            {
                isArrived = true;
                StartCoroutine(AttackRoutine());
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);
            if (anim != null) anim.SetTrigger("Attack");
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    // ==========================================
    // ANIMATION EVENTS (Bật / Tắt Laze)
    // ==========================================
    public void StartLaserEvent()
    {
        if (isDead) return;
        if (laserObject != null) laserObject.SetActive(true);
    }

    public void StopLaserEvent()
    {
        if (laserObject != null) laserObject.SetActive(false);
    }

    // ==========================================
    // PHẦN VA CHẠM VÀ CHẾT
    // ==========================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // Nếu lao thẳng vào mặt Player thì nổ tung xác
        if (collision.CompareTag("Player"))
        {
            Die(); // Hoặc gọi hàm trừ máu Player ở đây nếu ông có
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHP -= damageAmount;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        if (col != null) col.enabled = false;

        if (engineObject != null) engineObject.SetActive(false);
        if (laserObject != null) laserObject.SetActive(false);

        if (anim != null) anim.SetTrigger("Die");
        Destroy(gameObject, explosionDuration);
    }
}

