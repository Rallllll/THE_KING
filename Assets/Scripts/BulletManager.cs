using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    [Header("Kho Đạn Theo Tàu")]
    [Tooltip("Kéo các cục Prefab đạn vào ĐÚNG THỨ TỰ Tàu (ID 0, ID 1, ID 2...)")]
    public GameObject[] bulletPrefabsByID;

    private int currentShipID;
    private List<GameObject> bulletPool = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Vừa vào Game, thủ kho liền đọc xem người chơi đang dùng Tàu ID mấy
        currentShipID = PlayerPrefs.GetInt("EquippedShipID", 0);
    }

    public GameObject GetBullet()
    {
        // Tránh lỗi nếu ông quên chưa kéo đạn vào danh sách
        if (currentShipID < 0 || currentShipID >= bulletPrefabsByID.Length)
        {
            Debug.LogError("Chưa cài đặt đạn cho Tàu ID: " + currentShipID + " trong BulletManager!");
            return null;
        }

        GameObject prefab = bulletPrefabsByID[currentShipID];

        // Tìm xem trong tủ có viên nào đang tắt (rảnh) không
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        // Hết đạn rảnh -> Đẻ thêm 1 viên đúng màu của con tàu đó
        GameObject newBullet = Instantiate(prefab, transform);
        newBullet.SetActive(false);
        bulletPool.Add(newBullet);

        return newBullet;
    }
}