using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // Đây chính là Singleton Pattern: Biến Instance độc nhất toàn cục
    public static BulletManager Instance { get; private set; }

    [Header("Cài đặt Pool")]
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private List<GameObject> bulletPool;

    void Awake()
    {
        // Khởi tạo Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Khởi tạo Object Pool
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            // Thêm "transform" để đạn sinh ra được gom gọn làm con của GameManager
            GameObject obj = Instantiate(bulletPrefab, transform);
            obj.SetActive(false);
            bulletPool.Add(obj);
        }
    }

    // Hàm public để bất kỳ ai (Player, Enemy) cũng có thể gọi để xin đạn
    public GameObject GetBullet()
    {
        // 1. Vẫn đi tìm xem có viên nào đang rảnh trong kho không
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet; // Có thì ném ra cho dùng
            }
        }

        // 2. NẾU CHẠY TỚI ĐÂY TỨC LÀ KHO ĐÃ HẾT SẠCH ĐẠN
        // Giải pháp: Không trả về null nữa, mà lập tức đẻ thêm 1 viên mới!
        GameObject newBullet = Instantiate(bulletPrefab, transform);
        newBullet.SetActive(false);

        // Nhét viên mới này vào danh sách kho để lần sau tái sử dụng
        bulletPool.Add(newBullet);

        // Giao viên mới cho súng bắn
        return newBullet;
    }
}
