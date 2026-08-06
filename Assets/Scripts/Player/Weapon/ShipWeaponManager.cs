using UnityEngine;

public class ShipWeaponManager : MonoBehaviour
{
    // Cập nhật lại đúng tên 4 loại vũ khí của bạn
    public enum WeaponType { Cannon, Laser, Rocket, Zapper }

    [Header("Kéo 4 vật thể vũ khí con vào đây")]
    public GameObject cannonObject;
    public GameObject laserObject;
    public GameObject rocketObject;
    public GameObject zapperObject;

    void Start()
    {
        // Ẩn hết vũ khí phụ khi mới bắt đầu game
        if (cannonObject != null) cannonObject.SetActive(false);
        if (laserObject != null) laserObject.SetActive(false);
        if (rocketObject != null) rocketObject.SetActive(false);
        if (zapperObject != null) zapperObject.SetActive(false);
    }

    // Hàm mở khóa khi ăn được vật phẩm
    public void UnlockWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Cannon:
                if (cannonObject != null) cannonObject.SetActive(true);
                break;
            case WeaponType.Laser:
                if (laserObject != null) laserObject.SetActive(true);
                break;
            case WeaponType.Rocket:
                if (rocketObject != null) rocketObject.SetActive(true);
                break;
            case WeaponType.Zapper:
                if (zapperObject != null) zapperObject.SetActive(true);
                break;
        }

        Debug.Log("Đã trang bị vũ khí: " + type);
    }
}