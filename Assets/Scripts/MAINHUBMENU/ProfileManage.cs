using UnityEngine;
using UnityEngine.UI;
using TMPro; // Bắt buộc phải có để dùng InputField gõ tên

public class ProfileManage : MonoBehaviour
{
    [Header("Các Bảng Giao Diện (Panels)")]
    public GameObject profilePanel;
    public GameObject avatarSelectPanel;

    [Header("Khu vực Đổi Tên")]
    public TMP_InputField nameInputField;
    public TextMeshProUGUI mainHubNameText; // Chữ tên hiển thị ngoài sảnh (Cạnh nút Avatar góc trái)

    [Header("Khu vực Avatar")]
    public Image mainHubAvatarIcon;       // Nút Avatar góc trái ngoài sảnh
    public Image profileAvatarIcon;       // Nút Avatar to bên trong bảng Profile

    // THÊM BIẾN NÀY DÀNH CHO CÁI ẢNH PREVIEW TRONG BẢNG CHANGE AVATAR
    public Image previewAvatarIcon;

    public Sprite[] avatarDatabase;       // Danh sách tất cả ảnh Avatar ông có

    [Header("Khu vực Tàu (Main Hub)")]
    // Tùy ông dùng Image (UI) hay SpriteRenderer (2D) ngoài sảnh. Ở đây tôi ví dụ dùng UI Image.
    public Image mainHubShipDisplay;
    public Sprite[] shipDatabase;         // Danh sách ảnh Tàu ông có

    void Start()
    {
        // 1. LẤY DỮ LIỆU ĐÃ LƯU TỪ LẦN TRƯỚC (Hoặc gán mặc định nếu mới chơi)
        string savedName = PlayerPrefs.GetString("PlayerName", "Pilot 777");
        int savedAvatarId = PlayerPrefs.GetInt("AvatarID", 0);
        int savedShipId = PlayerPrefs.GetInt("ShipID", 0);

        // 2. HIỂN THỊ DỮ LIỆU LÊN MÀN HÌNH
        // Tên
        nameInputField.text = savedName;
        if (mainHubNameText != null) mainHubNameText.text = savedName;

        // Cập nhật lại Event khi gõ chữ xong
        nameInputField.onEndEdit.AddListener(OnNameChanged);

        // Avatar & Tàu
        UpdateAvatarDisplay(savedAvatarId);
        UpdateShipDisplay(savedShipId);
    }

    // =====================================
    // HỆ THỐNG ĐÓNG / MỞ BẢNG
    // =====================================
    public void OpenProfile() { profilePanel.SetActive(true); }
    public void CloseProfile() { profilePanel.SetActive(false); }

    public void OpenAvatarSelect() { avatarSelectPanel.SetActive(true); }
    public void CloseAvatarSelect() { avatarSelectPanel.SetActive(false); }

    // =====================================
    // HỆ THỐNG ĐỔI TÊN
    // =====================================
    public void OnNameChanged(string newName)
    {
        if (string.IsNullOrEmpty(newName)) newName = "Vô Danh"; // Tránh trường hợp xóa trắng tên

        // Lưu tên vào máy
        PlayerPrefs.SetString("PlayerName", newName);

        // Cập nhật chữ ngoài sảnh
        if (mainHubNameText != null) mainHubNameText.text = newName;
    }

    // =====================================
    // HỆ THỐNG ĐỔI AVATAR
    // =====================================
    // Bấm vào nút Avatar số mấy thì truyền index (0, 1, 2...) vào đây
    public void SelectAvatar(int index)
    {
        // 1. Lưu con số của Avatar này vào máy
        PlayerPrefs.SetInt("AvatarID", index);

        // 2. Cập nhật hình ảnh ở cả 3 nơi (Sảnh, Hồ sơ, và Cái ảnh Preview)
        UpdateAvatarDisplay(index);
    }

    private void UpdateAvatarDisplay(int index)
    {
        if (index >= 0 && index < avatarDatabase.Length)
        {
            Sprite selectedSprite = avatarDatabase[index];
            if (mainHubAvatarIcon != null) mainHubAvatarIcon.sprite = selectedSprite;
            if (profileAvatarIcon != null) profileAvatarIcon.sprite = selectedSprite;

            // THÊM DÒNG NÀY ĐỂ NÓ ĐỔI CẢ ẢNH PREVIEW TRONG BẢNG CHỌN
            if (previewAvatarIcon != null) previewAvatarIcon.sprite = selectedSprite;
        }
    }

    // =====================================
    // HỆ THỐNG ĐỔI TÀU
    // =====================================
    // Bấm vào nút Tàu số mấy thì truyền index vào đây
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
                // Lệnh set native size để lỡ tàu to tàu nhỏ nó không bị méo (chỉ dùng nếu là UI Image)
                mainHubShipDisplay.SetNativeSize();
            }
        }
    }
}