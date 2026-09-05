using UnityEngine;
using System.Collections;

// Đã bay màu MainGun, thêm Laser!
public enum WeaponType { Cannon, Missile, Laser, Cluster }

public class WeaponController : MonoBehaviour
{
    [Header("Loại súng (CHỌN Ở ĐÂY)")]
    public WeaponType weaponType = WeaponType.Cannon; // Mặc định là Cannon

    [Header("Cài đặt Nòng súng (Bỏ trống nếu là Laser)")]
    public Transform[] firePoints;

    [Header("Cài đặt bắn")]
    public float fireRate = 1f;
    private float fireTimer;

    [Header("Dành riêng cho Laser")]
    public float laserDuration = 2f;
    public GameObject[] beamObjects; // Kéo 2 tia Beam vào đây

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        fireTimer = fireRate;
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            Fire();
            fireTimer = fireRate;
        }
    }

    void Fire()
    {
        // 1. Dù là súng gì cũng phải chạy Animation bóp cò trước
        if (anim != null)
        {
            anim.SetTrigger("Fire");
        }

        // 2. NẾU LÀ LASER: Xong việc! Dừng ở đây chờ Animation Event gọi cái tia Beam ra
        if (weaponType == WeaponType.Laser) return;

        if (weaponType == WeaponType.Laser || weaponType == WeaponType.Missile || weaponType == WeaponType.Cluster) return;

        // 3. NẾU LÀ CANNON / MISSILE: Bắt đầu đi xin đạn
        if (BulletManager.Instance != null && firePoints.Length > 0)
        {
            foreach (Transform fp in firePoints)
            {
                GameObject bullet = null;

                if (weaponType == WeaponType.Cannon)
                    bullet = BulletManager.Instance.GetCannonBullet();
                else if (weaponType == WeaponType.Missile)
                    bullet = BulletManager.Instance.GetMissile();

                if (bullet != null)
                {
                    bullet.transform.position = fp.position;
                    bullet.transform.rotation = fp.rotation;
                    bullet.SetActive(true);
                }
            }
        }
    }

    public void FireMissileEvent(int pointID)
    {
        // Kiểm tra xem ID truyền vào có bị lố giới hạn không
        if (BulletManager.Instance != null && pointID >= 0 && pointID < firePoints.Length)
        {
            GameObject bullet = BulletManager.Instance.GetMissile();
            if (bullet != null)
            {
                Transform fp = firePoints[pointID]; // Lấy đúng tọa độ của nòng số [pointID]
                bullet.transform.position = fp.position;
                bullet.transform.rotation = fp.rotation;
                bullet.SetActive(true);
            }
        }
    }

    // ==========================================
    // CHIÊU THỨC DỪNG ANIMATION & BẬT TIA LASER
    // ==========================================
    public void TurnOnLaserEvent()
    {
        foreach (GameObject beam in beamObjects)
        {
            if (beam != null) beam.SetActive(true);
        }
    }

    // Kết thúc bắn: Gắn Event này ở Frame ông muốn tia Laser biến mất
    public void TurnOffLaserEvent()
    {
        foreach (GameObject beam in beamObjects)
        {
            if (beam != null) beam.SetActive(false);
        }
    }

    public void FireClusterEvent()
    {
        if (BulletManager.Instance != null && firePoints.Length > 0)
        {
            foreach (Transform fp in firePoints)
            {
                GameObject bullet = BulletManager.Instance.GetClusterBullet();
                if (bullet != null)
                {
                    bullet.transform.position = fp.position;
                    bullet.transform.rotation = fp.rotation;

                    // THÊM DÒNG NÀY: Ép đạn làm "con" của nòng súng
                    bullet.transform.SetParent(fp);

                    bullet.SetActive(true);
                }
            }
        }
    }
}