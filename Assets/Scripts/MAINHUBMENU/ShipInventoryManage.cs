using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ShipData
{
    public string shipName;
    public Sprite shipImage;

    [Header("Chỉ số cơ bản")]
    public int health;
    public int armor;
    public int speed;
    public int damage;

    [Header("Mở khóa")]
    public int price;
    public bool isUnlockedByDefault;
}

public class ShipInventoryManage : MonoBehaviour
{
    [Header("Dữ Liệu Tàu")]
    public ShipData[] shipDatabase;
    private int currentViewingIndex = 0;
    private int equippedShipIndex = 0;

    [Header("=== BẢNG QUẢN LÝ ===")]
    public GameObject shipManagePanel;
    public Image shipPreviewImage;
    public TextMeshProUGUI shipNameText;

    [Header("Hệ thống Nền & Nút Scroll")]
    public Image manageBackgroundImage;
    public Sprite bgUnlocked;
    public Sprite bgLocked;
    public Button[] scrollButtons;

    [Header("Hệ thống 3 Nút Bấm")]
    public Button btnBuy;
    public Button btnEquip;
    public Button btnUpgrade;
    public TextMeshProUGUI priceText;

    [Header("Text hiển thị chỉ số")]
    public TextMeshProUGUI statHealthText;
    public TextMeshProUGUI statArmorText;
    public TextMeshProUGUI statSpeedText;
    public TextMeshProUGUI statDamageText;

    [Header("=== MÀN HÌNH CHÍNH ===")]
    public GameObject[] mainScreenShipImages;

    [Header("=== BẢNG NÂNG CẤP ===")]
    public GameObject shipUpgradePanel;

    void Start()
    {
        equippedShipIndex = PlayerPrefs.GetInt("EquippedShipID", 0);

        for (int i = 0; i < shipDatabase.Length; i++)
        {
            if (shipDatabase[i].isUnlockedByDefault)
            {
                PlayerPrefs.SetInt("ShipUnlocked_" + i, 1);
            }
        }

        UpdateScrollButtonsVisual();
        UpdateMainScreenImage();
        ViewShip(equippedShipIndex);
    }

    public void ViewShip(int shipIndex)
    {
        if (shipIndex < 0 || shipIndex >= shipDatabase.Length) return;

        currentViewingIndex = shipIndex;
        ShipData data = shipDatabase[shipIndex];

        if (shipPreviewImage != null) shipPreviewImage.sprite = data.shipImage;
        if (shipNameText != null) shipNameText.text = data.shipName;

        bool isUnlocked = PlayerPrefs.GetInt("ShipUnlocked_" + shipIndex, 0) == 1;

        if (manageBackgroundImage != null) manageBackgroundImage.sprite = isUnlocked ? bgUnlocked : bgLocked;

        if (btnBuy != null) btnBuy.gameObject.SetActive(!isUnlocked);
        if (btnEquip != null) btnEquip.interactable = isUnlocked;
        if (btnUpgrade != null) btnUpgrade.interactable = isUnlocked;

        if (!isUnlocked && priceText != null) priceText.text = data.price.ToString();

        // --- BẬT/TẮT HIỂN THỊ CHỈ SỐ ---
        if (statHealthText != null) statHealthText.gameObject.SetActive(isUnlocked);
        if (statArmorText != null) statArmorText.gameObject.SetActive(isUnlocked);
        if (statSpeedText != null) statSpeedText.gameObject.SetActive(isUnlocked);
        if (statDamageText != null) statDamageText.gameObject.SetActive(isUnlocked);

        // Chỉ tính toán và in chữ nếu đã mở khóa
        if (isUnlocked)
        {
            int upgHealth = PlayerPrefs.GetInt("Upg_Health_" + shipIndex, 0);
            int upgArmor = PlayerPrefs.GetInt("Upg_Armor_" + shipIndex, 0);
            int upgSpeed = PlayerPrefs.GetInt("Upg_Speed_" + shipIndex, 0);
            int upgDamage = PlayerPrefs.GetInt("Upg_Damage_" + shipIndex, 0);

            if (statHealthText != null) statHealthText.text = "Health " + (data.health + upgHealth);
            if (statArmorText != null) statArmorText.text = "Armor " + (data.armor + upgArmor);
            if (statSpeedText != null) statSpeedText.text = "Speed " + (data.speed + upgSpeed);
            if (statDamageText != null) statDamageText.text = "Damage " + (data.damage + upgDamage);
        }
    }

    private void UpdateScrollButtonsVisual()
    {
        for (int i = 0; i < scrollButtons.Length; i++)
        {
            if (i >= shipDatabase.Length) break;
            bool isUnlocked = PlayerPrefs.GetInt("ShipUnlocked_" + i, 0) == 1;
            Image[] allImagesInButton = scrollButtons[i].GetComponentsInChildren<Image>();
            foreach (Image img in allImagesInButton)
            {
                img.color = isUnlocked ? Color.white : new Color(0.3f, 0.3f, 0.3f, 0.8f);
            }
        }
    }

    public void BuyViewingShip()
    {
        ShipData data = shipDatabase[currentViewingIndex];

        if (CurrencyManager.instance != null && CurrencyManager.instance.SpendGold(data.price))
        {
            PlayerPrefs.SetInt("ShipUnlocked_" + currentViewingIndex, 1);
            PlayerPrefs.Save();

            UpdateScrollButtonsVisual();
            ViewShip(currentViewingIndex);
        }
    }

    public void EquipViewingShip()
    {
        equippedShipIndex = currentViewingIndex;
        PlayerPrefs.SetInt("EquippedShipID", equippedShipIndex);
        UpdateMainScreenImage();
    }

    private void UpdateMainScreenImage()
    {
        for (int i = 0; i < mainScreenShipImages.Length; i++)
        {
            if (mainScreenShipImages[i] != null)
            {
                mainScreenShipImages[i].SetActive(i == equippedShipIndex);
            }
        }
    }

    public void OpenShipManage()
    {
        if (shipManagePanel != null)
        {
            shipManagePanel.SetActive(true);
            UpdateScrollButtonsVisual();
            ViewShip(equippedShipIndex);
        }
    }

    public void CloseShipManage() { shipManagePanel.SetActive(false); }
    public void OpenShipUpgrade() { shipUpgradePanel.SetActive(true); }
    public void CloseShipUpgrade() { shipUpgradePanel.SetActive(false); }

    public void UpgradeStat_Health()
    {
        int currentLvl = PlayerPrefs.GetInt("Upg_Health_" + currentViewingIndex, 0);
        PlayerPrefs.SetInt("Upg_Health_" + currentViewingIndex, currentLvl + 10);
        ViewShip(currentViewingIndex);
    }

    public void UpgradeStat_Damage()
    {
        int currentLvl = PlayerPrefs.GetInt("Upg_Damage_" + currentViewingIndex, 0);
        PlayerPrefs.SetInt("Upg_Damage_" + currentViewingIndex, currentLvl + 5);
        ViewShip(currentViewingIndex);
    }
}