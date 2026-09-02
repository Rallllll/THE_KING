using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [Header("Cài đặt rớt Vàng")]
    public GameObject coinPrefab;
    public int coinDropChance = 50;
    public int goldAmount = 10;

    [Header("Cài đặt rớt Máu (Heal)")]
    public GameObject healPrefab;
    public int healDropChance = 20;

    [Header("Cài đặt rớt Giáp (Shield)")]
    public GameObject shieldPrefab;
    public int shieldDropChance = 10;

    public void DropCoin()
    {
        MainShipStats playerStats = FindAnyObjectByType<MainShipStats>();
        bool isMaxHealth = false;

        if (playerStats != null)
        {
            isMaxHealth = (playerStats.currentHP >= playerStats.maxHP);
        }

        // 1. Tự động loại bỏ tỉ lệ rớt Máu nếu tàu đã đầy máu
        int currentHealChance = isMaxHealth ? 0 : healDropChance;

        // 2. Tung một viên xúc xắc DUY NHẤT (từ 0 đến 99)
        int roll = Random.Range(0, 100);

        // 3. Phân chia chiếc bánh 100% (Đảm bảo chỉ rớt 1 món)
        if (roll < currentHealChance)
        {
            // Trúng ô Máu
            if (healPrefab != null) Instantiate(healPrefab, transform.position, Quaternion.identity);
        }
        else if (roll < currentHealChance + shieldDropChance)
        {
            // Trúng ô Giáp
            if (shieldPrefab != null) Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        }
        else if (roll < currentHealChance + shieldDropChance + coinDropChance)
        {
            // Trúng ô Vàng
            if (coinPrefab != null)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                CoinPickup coinScript = coin.GetComponent<CoinPickup>();
                if (coinScript != null) coinScript.goldValue = goldAmount;
            }
        }
    }
}