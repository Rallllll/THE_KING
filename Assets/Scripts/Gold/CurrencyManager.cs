using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [Header("=== GIAO DIỆN HIỂN THỊ ===")]
    [Tooltip("Kéo TẤT CẢ các Text Vàng (ở màn hình chính, trong shop...) vào đây")]
    public TextMeshProUGUI[] goldTexts;

    [Tooltip("Kéo TẤT CẢ các Text Kim Cương vào đây")]
    public TextMeshProUGUI[] diamondTexts;

    private int currentGold;
    private int currentDiamonds; // Thêm biến lưu Kim Cương

    [Header("--- HACK TIỀN (TEST INSPECTOR) ---")]
    public int debugGoldAmount = 5000;
    public int debugDiamondAmount = 1000;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Load cả Vàng và Kim Cương
        currentGold = PlayerPrefs.GetInt("TotalGold", 0);
        currentDiamonds = PlayerPrefs.GetInt("TotalDiamonds", 0);
        UpdateAllUI();
    }

    // --- QUẢN LÝ VÀNG ---
    public void AddGold(int amount)
    {
        currentGold += amount;
        PlayerPrefs.SetInt("TotalGold", currentGold);
        UpdateAllUI();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            PlayerPrefs.SetInt("TotalGold", currentGold);
            UpdateAllUI();
            return true;
        }
        return false;
    }

    // --- QUẢN LÝ KIM CƯƠNG ---
    public void AddDiamonds(int amount)
    {
        currentDiamonds += amount;
        PlayerPrefs.SetInt("TotalDiamonds", currentDiamonds);
        UpdateAllUI();
    }

    public bool SpendDiamonds(int amount)
    {
        if (currentDiamonds >= amount)
        {
            currentDiamonds -= amount;
            PlayerPrefs.SetInt("TotalDiamonds", currentDiamonds);
            UpdateAllUI();
            return true;
        }
        return false;
    }

    // Cập nhật đồng loạt TẤT CẢ các Text trên mọi màn hình
    public void UpdateAllUI()
    {
        PlayerPrefs.Save();

        foreach (var text in goldTexts)
        {
            if (text != null) text.text = currentGold.ToString();
        }

        foreach (var text in diamondTexts)
        {
            if (text != null) text.text = currentDiamonds.ToString();
        }
    }

    // --- HACK TOOL ---
    [ContextMenu("💰 CỘNG THÊM Vàng & Kim Cương")]
    public void DebugAddCurrency()
    {
        AddGold(debugGoldAmount);
        AddDiamonds(debugDiamondAmount);
        Debug.Log("Đã hack tiền!");
    }
}