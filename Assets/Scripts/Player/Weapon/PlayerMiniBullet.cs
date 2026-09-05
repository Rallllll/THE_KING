using UnityEngine;

public class PlayerMiniBullet : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 4f;
    public int damage = 1;

    private float currentLifeTime;

    // Phải có OnEnable để hồi lại 4 giây mỗi khi đạn con được móc từ Pool ra
    void OnEnable()
    {
        currentLifeTime = lifeTime;
    }

    void Update()
    {
        // Space.Self để nó bay theo hướng mũi nhọn đã được xoay
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);

        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
        {
            gameObject.SetActive(false); // Tuyệt đối không dùng Destroy
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (DealDamage(collision, damage))
        {
            gameObject.SetActive(false); // Trúng quái thì tắt luôn, cất về kho
        }
    }

    private bool DealDamage(Collider2D collision, int dmg)
    {
        bool hit = false;
        NormalEnemy normal = collision.GetComponent<NormalEnemy>();
        if (normal != null) { normal.TakeDamage(dmg); hit = true; }

        Asteroid asteroid = collision.GetComponent<Asteroid>();
        if (asteroid != null) { asteroid.TakeDamage(dmg); hit = true; }

        Boss boss = collision.GetComponent<Boss>();
        if (boss != null) { boss.TakeDamage(dmg); hit = true; }

        ShooterEnemy shooter = collision.GetComponent<ShooterEnemy>();
        if (shooter != null) { shooter.TakeDamage(dmg); hit = true; }

        EliteEnemy elite = collision.GetComponent<EliteEnemy>();
        if (elite != null) { elite.TakeDamage(dmg); hit = true; }

        RocketEnemy rocket = collision.GetComponent<RocketEnemy>();
        if (rocket != null) { rocket.TakeDamage(dmg); hit = true; }

        LazerEnemy lazer = collision.GetComponent<LazerEnemy>();
        if (lazer != null) { lazer.TakeDamage(dmg); hit = true; }

        return hit;
    }
}