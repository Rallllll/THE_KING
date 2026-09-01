using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int goldValue = 10;
    public float fallSpeed = 3f;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (CurrencyManager.instance != null)
            {
                CurrencyManager.instance.AddGold(goldValue);
            }
            Destroy(gameObject);
        }
    }
}