using UnityEngine;

public class Heal : MonoBehaviour
{
    [Header("Cài đặt vật phẩm")]
    public float moveSpeed = 2f; // Tốc độ rơi

    [Header("Chỉ số hồi phục")]
    public int healthToHeal = 1; // Số máu sẽ hồi khi ăn

    void Update()
    {
        // 1. Liên tục di chuyển từ trên xuống dưới (Copy y xì từ Shield)
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // 2. Tự hủy nếu rớt ra khỏi mép dưới màn hình (Copy y xì từ Shield)
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    // 3. Xử lý va chạm khi Tàu ăn vật phẩm (Sửa lại chỗ này)
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem người chạm vào có phải là Player không
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lấy script MainShipStats gắn trên Tàu
            MainShipStats stats = collision.gameObject.GetComponent<MainShipStats>();

            if (stats != null)
            {
                // KHÁC BIỆT Ở ĐÂY: Thay vì gọi ActivateShield, ta gọi AddHealth
                stats.AddHealth(healthToHeal);
            }

            // Hủy cục vật phẩm này đi (nhặt xong rồi)
            Destroy(gameObject);
        }
    }
}