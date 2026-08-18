using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManage : MonoBehaviour
{
    [Header("Các Bảng Giao Diện (Panels)")]
    public GameObject profilePanel;
    public GameObject avatarSelectPanel;
    public GameObject flagSelectPanel;    // [MỚI] Bảng chọn Cờ

    [Header("Khu vực Đổi Tên")]
    public TMP_InputField nameInputField;
    public TextMeshProUGUI mainHubNameText;

    [Header("Khu vực Avatar")]
    public Image mainHubAvatarIcon;
    public Image profileAvatarIcon;
    public Image previewAvatarIcon;
    public Sprite[] avatarDatabase;

    [Header("Khu vực Chọn Cờ (MỚI)")]
    public Image mainHubFlagIcon;         // Ảnh Cờ ngoài sảnh
    public Image profileFlagIcon;         // Ảnh Cờ trong bảng Profile
    public Image previewFlagIcon;         // Ảnh Cờ xem trước trong bảng Chọn Cờ
    public Sprite[] flagDatabase;         // Kho chứa ảnh các loại Cờ

    [Header("Khu vực Tàu (Main Hub)")]
    public Image mainHubShipDisplay;
    public Sprite[] shipDatabase;

    void Start()
    {
        // 1. LẤY DỮ LIỆU ĐÃ LƯU
        string savedName = PlayerPrefs.GetString("PlayerName", "Pilot 777");
        int savedAvatarId = PlayerPrefs.GetInt("AvatarID", 0);
        int savedShipId = PlayerPrefs.GetInt("ShipID", 0);
        int savedFlagId = PlayerPrefs.GetInt("FlagID", 0); // [MỚI] Lấy ID Cờ đã lưu

        // 2. HIỂN THỊ DỮ LIỆU
        nameInputField.text = savedName;
        if (mainHubNameText != null) mainHubNameText.text = savedName;
        nameInputField.onEndEdit.AddListener(OnNameChanged);

        UpdateAvatarDisplay(savedAvatarId);
        UpdateShipDisplay(savedShipId);
        UpdateFlagDisplay(savedFlagId); // [MỚI] Hiển thị Cờ lúc mới vào game
    }

    // =====================================
    // HỆ THỐNG ĐÓNG / MỞ BẢNG
    // =====================================
    public void OpenProfile() { profilePanel.SetActive(true); }
    public void CloseProfile() { profilePanel.SetActive(false); }

    public void OpenAvatarSelect() { avatarSelectPanel.SetActive(true); }
    public void CloseAvatarSelect() { avatarSelectPanel.SetActive(false); }

    // [MỚI] Bật/Tắt bảng chọn Cờ
    public void OpenFlagSelect() { flagSelectPanel.SetActive(true); }
    public void CloseFlagSelect() { flagSelectPanel.SetActive(false); }

    // =====================================
    // HỆ THỐNG ĐỔI TÊN
    // =====================================
    public void OnNameChanged(string newName)
    {
        if (string.IsNullOrEmpty(newName)) newName = "Vô Danh";
        PlayerPrefs.SetString("PlayerName", newName);
        if (mainHubNameText != null) mainHubNameText.text = newName;
    }

    // =====================================
    // HỆ THỐNG ĐỔI AVATAR
    // =====================================
    public void SelectAvatar(int index)
    {
        PlayerPrefs.SetInt("AvatarID", index);
        UpdateAvatarDisplay(index);
    }

    private void UpdateAvatarDisplay(int index)
    {
        if (index >= 0 && index < avatarDatabase.Length)
        {
            Sprite selectedSprite = avatarDatabase[index];
            if (mainHubAvatarIcon != null) mainHubAvatarIcon.sprite = selectedSprite;
            if (profileAvatarIcon != null) profileAvatarIcon.sprite = selectedSprite;
            if (previewAvatarIcon != null) previewAvatarIcon.sprite = selectedSprite;
        }
    }

    // =====================================
    // HỆ THỐNG ĐỔI CỜ (MỚI)
    // =====================================
    // Bấm vào nút Cờ số mấy thì truyền index (0, 1, 2...) vào đây
    public void SelectFlag(int index)
    {
        PlayerPrefs.SetInt("FlagID", index);
        UpdateFlagDisplay(index);
    }

    private void UpdateFlagDisplay(int index)
    {
        if (index >= 0 && index < flagDatabase.Length)
        {
            Sprite selectedSprite = flagDatabase[index];

            if (mainHubFlagIcon != null) mainHubFlagIcon.sprite = selectedSprite;
            if (profileFlagIcon != null) profileFlagIcon.sprite = selectedSprite;
            if (previewFlagIcon != null) previewFlagIcon.sprite = selectedSprite;
        }
    }

    // =====================================
    // HỆ THỐNG ĐỔI TÀU
    // =====================================
    public void SelectShip(int index)
    {
        PlayerPrefs.SetInt("ShipID", index);
        UpdateShipDisplay(index);
    }

    private void UpdateShipDisplay(int index)
    {
        if (index >= 0 && index < shipDatabase.Length)
        {
            if (mainHubShipDisplay != null)
            {
                mainHubShipDisplay.sprite = shipDatabase[index];
                mainHubShipDisplay.SetNativeSize();
            }
        }
    }
}