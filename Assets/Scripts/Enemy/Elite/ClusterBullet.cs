using UnityEngine;

public class ClusterBullet : MonoBehaviour
{
    [Header("Cài đặt Đạn To")]
    public float speed = 4f;        // Tốc độ bay xuống
    public float lifeTime = 2f;     // Bay bao lâu thì tự nổ 

    [Header("Cài đặt Tỏa Đạn")]
    public GameObject miniBulletPrefab; // Prefab đạn con
    public int bulletCount = 8;         // Nổ tung ra 8 viên

    // Dùng biến phụ để đếm giờ, giữ nguyên biến gốc để reset
    private float currentLifeTime;

    // Hàm này tự động chạy mỗi khi viên đạn được "Bật lên" từ kho đạn
    void OnEnable()
    {
        // Reset lại đồng hồ về 2 giây cho những lần dùng lại sau
        currentLifeTime = lifeTime;
    }

    void Update()
    {
        // Lúc gồng, script này bị tắt nên nó không bay. Gồng xong bật lên nó mới chạy lệnh này
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        currentLifeTime -= Time.deltaTime;
        if (currentLifeTime <= 0)
        {
            ExplodeAndScatter();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Trừ máu 
            /* 
            MainShipStats stats = collision.GetComponent<MainShipStats>();
            if (stats != null) stats.TakeDamage(1); 
            */
            ExplodeAndScatter();
        }
    }

    void ExplodeAndScatter()
    {
        // 1. Chia đều 360 độ cho số lượng đạn
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        // 2. Gọi kho đạn đẻ đạn con theo vòng tròn
        for (int i = 0; i < bulletCount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            if (BulletManager.Instance != null && miniBulletPrefab != null)
            {
                // Xin đạn con từ kho đạn của quái vật
                GameObject miniBullet = BulletManager.Instance.GetEnemyBullet(miniBulletPrefab);

                if (miniBullet != null)
                {
                    miniBullet.transform.position = transform.position;
                    miniBullet.transform.rotation = rotation;
                    miniBullet.SetActive(true); // Bật đạn con lên
                }
            }

            angle += angleStep;
        }

        // 3. Trả quả đạn to về kho (Tuyệt đối không dùng Destroy)
        gameObject.SetActive(false);
    }
}