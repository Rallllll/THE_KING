using UnityEngine;
using UnityEngine.UI;

public class WaveProgressBar : MonoBehaviour
{
    public static WaveProgressBar instance; // Gọi tắt từ mọi nơi

    [Header("Thành phần UI")]
    public Slider progressBar;
    public Image[] bossNodes; // Kéo 3 cục Node_1, Node_2, Node_3 vào đây

    [Header("Màu sắc báo hiệu")]
    public Color normalColor = Color.white; // Lúc chưa tới
    public Color warningColor = Color.red;  // Lúc Boss đang xuất hiện
    public Color clearedColor = Color.gray; // Lúc giết Boss xong

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Khởi tạo thanh rỗng
        progressBar.value = 0f;
        foreach (var node in bossNodes)
        {
            node.color = normalColor;
        }
    }

    // Hàm này để làm thanh trượt trôi từ từ
    public void UpdateProgress(float fillAmount)
    {
        progressBar.value = fillAmount;
    }

    // Báo động đỏ khi Boss ra
    public void HighlightBossNode(int waveIndex)
    {
        bossNodes[waveIndex].color = warningColor;
    }

    // Đổi màu xám khi Boss chết
    public void ClearBossNode(int waveIndex)
    {
        bossNodes[waveIndex].color = clearedColor;
    }
}