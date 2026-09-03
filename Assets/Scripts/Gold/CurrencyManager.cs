using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public TextMeshProUGUI goldText;
    private int currentGold;

    [Header("--- HACK TIỀN (TEST INSPECTOR) ---")]
    [Tooltip("Nhập số vàng ông muốn thêm hoặc cài đặt vào đây")]
    public int debugGoldAmount = 5000;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            PlayerPrefs.SetInt("TotalGold", currentGold);
            PlayerPrefs.Save();
            UpdateGoldUI();
            return true;
        }
        return false;
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = currentGold.ToString();
        }
    }

    // ==========================================
    // CÁC HÀM NÀY SẼ HIỆN LÊN KHI CLICK CHUỘT PHẢI VÀO SCRIPT
    // ==========================================

    [ContextMenu("💰 CỘNG THÊM vàng (Theo ô Debug)")]
    public void DebugAddGold()
    {
        AddGold(debugGoldAmount);
        Debug.Log("Đã hack thêm: " + debugGoldAmount + " vàng!");
    }

    [ContextMenu("🎯 CÀI ĐẶT vàng bằng đúng số này")]
    public void DebugSetGold()
    {
        currentGold = debugGoldAmount;
        PlayerPrefs.SetInt("TotalGold", currentGold);
        PlayerPrefs.Save();
        UpdateGoldUI();
        Debug.Log("Đã set vàng thành: " + debugGoldAmount);
    }

    [ContextMenu("🔥 XÓA SẠCH vàng (Về 0)")]
    public void DebugResetGold()
    {
        currentGold = 0;
        PlayerPrefs.SetInt("TotalGold", 0);
        PlayerPrefs.Save();
        UpdateGoldUI();
        Debug.Log("Đã reset vàng về 0!");
    }
}