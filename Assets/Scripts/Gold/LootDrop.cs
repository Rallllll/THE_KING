using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [Header("Cài đặt rớt Vàng")]
    public GameObject coinPrefab;
    [Range(0, 100)] public int coinDropChance = 50;
    public int goldAmount = 10;

    [Header("Cài đặt rớt Máu (Heal)")]
    public GameObject healPrefab; // Kéo Prefab cục Heal vào đây
    [Range(0, 100)] public int healDropChance = 20; // Tỉ lệ rớt cục máu (VD: 20%)

    // Đổi tên hàm thành DropLoot cho tổng quát (Vẫn giữ nguyên chức năng)
    public void DropCoin()
    {
        // 1. Tìm tàu Player trên màn hình để kiểm tra máu
        MainShipStats playerStats = FindAnyObjectByType<MainShipStats>();
        bool isMaxHealth = false;

        if (playerStats != null)
        {
            isMaxHealth = (playerStats.currentHP >= playerStats.maxHP);
        }

        // 2. NẾU CHƯA ĐẦY MÁU -> Quay xổ số rớt cục Heal trước
        if (!isMaxHealth && Random.Range(0, 100) < healDropChance)
        {
            if (healPrefab != null)
            {
                Instantiate(healPrefab, transform.position, Quaternion.identity);
                return; // Đã rớt máu rồi thì ngắt luôn, KHÔNG rớt vàng nữa
            }
        }

        // 3. NẾU ĐÃ ĐẦY MÁU (Hoặc quay trượt cục Heal) -> Quay xổ số rớt Vàng
        if (Random.Range(0, 100) < coinDropChance)
        {
            if (coinPrefab != null)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

                CoinPickup coinScript = coin.GetComponent<CoinPickup>();
                if (coinScript != null) coinScript.goldValue = goldAmount;
            }
        }
    }
}