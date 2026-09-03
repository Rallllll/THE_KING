using UnityEngine;

public class ShopManage : MonoBehaviour
{
    [Header("Giao diện Shop")]
    public GameObject shopPanel;

    // Bật bảng Shop (Đồng thời ép hệ thống load lại Text cho chắc cú)
    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            if (CurrencyManager.instance != null)
            {
                CurrencyManager.instance.UpdateAllUI();
            }
        }
    }

    // Tắt bảng Shop
    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }

    // ==========================================
    // CÁC Ổ CẮM CHO NÚT MUA HÀNG
    // ==========================================

    // Ví dụ 1: Gắn vào nút "Mua 1000 Vàng giá 10 Kim Cương"
    public void BuyGoldWithDiamonds()
    {
        int diamondCost = 10;
        int goldReward = 1000;

        // Trừ Kim Cương, nếu thành công thì cộng Vàng
        if (CurrencyManager.instance.SpendDiamonds(diamondCost))
        {
            CurrencyManager.instance.AddGold(goldReward);
            Debug.Log("Mua Vàng thành công!");
        }
        else
        {
            Debug.Log("Không đủ Kim Cương!");
        }
    }

    // Ví dụ 2: Gắn vào nút mua đồ (Tốn 500 Vàng)
    public void BuyItemExample()
    {
        int goldCost = 500;

        if (CurrencyManager.instance.SpendGold(goldCost))
        {
            // Code add vật phẩm vào túi đồ ở đây
            Debug.Log("Đã mua thành công vật phẩm!");
        }
        else
        {
            Debug.Log("Không đủ Vàng!");
        }
    }
}