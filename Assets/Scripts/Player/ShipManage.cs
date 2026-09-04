using UnityEngine;

public class ShipManage : MonoBehaviour
{
    [Header("Danh Sách Tàu Trong Game")]
    [Tooltip("Kéo các con tàu ở Scene Game vào đây ĐÚNG THỨ TỰ (0, 1, 2...) giống hệt bên Shop")]
    public GameObject[] playableShips;

    void Start()
    {
        // 1. Lôi vé xuất bến (ID tàu) mà người chơi đã trang bị ở Menu ra
        int equippedID = PlayerPrefs.GetInt("EquippedShipID", 0);

        // 2. Duyệt qua toàn bộ danh sách tàu có trong Scene
        for (int i = 0; i < playableShips.Length; i++)
        {
            if (playableShips[i] != null)
            {
                // Nếu đúng ID đã chọn -> BẬT tàu đó lên
                if (i == equippedID)
                {
                    playableShips[i].SetActive(true);
                }
                // Còn lại -> TẮT hết đi
                else
                {
                    playableShips[i].SetActive(false);
                }
            }
        }
    }
}