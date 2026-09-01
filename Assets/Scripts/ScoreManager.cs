using UnityEngine;
using TMPro; // Dùng cho TextMeshPro
using UnityEngine.SceneManagement; // Bắt buộc phải có để chuyển màn

public class ScoreManager : MonoBehaviour
{
    // Singleton để các script khác (Quái, Player) dễ dàng gọi đến mà không cần GetComponent
    public static ScoreManager instance;

    [Header("=== GIAO DIỆN ĐANG CHƠI ===")]
    public TextMeshProUGUI inGameScoreText;
    private int currentScore = 0;

    [Header("=== GIAO DIỆN THẮNG (WIN PANEL) ===")]
    public GameObject winPanel;
    public TextMeshProUGUI winScoreText;

    [Tooltip("Kéo 3 cái GameObject Ngôi Sao Vàng (con) vào đây")]
    public GameObject[] starFills;
    public int score1Star = 300; 
    public int score2Star = 600; 
    public int score3Star = 1000; 

    [Header("=== GIAO DIỆN THUA (LOSE PANEL) ===")]
    public GameObject losePanel;
    public TextMeshProUGUI loseScoreText;

    [Header("=== CÀI ĐẶT CHUYỂN SCENE ===")]
    public string nextLevelSceneName = "Level_2"; // Gõ tên Scene tiếp theo
    public string menuSceneName = "MainMenu";     // Gõ tên Scene sảnh chờ

    private void Awake()
    {
        instance = this;
    }

    // ==========================================
    // 1. HÀM CỘNG ĐIỂM (Quái gọi khi nổ tung)
    // ==========================================
    public void AddScore(int amount)
    {
        currentScore += amount;
        if (inGameScoreText != null)
            inGameScoreText.text = currentScore.ToString();
    }

    // ==========================================
    // 2. HÀM GỌI KHI THẮNG (Boss gọi khi nổ tung)
    // ==========================================
    public void ShowWinPanel()
    {
        Time.timeScale = 0f; // Dừng hẳn game lại (đạn, quái đứng im hết)

        winPanel.SetActive(true); // Bật bảng Win
        if (inGameScoreText != null) inGameScoreText.gameObject.SetActive(false); // Giấu điểm góc màn hình đi

        winScoreText.text = currentScore.ToString();

        // Bật sáng Ngôi Sao dựa vào điểm
        if (starFills.Length >= 3)
        {
            starFills[0].SetActive(currentScore >= score1Star); // Lớn hơn mốc 1 -> Bật sao 1
            starFills[1].SetActive(currentScore >= score2Star); // Lớn hơn mốc 2 -> Bật sao 2
            starFills[2].SetActive(currentScore >= score3Star); // Lớn hơn mốc 3 -> Bật sao 3
        }
    }

    // ==========================================
    // 3. HÀM GỌI KHI THUA (Tàu Player gọi khi hết máu)
    // ==========================================
    public void ShowLosePanel()
    {
        Time.timeScale = 0f; // Dừng game

        losePanel.SetActive(true); // Bật bảng Lose
        if (inGameScoreText != null) inGameScoreText.gameObject.SetActive(false);

        loseScoreText.text = currentScore.ToString();
    }

    // ==========================================
    // 4. HÀM CỦA CÁC NÚT BẤM (BUTTON CLICK)
    // ==========================================

    public void Btn_NextLevel()
    {
        Time.timeScale = 1f; // BẮT BUỘC: Phải trả lại thời gian về bình thường trước khi chuyển màn
        SceneManager.LoadScene(nextLevelSceneName);

        // (Nếu ông dùng Prefab Loading thì thay dòng trên bằng: loadingScript.LoadLevel(nextLevelSceneName))
    }

    public void Btn_Replay()
    {
        Time.timeScale = 1f;
        // Tự động lấy tên Scene hiện tại đang chơi và Load lại nó (Reset màn)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Btn_Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}