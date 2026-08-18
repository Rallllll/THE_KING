using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectionManage : MonoBehaviour
{
    [Header("Bảng Giao Diện")]
    public GameObject mapSelectPanel; // Kéo cái Panel "CHỌN CHIẾN DỊCH" vào đây

    // =====================================
    // 1. HỆ THỐNG MỞ / ĐÓNG BẢNG
    // =====================================

    // Gắn vào nút PLAY to đùng ở ngoài sảnh
    public void OpenMapPanel()
    {
        mapSelectPanel.SetActive(true);
    }

    // Gắn vào nút X (màu đỏ) ở góc trên bảng
    public void CloseMapPanel()
    {
        mapSelectPanel.SetActive(false);
    }

    // =====================================
    // 2. HỆ THỐNG CHUYỂN MÀN CHƠI
    // =====================================

    // Gắn vào các nút "XUẤT KÍCH" màu xanh ở dưới mỗi thẻ
    public void LoadCampaignMap(string sceneName)
    {
        // In ra Console để ông dễ kiểm tra xem nút có hoạt động không
        Debug.Log("Tàu đang chuẩn bị xuất kích đến bản đồ: " + sceneName);

        // Lệnh thần thánh đưa người chơi sang Scene mới
        SceneManager.LoadScene(sceneName);
    }
}
