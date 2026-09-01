using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    [Header("Chỉ số Kẻ địch")]
    public int hp = 3;
    public float speed = 2f; // Tốc độ trôi từ từ giống thiên thạch

    [Header("Cài đặt Bắn đạn")]
    public GameObject bulletPrefab; // Kéo Prefab đạn của địch vào đây
    public Transform firePos;       // Kéo cái nòng súng vào đây
    public float fireRate = 1.5f;   // Bao nhiêu giây bắn 1 viên (1.5 là nhịp vừa đẹp)

    private float fireTimer;

    [Header("Cài đặt Nổ")]
    public float explosionDuration = 0.5f;

    private float screenBottom;
    private bool isDead = false;

    private Animator anim;
    private Collider2D col;

    public int scoreValue = 10;

    void Start()
    {
        screenBottom = -Camera.main.orthographicSize - 2f;

        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        // Khởi tạo timer để vừa bay ra sân được một tí là xả đạn luôn
        fireTimer = fireRate;
    }

    void Update()
    {
        // Đang nổ thì không bay và không bắn nữa
        if (isDead) return;

        // 1. Di chuyển lững lờ trôi xuống dưới
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        // 2. Hệ thống xả đạn (Đếm ngược thời gian)
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            Shoot();
            fireTimer = fireRate; // Bắn xong thì reset lại đồng hồ
        }

        // 3. Bay quá đà lọt khỏi màn hình thì tự dọn rác
        if (transform.position.y < screenBottom)
        {
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        // Kiểm tra xem đã gắn đạn và nòng súng chưa để tránh văng lỗi
        if (bulletPrefab != null && firePos != null)
        {
            // Đẻ viên đạn ra tại vị trí của firePos
            Instantiate(bulletPrefab, firePos.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        hp -= damageAmount;
        if (hp <= 0) Explode();
    }

    void Explode()
    {
        isDead = true;

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        LootDrop loot = GetComponent<LootDrop>();
        if (loot != null)
        {
            loot.DropCoin();
        }

        if (col != null) col.enabled = false;
        if (anim != null) anim.SetTrigger("Die"); // Nhớ set trigger "Explo" trong Animator nhé

        Destroy(gameObject, explosionDuration);
    }
}