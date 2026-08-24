using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 1; // Sát thương đạn chỉnh ở đây
    private float screenTop;

    void Start()
    {
        screenTop = Camera.main.orthographicSize + 1f;
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > screenTop)
        {
            gameObject.SetActive(false);
        }
    }

    // ==========================================
    // VIÊN ĐẠN ĐI HỎI THĂM TỪNG LOẠI QUÁI
    // ==========================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Quái thường (NormalEnemy)
        NormalEnemy normal = collision.GetComponent<NormalEnemy>();
        if (normal != null) { normal.TakeDamage(damage); gameObject.SetActive(false); return; }

        // 2. Thiên thạch (Asteroid)
        Asteroid asteroid = collision.GetComponent<Asteroid>();
        if (asteroid != null) { asteroid.TakeDamage(damage); gameObject.SetActive(false); return; }

        // 3. Trùm (Boss)
        Boss boss = collision.GetComponent<Boss>();
        if (boss != null) { boss.TakeDamage(damage); gameObject.SetActive(false); return; }

        // 4. Quái bắn súng (ShooterEnemy)
        ShooterEnemy shooter = collision.GetComponent<ShooterEnemy>();
        if (shooter != null) { shooter.TakeDamage(damage); gameObject.SetActive(false); return; }

        // 5. Quái tinh anh (EliteEnemy)
        EliteEnemy elite = collision.GetComponent<EliteEnemy>();
        if (elite != null) { elite.TakeDamage(damage); gameObject.SetActive(false); return; }

        RocketEnemy rocket = collision.GetComponent<RocketEnemy>();
        if (rocket != null) { rocket.TakeDamage(damage); gameObject.SetActive(false); return; }
    }
}