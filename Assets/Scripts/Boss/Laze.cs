using UnityEngine;

public class Laze : MonoBehaviour
{
    [Header("Cài đặt Sát thương")]
    public int damage = 1;          // Mỗi lần đốt mất bao nhiêu máu
    public float damageRate = 0.2f; // Bao nhiêu giây đốt máu 1 lần (0.2s = 1 giây mất 5 máu)

    private float nextDamageTime = 0f;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Kiểm tra xem đã hết thời gian hồi (cooldown) để đốt máu nhịp tiếp theo chưa
            if (Time.time >= nextDamageTime)
            {
                
                MainShipStats playerStats = collision.GetComponent<MainShipStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                }
                
                // Đặt lại đồng hồ chờ cho nhịp đốt máu tiếp theo
                nextDamageTime = Time.time + damageRate;
            }
        }
    }

    // Reset lại bộ đếm thời gian mỗi khi tia Laze được bật lên
    void OnEnable()
    {
        nextDamageTime = Time.time;
    }
}
