using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingManage : MonoBehaviour
{
    [Header("Panel Quản Lý")]
    public GameObject settingDarkPanel;

    [Header("Cục Image của 4 Nút Toggle")]
    public Image soundImage;
    public Image vfxImage;
    public Image vibrationImage;
    public Image noticeImage;

    [Header("Hình ảnh lúc BẬT (ON)")]
    public Sprite soundOnSprite;
    public Sprite vfxOnSprite;
    public Sprite vibrationOnSprite;
    public Sprite noticeOnSprite;

    [Header("Hình ảnh lúc TẮT (OFF)")]
    public Sprite soundOffSprite;
    public Sprite vfxOffSprite;
    public Sprite vibrationOffSprite;
    public Sprite noticeOffSprite;

    // Trạng thái lưu trữ (Mặc định cho Bật hết)
    private bool isSoundOn = true;
    private bool isVFXOn = true;
    private bool isVibrationOn = true;
    private bool isNoticeOn = true;

    // ==========================================
    // 1. NHÓM ĐIỀU HƯỚNG PANEL & SCENE
    // ==========================================

    // Gắn vào nút Setting (Bánh răng) ngoài màn hình chơi
    public void OpenSettingsPanel()
    {
        if (settingDarkPanel != null) settingDarkPanel.SetActive(true);
        Time.timeScale = 0f; // Dừng game
    }

    // Gắn vào nút ComeBackButton (Mũi tên quay lại)
    public void CloseSettingsPanel()
    {
        if (settingDarkPanel != null) settingDarkPanel.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game
    }

    // Gắn vào nút HomeButton (Kéo xuống OnClick gõ chuỗi "MAINHUBMENU")
    public void GoToHomeScene(string sceneName)
    {
        Time.timeScale = 1f; // BẮT BUỘC: Phải nhả pause ra trước khi chuyển cảnh, nếu không scene mới sẽ bị đứng hình
        SceneManager.LoadScene(sceneName);
    }

    // ==========================================
    // 2. NHÓM BẬT/TẮT 4 TRẠNG THÁI
    // ==========================================

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        soundImage.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
        // Thêm code xử lý âm thanh thực tế ở đây (VD: AudioListener.volume = 0)
    }

    public void ToggleVFX()
    {
        isVFXOn = !isVFXOn;
        vfxImage.sprite = isVFXOn ? vfxOnSprite : vfxOffSprite;
    }

    public void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;
        vibrationImage.sprite = isVibrationOn ? vibrationOnSprite : vibrationOffSprite;
    }

    public void ToggleNotice()
    {
        isNoticeOn = !isNoticeOn;
        noticeImage.sprite = isNoticeOn ? noticeOnSprite : noticeOffSprite;
    }
}