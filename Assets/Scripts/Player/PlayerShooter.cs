using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Cài đặt Súng")]
    public Transform firePosition;
    public float fireRate = 0.2f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Gọi Singleton xin 1 viên đạn từ kho chung
        GameObject bullet = BulletManager.Instance.GetBullet();

        if (bullet != null)
        {
            // Bê đạn ra nòng súng và bóp cò (Hiện đạn lên)
            bullet.transform.position = firePosition.position;
            bullet.transform.rotation = firePosition.rotation;
            bullet.SetActive(true);
        }
    }
}