using UnityEngine;

public class Bullet_Boss : MonoBehaviour
{
    [Header("Cài đặt bay")]
    public float speed = 8f;
    public float rotateSpeed = 300f;     // Lượn càng cao cua càng gắt

    [Header("Định hướng ảnh (Căn phần lồi)")]
    // Vì Kla'ed - Wave.png có phần lồi hướng lên trên, ta trừ đi 90 độ 
    // để ép cái phần lồi đó luôn chĩa thẳng vào mục tiêu.
    public float angleOffset = -90f;

    [Header("Cài đặt tự hủy & Delay")]
    public float lifeTime = 5f;
    public float startDelay = 0.1f;      // Thời gian khựng lại trước khi bay 
    public GameObject explosionPrefab;

    private Transform target;
    private SpriteRenderer sr;
    private float delayTimer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        delayTimer = startDelay;

        // Quét tìm tàu Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) target = player.transform;

        // =========================================================
        // ÉP TƯ THẾ BAN ĐẦU TRONG THỜI GIAN DELAY 0.1s
        // =========================================================
        if (transform.position.x > 0)
        {
            // Nằm ở nửa bên Phải màn hình -> Đầu lồi chĩa sang Phải (>)
            // Vì ảnh gốc chĩa lên trên, muốn chĩa sang phải thì xoay Z -90 độ
            transform.rotation = Quaternion.Euler(0, 0, -90f);
            if (sr != null) sr.flipX = true;
        }
        else
        {
            // Nằm ở nửa bên Trái màn hình -> Đầu lồi chĩa sang Trái (<)
            // Vì ảnh gốc chĩa lên trên, muốn chĩa sang trái thì xoay Z 90 độ
            transform.rotation = Quaternion.Euler(0, 0, 90f);
            if (sr != null) sr.flipX = false;
        }
    }

    void Update()
    {
        // --- BỘ ĐẾM DELAY KHỰNG LẠI (0.1s) ---
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return; // Dừng tại đây, giữ nguyên dáng < hoặc > chờ hết giờ
        }

        // Bắt đầu tính thời gian sống sau khi đã hết khựng
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Explode();
            return;
        }

        // --- BẺ LÁI ÉP PHẦN LỒI CHĨA VÀO MỤC TIÊU ---
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            // Tính góc và cộng thêm angleOffset để bù trừ cho ảnh sóng
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
            Quaternion targetRot = Quaternion.Euler(0, 0, angle);

            // Xoay mượt mà từ dáng < hoặc > ban đầu về phía Player
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        // --- BAY VỀ PHÍA TRƯỚC BẰNG PHẦN LỒI ---
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    void Explode()
    {
        if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {         
            MainShipStats stats = collision.GetComponent<MainShipStats>();
            if (stats != null) stats.TakeDamage(1);
            
            Explode();
        }
    }
}