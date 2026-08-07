using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Cài đặt bay")]
    public float speed = 7f;             // Tốc độ bay
    public float rotateSpeed = 200f;     // Tốc độ bẻ lái (càng cao cua càng gắt)

    [Header("Cài đặt tự hủy")]
    public float lifeTime = 3f;          // Sống được 3 giây thì nổ
    public GameObject explosionPrefab;   // Kéo hiệu ứng nổ vào đây (nếu có)
    public int damage = 1;               // Sát thương gây ra cho Player

    private Transform target;            // Mục tiêu (Tàu của người chơi)

    void Start()
    {
        // Vừa đẻ ra là tự tìm người chơi (Đảm bảo tàu của ông đã gắn Tag là "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        // 1. Đếm ngược 3 giây thì tự hủy/nổ
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Explode();
            return; // Hết giờ thì dừng code, không bay nữa
        }

        // 2. Liên tục bẻ lái hướng về người chơi
        if (target != null)
        {
            Vector2 direction = target.position - transform.position;

            // Tính góc xoay. 
            // Lưu ý: -90f là để bù trừ nếu mũi tên lửa trong file ảnh gốc của ông đang hướng thẳng lên trên
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        // 3. Luôn luôn lao thẳng tới trước (theo hướng cái mũi đang chĩa vào)
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    void Explode()
    {
        // Đẻ ra hiệu ứng vụ nổ (nếu có kéo vào Inspector)
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Xóa viên tên lửa này
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu tông trúng tàu của người chơi
        if (collision.CompareTag("Player"))
        {
            // Trừ máu (ông nhớ đổi MainShipStats thành tên script máu của tàu ông nếu nó khác nhé)
            MainShipStats stats = collision.GetComponent<MainShipStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
            Explode(); // Chạm là nổ tung
        }
    }
}
