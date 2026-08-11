using UnityEngine;

public class ClusterBullet : MonoBehaviour
{
    [Header("Cài đặt Đạn To")]
    public float speed = 4f;        // Tốc độ bay xuống
    public float lifeTime = 2f;     // Bay bao lâu thì tự nổ (để 2-3s tùy màn hình)

    [Header("Cài đặt Tỏa Đạn")]
    public GameObject miniBulletPrefab; // Kéo Prefab đạn con vào đây
    public int bulletCount = 8;         // Nổ tung ra 8 viên (hoặc 12, 16 tùy ông)

    void Update()
    {
        // Lúc gồng, script này bị tắt nên nó không bay. Gồng xong bật lên nó mới chạy lệnh này
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            ExplodeAndScatter();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Trừ máu (Nhớ mở comment đoạn này và thay tên script Máu của Tàu ông vào)
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

        // 2. Đẻ đạn con theo vòng tròn
        for (int i = 0; i < bulletCount; i++)
        {
            // Lệnh này bẻ cổ viên đạn xoay ra các hướng khác nhau
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(miniBulletPrefab, transform.position, rotation);

            angle += angleStep; // Cứ mỗi viên lại cộng thêm góc
        }

        // 3. Xóa sổ quả đạn to
        Destroy(gameObject);
    }
}