using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Chỉ số Boss")]
    public int hp = 50;
    public float moveDownSpeed = 2f;
    public float stopY = 3f;

    [Header("Cài đặt Nhịp độ Chiêu thức")]
    public float startDelay = 1f;       // Thời gian đứng thở lấy hơi trước khi tung chiêu
    public float attackDuration = 3f;   // Thời gian dành cho Animation bắn đạn (đủ để chạy hết Event)
    public float shieldCooldown = 2f;   // Thời gian nghỉ giữa các lần chuẩn bị bật giáp

    [Header("Chiêu 1: Cụm Nòng Súng")]
    public GameObject bulletPrefab;
    public Transform[] firePoints;

    [Header("Chiêu 2: Giáp Bảo Vệ")]
    public GameObject shieldObject;
    public int maxShieldHP = 15;

    private int currentShieldHP;
    private bool isShieldActive = false;

    [Header("Hiệu ứng Nổ & Hoạt ảnh")]
    public GameObject engineObject;
    public float explosionDuration = 0.5f;

    public int scoreValue = 500;

    private bool isArrived = false;
    private bool isDead = false;

    private Animator anim;
    private Collider2D col;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        if (shieldObject != null) shieldObject.SetActive(false);
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
                StartCoroutine(BossComboRoutine());
            }
        }
    }

    IEnumerator BossComboRoutine()
    {
        while (!isDead)
        {
            // 1. Đứng thở
            yield return new WaitForSeconds(startDelay);

            // 2. Kích hoạt trạng thái Bắn
            if (anim != null) anim.SetTrigger("Attack");

            // 3. Đợi cho Animation bắn chạy hết
            yield return new WaitForSeconds(attackDuration);

            // --- THÊM DÒNG NÀY: Ép nó quay về dáng đứng im, không cho lặp lại Animation Bắn nữa ---
            if (anim != null) anim.SetTrigger("Idle");

            // 4. Bật Giáp
            if (!isShieldActive)
            {
                ActivateShield();
            }

            // 5. Nghỉ ngơi hồi chiêu (lúc này nó đang đứng im chờ hết 10s)
            yield return new WaitForSeconds(shieldCooldown);
        }
    }

    public void FireBulletEvent(int pointIndex)
    {
        if (isDead || bulletPrefab == null) return;

        if (pointIndex >= 0 && pointIndex < firePoints.Length)
        {
            Transform currentPoint = firePoints[pointIndex];
            if (currentPoint != null)
            {
                Instantiate(bulletPrefab, currentPoint.position, currentPoint.rotation);
            }
        }
    }

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        // Chỉ kiểm tra đâm vào Player
        if (collision.CompareTag("Player"))
        {
            Die();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        // Nếu đang bật giáp thì trừ máu giáp
        if (isShieldActive)
        {
            currentShieldHP -= damageAmount;
            if (currentShieldHP <= 0) BreakShield();
        }
        // Nếu không có giáp thì trừ máu thật
        else
        {
            hp -= damageAmount;
            if (hp <= 0) Die();
        }
    }

    void Die()
    {
        isDead = true;
        if (col != null) col.enabled = false;

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue); // Cộng nốt điểm giết boss
            ScoreManager.instance.ShowWinPanel();       // Văng bảng VICTORY ra!
        }

        if (engineObject != null) engineObject.SetActive(false);
        if (shieldObject != null) shieldObject.SetActive(false);

        if (anim != null) anim.SetTrigger("Die");
        Destroy(gameObject, explosionDuration);
    }

    public void HideEngineEvent()
    {
        if (engineObject != null) engineObject.SetActive(false);
    }
}