using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Tạo một "khuôn" để chứa thông tin của từng chiếc Tàu
[System.Serializable]
public class ShipData
{
    public string shipName;
    public Sprite shipImage; // Ảnh tàu dùng cho cả Scroll và Preview

    // 4 Chỉ số cơ bản
    public int hp;
    public int attack;
    public int speed;
    public int fireRate;
}

public class ShipInventoryManage : MonoBehaviour
{
    [Header("Dữ Liệu Tàu")]
    public ShipData[] shipDatabase; // Nơi ông nhét 5-10 chiếc tàu vào
    private int currentViewingIndex = 0; // Tàu đang xem trên màn hình
    private int equippedShipIndex = 0;   // Tàu đang được CHỌN (tick) để chơi

    [Header("=== BẢNG QUẢN LÝ (SHIP MANAGE) ===")]
    public GameObject shipManagePanel;
    public Image shipPreviewImage;
    public TextMeshProUGUI shipNameText;

    [Header("Text hiển thị 4 chỉ số")]
    public TextMeshProUGUI stat1_HP;
    public TextMeshProUGUI stat2_Attack;
    public TextMeshProUGUI stat3_Speed;
    public TextMeshProUGUI stat4_FireRate;

    [Header("=== BẢNG NÂNG CẤP (SHIP UPGRADE) ===")]
    public GameObject shipUpgradePanel;
    // (Ông có thể thêm Text hiển thị tiền hoặc level nâng cấp ở đây sau)

    void Start()
    {
        // Lấy con tàu đang được trang bị từ lần chơi trước
        equippedShipIndex = PlayerPrefs.GetInt("EquippedShipID", 0);

        // Mặc định bật lên là xem con tàu mình đang chọn
        ViewShip(equippedShipIndex);
    }

    // ===============================================
    // KHU VỰC 1: SCROLL VIEW CHỌN TÀU ĐỂ XEM
    // ===============================================

    // Gắn hàm này vào các Button Tàu ở trên thanh Scroll (Truyền số 0, 1, 2...)
    public void ViewShip(int shipIndex)
    {
        if (shipIndex >= 0 && shipIndex < shipDatabase.Length)
        {
            currentViewingIndex = shipIndex;
            ShipData data = shipDatabase[shipIndex];

            // Cập nhật ảnh và tên vào khung Preview
            if (shipPreviewImage != null) shipPreviewImage.sprite = data.shipImage;
            if (shipNameText != null) shipNameText.text = data.shipName;

            // Cập nhật 4 thông số (Cộng thêm phần đã nâng cấp nếu có)
            // Lấy level nâng cấp từ PlayerPrefs (mặc định là 0 nếu chưa nâng)
            int upgHP = PlayerPrefs.GetInt("Upg_HP_" + shipIndex, 0);
            int upgAtk = PlayerPrefs.GetInt("Upg_Atk_" + shipIndex, 0);
            int upgSpd = PlayerPrefs.GetInt("Upg_Spd_" + shipIndex, 0);
            int upgFire = PlayerPrefs.GetInt("Upg_Fire_" + shipIndex, 0);

            if (stat1_HP != null) stat1_HP.text = "HP: " + (data.hp + upgHP);
            if (stat2_Attack != null) stat2_Attack.text = "CÔNG: " + (data.attack + upgAtk);
            if (stat3_Speed != null) stat3_Speed.text = "TỐC: " + (data.speed + upgSpd);
            if (stat4_FireRate != null) stat4_FireRate.text = "ĐẠN: " + (data.fireRate + upgFire);
        }
    }

    // ===============================================
    // KHU VỰC 2: 3 NÚT DƯỚI CÙNG CỦA SHIP MANAGE
    // ===============================================

    // Nút 1: Thoát Bảng Manage
    public void CloseShipManage()
    {
        shipManagePanel.SetActive(false);
    }

    // Nút 2: Mở Bảng Upgrade (Mở đè lên)
    public void OpenShipUpgrade()
    {
        shipUpgradePanel.SetActive(true);
    }

    // Nút 3: Nút TICK để chọn tàu này đem ra chiến đấu
    public void EquipViewingShip()
    {
        equippedShipIndex = currentViewingIndex;
        PlayerPrefs.SetInt("EquippedShipID", equippedShipIndex);
        Debug.Log("Đã trang bị tàu: " + shipDatabase[equippedShipIndex].shipName);
        // Ở đây ông có thể làm thêm 1 cái text báo "Đã trang bị!" cho người chơi biết
    }

    // ===============================================
    // KHU VỰC 3: 5 NÚT TRONG BẢNG SHIP UPGRADE
    // ===============================================

    // Nút Thoát của bảng Upgrade
    public void CloseShipUpgrade()
    {
        shipUpgradePanel.SetActive(false);
    }

    // Nút Nâng cấp 1: HP
    public void UpgradeStat_HP()
    {
        int currentLvl = PlayerPrefs.GetInt("Upg_HP_" + currentViewingIndex, 0);
        PlayerPrefs.SetInt("Upg_HP_" + currentViewingIndex, currentLvl + 10); // Mỗi lần nâng +10 máu
        ViewShip(currentViewingIndex); // Load lại màn hình để số nhảy ngay lập tức
    }

    // Nút Nâng cấp 2: Tấn Công
    public void UpgradeStat_Attack()
    {
        int currentLvl = PlayerPrefs.GetInt("Upg_Atk_" + currentViewingIndex, 0);
        PlayerPrefs.SetInt("Upg_Atk_" + currentViewingIndex, currentLvl + 5);
        ViewShip(currentViewingIndex);
    }

    // Nút Nâng cấp 3: Tốc Độ
    public void UpgradeStat_Speed()
    {
        int currentLvl = PlayerPrefs.GetInt("Upg_Spd_" + currentViewingIndex, 0);
        PlayerPrefs.SetInt("Upg_Spd_" + currentViewingIndex, currentLvl + 2);
        ViewShip(currentViewingIndex);
    }

    // Nút Nâng cấp 4: Tốc độ bắn
    public void UpgradeStat_FireRate()
    {
        int currentLvl = PlayerPrefs.GetInt("Upg_Fire_" + currentViewingIndex, 0);
        PlayerPrefs.SetInt("Upg_Fire_" + currentViewingIndex, currentLvl + 1);
        ViewShip(currentViewingIndex);
    }
    public void OpenShipManage()
    {
        if (shipManagePanel != null)
        {
            shipManagePanel.SetActive(true);

            // Tùy chọn thêm: Khi vừa mở bảng lên, tự động hiển thị con tàu đang được trang bị
            ViewShip(equippedShipIndex);
        }
    }
}