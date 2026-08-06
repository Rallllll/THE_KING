using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    [Header("Cài đặt Vật phẩm")]
    public float moveSpeed = 2f;

    // Chọn loại vũ khí muốn mở khóa ngay trên Inspector
    public ShipWeaponManager.WeaponType weaponToUnlock;

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (transform.position.y < -10f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ShipWeaponManager weaponManager = collision.gameObject.GetComponent<ShipWeaponManager>();

            if (weaponManager != null)
            {
                // Báo cho tàu biết để bật vũ khí tương ứng lên
                weaponManager.UnlockWeapon(weaponToUnlock);
            }

            Destroy(gameObject);
        }
    }
}