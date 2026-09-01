using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public TextMeshProUGUI goldText; // Kéo chữ hiển thị Vàng vào đây
    private int currentGold;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Vừa vào Scene là móc ví (PlayerPrefs) ra đếm tiền ngay
        currentGold = PlayerPrefs.GetInt("TotalGold", 0);
        UpdateGoldUI();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        PlayerPrefs.SetInt("TotalGold", currentGold); 
        PlayerPrefs.Save(); 
        UpdateGoldUI();
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = currentGold.ToString(); // Chỉ hiện số
        }
    }
}