using UnityEngine;
using System.Collections; // Bắt buộc phải có dòng này để dùng Coroutine

public class RocketEnemy : MonoBehaviour
{
    [Header("Cài đặt Máu & Nổ")]
    public int maxHP = 50;
    private int currentHP;
    public float timeToDestroy = 0.5f;
    private bool isDead = false;

    [Header("Cài đặt Phóng Tên Lửa")]
    public GameObject missilePrefab;
    public Transform[] firePoints;
    private int currentFireIndex = 0;

    [Header("Cài đặt Kamikaze (Lùi để lấy đà)")]
    public float pullBackSpeed = 2f;      // Tốc độ bay giật lùi
    public float pullBackDuration = 0.5f; // Thời gian lùi (giây) - lùi nửa giây
    public float ramSpeed = 25f;          // Tốc độ lao cắm đầu (Chỉnh lên 25-30 cho nhanh vãi luôn!)

    // Chia làm 2 trạng thái rõ ràng
    private bool isPullingBack = false;
    private bool isDashing = false;

    private Animator anim;
    private Collider2D col;

    [Header("Hiệu ứng Động cơ")]
    public GameObject engineObject;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        currentHP = maxHP;
    }

    void Update()
    {
        if (isDead) return;

        // 1. Giai đoạn lùi về phía trên (Space.World để đảm bảo luôn lùi hướng lên)
        if (isPullingBack)
        {
            transform.Translate(Vector3.up * pullBackSpeed * Time.deltaTime, Space.World);
        }
        // 2. Giai đoạn lao cắm đầu cực nhanh
        else if (isDashing)
        {
            transform.Translate(Vector3.down * ramSpeed * Time.deltaTime, Space.World);

            if (transform.position.y < -15f)
            {
                Destroy(gameObject);
            }
        }
    }

    // ==========================================
    // PHẦN BẮN ĐẠN 
    // ==========================================
    public void LaunchNextMissileEvent()
    {
        if (isDead) return;

        if (currentFireIndex < firePoints.Length)
        {
            Transform currentPoint = firePoints[currentFireIndex];

            if (missilePrefab != null && currentPoint != null)
            {
                Instantiate(missilePrefab, currentPoint.position, currentPoint.rotation);
            }

            currentFireIndex++;

            if (currentFireIndex >= firePoints.Length)
            {
                StartRammingPhase();
            }
        }
    }

    void StartRammingPhase()
    {
        if (anim != null) anim.SetTrigger("Ram");

        // Bắt đầu chuỗi kịch bản: Lùi -> Khựng -> Lao
        StartCoroutine(RamSequence());
    }

    // Coroutine: Kịch bản phim chạy theo thời gian
    private IEnumerator RamSequence()
    {
        // 1. Bật trạng thái lùi lại
        isPullingBack = true;

        // Đợi một khoảng thời gian (chính là thời gian đang lùi)
        yield return new WaitForSeconds(pullBackDuration);

        // 2. Dừng lùi. 
        isPullingBack = false;

        // Thêm 1 khoảng KHỰNG LẠI (0.2 giây) lơ lửng trên không để tạo lực ép.
        // Cảm giác giống như nó đang gồng năng lượng trước khi phóng.
        yield return new WaitForSeconds(0.2f);

        // 3. Phóng vút đi!
        isDashing = true;
        Debug.Log("KAMIKAZEEEE!!!!!");
    }

    // ==========================================
    // PHẦN MÁU VÀ CHẾT
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
        isPullingBack = false;
        isDashing = false; // Ngắt luôn mọi chuyển động

        if (col != null) col.enabled = false;
        if (anim != null) anim.SetTrigger("Die");

        Destroy(gameObject, timeToDestroy);
    }

    public void HideEngineEvent()
    {
        if (engineObject != null)
        {
            // Tắt phụt cái object engine đi ngay lập tức
            engineObject.SetActive(false);
        }
    }
}