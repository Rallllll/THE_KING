using UnityEngine;
using UnityEngine.SceneManagement; // BẮT BUỘC PHẢI CÓ DÒNG NÀY ĐỂ CHUYỂN SCENE

public class MainMenu : MonoBehaviour
{
    // ================================
    // HÀM GẮN CHO NÚT START
    // ================================
    public void PlayGame()
    {
        // Có 2 cách load Scene:
        // Cách 1: Ghi đúng tên cái Scene ông muốn chơi (VD: "Level_1")
        // SceneManager.LoadScene("Level_1"); 

        // Cách 2: Load Scene tiếp theo trong danh sách Build (Khuyên dùng)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        Debug.Log("Đang load vào game...");
    }

    // ================================
    // HÀM GẮN CHO NÚT EXIT
    // ================================
    public void QuitGame()
    {
        Debug.Log("Đã thoát game!"); // Hiện trên Console để ông biết nó có chạy

        // Lệnh này sẽ đóng app khi cài lên điện thoại/PC
        // (Lưu ý: Chơi thử trong Unity Editor thì lệnh này sẽ KHÔNG có tác dụng đóng màn hình)
        Application.Quit();
    }
}