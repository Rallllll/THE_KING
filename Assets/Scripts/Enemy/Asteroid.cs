using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Chỉ số Thiên thạch")]
    public int hp = 3;
    public float speed = 3f;
    public float explosionDuration = 0.5f; // Thời gian chạy hết animation nổ (ví dụ 0.8 giây)

    private float screenBottom;
    private bool isDead = false; // Cờ đánh dấu thiên thạch đã chết chưa

    private Animator anim;
    private Collider2D col;

    public int scoreValue = 10;

    void Start()
    {
        screenBottom = -Camera.main.orthographicSize - 2f;

        // Lấy linh kiện từ Prefab
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Vẫn cho trôi xuống, hoặc nếu muốn nổ thì đứng im, bạn có thể thêm: if(isDead) speed = 0;
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

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
        isDead = true; // Đánh dấu là đã chết

        col.enabled = false; // TẮT KHUNG VA CHẠM: Đạn bay xuyên qua, không gây sát thương cho tàu nữa

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        LootDrop loot = GetComponent<LootDrop>();
        if (loot != null)
        {
            loot.DropCoin();
        }

        anim.SetTrigger("Explo"); // KÍCH HOẠT ANIMATION VỠ + NỔ

        // Hủy object sau khi đã chạy xong thời gian của đoạn phim nổ
        Destroy(gameObject, explosionDuration);
    }
}

