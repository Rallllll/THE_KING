using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    [Header("Chỉ số Kẻ địch")]
    public int hp = 3;

    [Header("Cài đặt Di chuyển")]
    public float fastSpeed = 10f;       // Tốc độ bay nhanh lúc mới xuất hiện
    public float slowSpeed = 2f;        // Tốc độ trôi từ từ sau khi phanh
    public float slowDownY = 5f;        // Tới tọa độ Y này (trên màn hình) thì bắt đầu đạp phanh
    public float brakeSmoothness = 3f;  // Độ mượt khi phanh (càng to phanh càng gắt)

    private float currentSpeed;         // Tốc độ thực tế đang chạy

    [Header("Cài đặt Nổ")]
    public float explosionDuration = 0.5f;

    public int scoreValue = 10;

    private float screenBottom;
    private bool isDead = false;

    private Animator anim;
    private Collider2D col;

    void Start()
    {
        screenBottom = -Camera.main.orthographicSize - 2f;

        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        // Vừa chui ra là cho chạy max tốc độ
        currentSpeed = fastSpeed;
    }

    void Update()
    {
        // 1. Xử lý logic thay đổi tốc độ (Chỉ chạy khi chưa chết)
        if (!isDead)
        {
            // Nếu bay vượt qua vạch mốc Y (slowDownY) thì bắt đầu giảm tốc
            if (transform.position.y <= slowDownY)
            {
                // Hàm Lerp giúp giảm tốc độ từ từ (êm như đạp phanh ô tô) thay vì khựng lại giật cục
                currentSpeed = Mathf.Lerp(currentSpeed, slowSpeed, Time.deltaTime * brakeSmoothness);
            }
        }
        else
        {
            // TÙY CHỌN: Nếu muốn lúc đang nổ mà xác nó trôi chậm dần lại thì dùng dòng này
            // Còn nếu muốn đứng khựng lại luôn thì thay bằng: currentSpeed = 0;
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 5f);
        }

        // 2. Thực hiện di chuyển xuống dưới
        transform.Translate(Vector3.down * currentSpeed * Time.deltaTime, Space.World);

        // 3. Dọn rác khi bay lọt khỏi màn hình
        if (transform.position.y < screenBottom)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // NẾU ĐÃ CHẾT RỒI THÌ KHÔNG XỬ LÝ VA CHẠM NỮA
        if (isDead) return;

        // Chỉ cần giữ lại logic nếu đâm thẳng vào tàu Player thì nổ tung
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        hp -= damageAmount;

        if (hp <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        isDead = true;

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        if (col != null) col.enabled = false;
        if (anim != null) anim.SetTrigger("Die");

        Destroy(gameObject, explosionDuration);
    }
}