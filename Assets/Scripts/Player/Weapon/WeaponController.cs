using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    [Header("Cài đặt Đạn & Nòng súng")]
    public GameObject bulletPrefab;

    // Đã nâng cấp thành Mảng (Array) để bạn có thể thêm bao nhiêu nòng súng tùy thích
    public Transform[] firePoints;

    [Header("Cài đặt bắn")]
    public float fireRate = 1f;
    private float fireTimer;

    [Header("Dành riêng cho Laser")]
    public float laserDuration = 2f;

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
        if (anim != null)
        {
            anim.SetTrigger("Fire");
        }

        // Kiểm tra xem có đạn và có nòng súng nào trong danh sách không
        if (bulletPrefab != null && firePoints.Length > 0)
        {
            // Vòng lặp: Duyệt qua toàn bộ các nòng súng có trong danh sách
            // và đẻ ra 1 viên đạn ở đúng vị trí, góc xoay của từng nòng
            foreach (Transform fp in firePoints)
            {
                Instantiate(bulletPrefab, fp.position, fp.rotation);
            }
        }
        else
        {
            Debug.LogWarning("Chưa gắn Bullet Prefab hoặc chưa có Fire Point nào cho súng!");
        }
    }

    // ==========================================
    // CHIÊU THỨC DỪNG ANIMATION DÀNH CHO LASER
    // ==========================================
    public void HoldLaserEvent()
    {
        StartCoroutine(LaserRoutine());
    }

    private IEnumerator LaserRoutine()
    {
        anim.speed = 0f;
        yield return new WaitForSeconds(laserDuration);
        anim.speed = 1f;
    }
}