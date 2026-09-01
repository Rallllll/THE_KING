using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public GameObject coinPrefab; // Kéo Prefab cục vàng vào đây

    [Header("Cài đặt rớt vàng")]
    [Range(0, 100)]
    public int dropChance = 50;   // Xác suất rớt (50%)
    public int goldAmount = 10;   // Cục vàng này trị giá bao nhiêu?

    public void DropCoin()
    {
        if (coinPrefab != null)
        {
            // Random từ 0 đến 99. Nếu nhỏ hơn dropChance thì rớt
            if (Random.Range(0, 100) < dropChance)
            {
                // Đẻ ra cục vàng
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

                // Báo cho cục vàng biết nó mang giá trị bao nhiêu
                CoinPickup coinScript = coin.GetComponent<CoinPickup>();
                if (coinScript != null) coinScript.goldValue = goldAmount;
            }
        }
    }
}