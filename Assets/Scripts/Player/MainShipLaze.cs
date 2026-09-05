using UnityEngine;

public class MainShipLaze : MonoBehaviour
{
    [Header("Cài đặt Sát thương Laser")]
    public int damage = 1;
    public float damageRate = 0.2f; // Cứ 0.2 giây sẽ giật sát thương 1 lần (1 giây trừ máu 5 lần)

    private float nextDamageTime = 0f;

    // Dùng Stay thay vì Enter để quái vật đứng trong tia Laser sẽ bị trừ máu liên tục
    void OnTriggerStay2D(Collider2D collision)
    {
        // 1. Kiểm tra Tag trước cho nhẹ máy (Đúng Tag Enemy mới xử lý tiếp)
        // Lưu ý: Nếu quái của ông chưa có Tag "Enemy", hãy vào Inspector gắn Tag cho tụi nó nhé
        if (collision.CompareTag("Enemy"))
        {
            // 2. Kiểm tra xem đã đến nhịp trừ máu tiếp theo chưa
            if (Time.time >= nextDamageTime)
            {
                bool hasDealtDamage = DealDamage(collision);

                // Nếu thực sự giật trúng quái, thì thiết lập lại đồng hồ đếm ngược
                if (hasDealtDamage)
                {
                    nextDamageTime = Time.time + damageRate;
                }
            }
        }
    }

    // ==========================================
    // KIỂM TRA TỪNG LOẠI QUÁI (GIỐNG HỆT ĐẠN THƯỜNG)
    // ==========================================
    private bool DealDamage(Collider2D collision)
    {
        bool hit = false;

        // 1. Quái thường (NormalEnemy)
        NormalEnemy normal = collision.GetComponent<NormalEnemy>();
        if (normal != null) { normal.TakeDamage(damage); hit = true; }

        // 2. Thiên thạch (Asteroid)
        Asteroid asteroid = collision.GetComponent<Asteroid>();
        if (asteroid != null) { asteroid.TakeDamage(damage); hit = true; }

        // 3. Trùm (Boss)
        Boss boss = collision.GetComponent<Boss>();
        if (boss != null) { boss.TakeDamage(damage); hit = true; }

        // 4. Quái bắn súng (ShooterEnemy)
        ShooterEnemy shooter = collision.GetComponent<ShooterEnemy>();
        if (shooter != null) { shooter.TakeDamage(damage); hit = true; }

        // 5. Quái tinh anh (EliteEnemy)
        EliteEnemy elite = collision.GetComponent<EliteEnemy>();
        if (elite != null) { elite.TakeDamage(damage); hit = true; }

        RocketEnemy rocket = collision.GetComponent<RocketEnemy>();
        if (rocket != null) { rocket.TakeDamage(damage); hit = true; }

        LazerEnemy lazer = collision.GetComponent<LazerEnemy>();
        if (lazer != null) { lazer.TakeDamage(damage); hit = true; }

        return hit; // Trả về true nếu quái đã ăn đòn
    }
}