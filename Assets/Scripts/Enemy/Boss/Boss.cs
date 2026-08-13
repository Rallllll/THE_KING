using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Chỉ số Boss")]
    public int hp = 50;
    public float moveDownSpeed = 2f;
    public float stopY = 3f;

    [Header("Chiêu 1: Cụm Nòng Súng")]
    public GameObject bulletPrefab;
    // Khai báo mảng: Ra ngoài Unity gõ số lượng nòng rồi kéo thả vào
    public Transform[] firePoints;

    [Header("Chiêu 2: Giáp Bảo Vệ")]
    public GameObject shieldObject;  // Kéo cái Object Sprite Giáp (con) vào đây
    public int maxShieldHP = 15;

    private int currentShieldHP;
    private bool isShieldActive = false;

    [Header("Hiệu ứng Nổ & Hoạt ảnh")]
    public GameObject engineObject;
    public float explosionDuration = 0.5f;

    private bool isArrived = false;
    private bool isDead = false;

    private Animator anim;
    private Collider2D col;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        // Chắc chắn giáp tắt lúc Boss mới sinh ra
        if (shieldObject != null) shieldObject.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;

        // Bay từ từ xuống mốc rồi dừng lại
        if (!isArrived)
        {
            transform.Translate(Vector3.down * moveDownSpeed * Time.deltaTime, Space.World);
            if (transform.position.y <= stopY)
            {
                isArrived = true;
                StartCoroutine(BossComboRoutine());
            }
        }
    }

    IEnumerator BossComboRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);

            // BẬT CHIÊU 1: Gọi Animation tấn công. 
            // Việc xả đạn sẽ do Animation Event quyết định trên Timeline!
            if (anim != null) anim.SetTrigger("Attack");

            yield return new WaitForSeconds(3f);

            // BẬT CHIÊU 2: Kích hoạt giáp (nếu đang không có)
            if (!isShieldActive)
            {
                ActivateShield();
            }

            yield return new WaitForSeconds(2f);
        }
    }

    // ==========================================
    // ANIMATION EVENT: BẮN ĐÚNG NÒNG SÚNG CHỈ ĐỊNH
    // ==========================================
    // Điền số thứ tự (Index: 0, 1, 2...) của nòng súng vào ô Int trên cửa sổ Animation
    public void FireBulletEvent(int pointIndex)
    {
        if (isDead || bulletPrefab == null) return;

        // Kiểm tra xem số Index truyền vào có hợp lệ không (tránh lỗi Out of Range)
        if (pointIndex >= 0 && pointIndex < firePoints.Length)
        {
            Transform currentPoint = firePoints[pointIndex];
            if (currentPoint != null)
            {
                // Bắn 1 viên đạn tại đúng vị trí và góc độ của nòng súng đó
                Instantiate(bulletPrefab, currentPoint.position, currentPoint.rotation);
            }
        }
    }

    // ==========================================
    // CƠ CHẾ GIÁP (SHIELD TỪ OBJECT CON)
    // ==========================================
    void ActivateShield()
    {
        if (isDead) return;
        isShieldActive = true;
        currentShieldHP = maxShieldHP;
        if (shieldObject != null) shieldObject.SetActive(true);
    }

    void BreakShield()
    {
        isShieldActive = false;
        if (shieldObject != null) shieldObject.SetActive(false);
    }

    // ==========================================
    // PHẦN VA CHẠM VÀ CHẾT
    // ==========================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // Xử lý đạn Player (Tái chế đạn bằng SetActive(false) y như code cũ của ông)
        if (collision.CompareTag("MBullet") || collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);

            if (isShieldActive)
            {
                currentShieldHP--;
                if (currentShieldHP <= 0) BreakShield();
            }
            else
            {
                hp--;
                if (hp <= 0) Die();
            }
        }
    }

    void Die()
    {
        isDead = true;
        if (col != null) col.enabled = false;

        // Tắt động cơ ngay lập tức
        if (engineObject != null) engineObject.SetActive(false);

        // Tắt luôn giáp nếu nó đang bật (để tránh Boss nổ mà giáp vẫn lơ lửng)
        if (shieldObject != null) shieldObject.SetActive(false);

        if (anim != null) anim.SetTrigger("Die");
        Destroy(gameObject, explosionDuration);
    }

    // Event dự phòng: Nếu ông thích tắt động cơ từ 1 frame cụ thể trên Timeline
    public void HideEngineEvent()
    {
        if (engineObject != null) engineObject.SetActive(false);
    }
}