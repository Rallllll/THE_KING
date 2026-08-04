using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        Shield,
        Health,
        WeaponUpgrade
    }

    [Header("Cài đặt Vật phẩm")]
    public ItemType currentType; // Chọn ở ngoài Inspector
    public int amount = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MainShipStats stats = collision.gameObject.GetComponent<MainShipStats>();

            switch (currentType)
            {
                case ItemType.Shield:
                    //if (stats != null) stats.ActivateShield();
                    break;

                // Các chức năng khác bổ sung sau
                case ItemType.Health:
                    break;
                case ItemType.WeaponUpgrade:
                    break;
            }

            Destroy(gameObject); // Ăn xong thì tự hủy
        }
    }
}