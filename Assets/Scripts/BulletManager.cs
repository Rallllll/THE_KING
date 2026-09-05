using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    [Header("1. Kho Đạn Chính (Theo ID Tàu)")]
    public GameObject[] bulletPrefabsByID;
    private int currentShipID;
    private List<GameObject> mainBulletPool = new List<GameObject>(); // Tủ đạn chính

    [Header("2. Kho Đạn Súng Phụ (Cannon)")]
    public GameObject cannonPrefab;
    private List<GameObject> cannonPool = new List<GameObject>();     // Tủ đạn cannon

    [Header("3. Kho Tên Lửa (Missile)")]
    public GameObject missilePrefab;
    private List<GameObject> missilePool = new List<GameObject>();    // Tủ tên lửa

    [Header("4. Kho đạn Tụ (Cluster - Quả to)")]
    public GameObject clusterBulletPrefab;
    private List<GameObject> clusterPool = new List<GameObject>();    // Tủ đạn chùm

    [Header("5. Kho đạn con (Mini Bullet từ Đạn Chùm)")]
    public GameObject miniBulletPrefab;
    public int miniBulletPoolSize = 60; // Nổ 8 viên 1 lúc nên kho cần to một tí
    private List<GameObject> miniBulletPool;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentShipID = PlayerPrefs.GetInt("EquippedShipID", 0);

        // Tạo sẵn một rổ đạn mini lúc mới vào game cho đỡ giật lag
        miniBulletPool = new List<GameObject>();
        for (int i = 0; i < miniBulletPoolSize; i++)
        {
            // Thêm chữ transform vào để đạn con sinh ra nằm gọn trong object BulletManager
            GameObject obj = Instantiate(miniBulletPrefab, transform);
            obj.SetActive(false);
            miniBulletPool.Add(obj);
        }
    }

    // ==========================================
    // CÁC HÀM ĐỂ SÚNG GỌI RA XIN ĐẠN
    // ==========================================

    public GameObject GetPlayerBullet()
    {
        if (currentShipID < 0 || currentShipID >= bulletPrefabsByID.Length) return null;
        return GetFromPool(mainBulletPool, bulletPrefabsByID[currentShipID]);
    }

    public GameObject GetCannonBullet()
    {
        if (cannonPrefab == null) return null;
        return GetFromPool(cannonPool, cannonPrefab);
    }

    public GameObject GetMissile()
    {
        if (missilePrefab == null) return null;
        return GetFromPool(missilePool, missilePrefab);
    }

    public GameObject GetClusterBullet()
    {
        if (clusterBulletPrefab == null) return null;
        return GetFromPool(clusterPool, clusterBulletPrefab);
    }

    public GameObject GetMiniBullet()
    {
        if (miniBulletPrefab == null) return null;
        // Áp dụng luôn hàm lõi của ông, code rút từ 15 dòng xuống còn đúng 1 dòng này!
        return GetFromPool(miniBulletPool, miniBulletPrefab);
    }

    // ==========================================
    // HÀM LÕI XỬ LÝ XUẤT KHO (DÙNG CHUNG)
    // ==========================================
    private GameObject GetFromPool(List<GameObject> pool, GameObject prefab)
    {
        // 1. Quét dọn các viên đạn bị xóa nhầm (Lỗi MissingReference)
        pool.RemoveAll(item => item == null);

        // 2. Tìm viên đạn rảnh rỗi trong ngăn tủ tương ứng
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // 3. Nếu ngăn tủ hết đạn rảnh -> Đẻ thêm viên mới rồi cất vào tủ đó
        GameObject newObj = Instantiate(prefab, transform);
        newObj.SetActive(false);
        pool.Add(newObj);

        return newObj;
    }

    // ==========================================
    // KHO ĐẠN KẺ ĐỊCH (Enemy Pools)
    // ==========================================
    private Dictionary<string, List<GameObject>> enemyPools = new Dictionary<string, List<GameObject>>();

    public GameObject GetEnemyBullet(GameObject prefab)
    {
        if (prefab == null) return null;
        string key = prefab.name;

        // Nếu chưa có tủ cho loại đạn này -> Đóng tủ mới
        if (!enemyPools.ContainsKey(key))
        {
            enemyPools[key] = new List<GameObject>();
        }

        List<GameObject> pool = enemyPools[key];
        pool.RemoveAll(item => item == null); // Dọn rác

        // Tìm đạn rảnh
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy) return obj;
        }

        // Đẻ đạn mới
        GameObject newObj = Instantiate(prefab, transform);
        newObj.name = key;
        newObj.SetActive(false);
        pool.Add(newObj);

        return newObj;
    }
}