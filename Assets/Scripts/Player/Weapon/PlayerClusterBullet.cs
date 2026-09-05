using UnityEngine;
using System.Collections;

public class PlayerClusterBullet : MonoBehaviour
{
    [Header("Cài đặt Tụ Đạn")]
    public float chargeTime = 0.5f; // Thời gian gồng đạn (tăng giảm tùy ý)
    public Vector3 maxScale = new Vector3(1f, 1f, 1f); // Kích thước gốc của viên đạn

    [Header("Cài đặt Đạn To")]
    public float speed = 4f;
    public float lifeTime = 2f;
    public int directDamage = 5;

    [Header("Cài đặt Tỏa Đạn")]
    public int bulletCount = 8;

    private float currentLifeTime;
    private bool isCharging = false;

    void OnEnable()
    {
        currentLifeTime = lifeTime;
        StartCoroutine(ChargeRoutine());
    }

    private IEnumerator ChargeRoutine()
    {
        isCharging = true; // Khóa di chuyển
        transform.localScale = Vector3.zero; // Bắt đầu bằng 0 (vô hình)

        float timer = 0f;
        while (timer < chargeTime)
        {
            timer += Time.deltaTime;
            // Phình to dần dần mượt mà
            transform.localScale = Vector3.Lerp(Vector3.zero, maxScale, timer / chargeTime);
            yield return null;
        }

        // Chốt lại kích thước chuẩn để tránh sai số
        transform.localScale = maxScale;
        isCharging = false; // Bắt đầu bay

        // Quan trọng: Gỡ đạn khỏi nòng súng để nó bay tự do, không bị kéo rê theo tàu
        transform.SetParent(null);
    }

    void Update()
    {
        if (isCharging) return; // Nếu đang gồng đạn thì đứng im, không đếm ngược lifeTime

        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);

        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
        {
            ExplodeAndScatter();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Tụ xong mới có sát thương, đang tụ đâm vào không tính
        if (isCharging) return;

        if (DealDamage(collision, directDamage))
        {
            ExplodeAndScatter();
        }
    }

    void ExplodeAndScatter()
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            if (BulletManager.Instance != null)
            {
                GameObject miniBullet = BulletManager.Instance.GetMiniBullet();

                if (miniBullet != null)
                {
                    miniBullet.transform.position = transform.position;
                    miniBullet.transform.rotation = rotation;
                    miniBullet.SetActive(true);
                }
            }
            angle += angleStep;
        }
        gameObject.SetActive(false);
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