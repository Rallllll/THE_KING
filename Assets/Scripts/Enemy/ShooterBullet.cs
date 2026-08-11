using UnityEngine;

public class ShooterBullet : MonoBehaviour
{
    [Header("Cài đặt bay")]
    public float speed = 7f;             // Tốc độ bay thẳng

    [Header("Cài đặt tự hủy")]
    public float lifeTime = 3f;          // Sống được 3 giây thì nổ (đỡ rác game nếu bay trượt)
    
    public int damage = 1;               // Sát thương gây ra cho Player

    void Start()
    {
        // Vừa đẻ ra là tự động bẻ cổ cắm đầu xuống dưới (Xoay 180 độ trục Z)
        transform.rotation = Quaternion.Euler(0, 0, 180f);
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

        // 2. Luôn luôn lao thẳng tới trước (theo hướng cái nòng súng đẻ nó ra)
        // Lưu ý: Nếu thấy đạn bay ngược lên, ông hãy đổi Vector3.up thành Vector3.down nhé!
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
    }

    void Explode()
    {
        // Đẻ ra hiệu ứng vụ nổ (nếu có kéo vào Inspector)
        

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

