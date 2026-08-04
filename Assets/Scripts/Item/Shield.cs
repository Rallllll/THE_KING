using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Tốc độ rơi")]
    public float moveSpeed = 2f;

    void Update()
    {
        // 1. Liên tục di chuyển từ trên xuống dưới
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // 2. Tự hủy nếu rớt ra khỏi mép dưới màn hình (Ví dụ tọa độ Y < -10)
        // Tránh việc vật phẩm rơi mãi vào hư không làm nặng RAM
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    // 3. Xử lý va chạm khi Tàu ăn vật phẩm
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem người chạm vào có phải là Player không
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lấy script MainShipStats gắn trên Tàu
            MainShipStats stats = collision.gameObject.GetComponent<MainShipStats>();

            if (stats != null)
            {
                // Kích hoạt khiên!
                stats.ActivateShield();
            }

            // Hủy cục vật phẩm này đi (nhặt xong rồi)
            Destroy(gameObject);
        }
    }
}