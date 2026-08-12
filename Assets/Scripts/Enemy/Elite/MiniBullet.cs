using UnityEngine;

public class MiniBullet : MonoBehaviour
{
    public float speed = 6f;
    private float lifeTime = 4f; // Sống 4 giây thì dọn rác

    public int damage = 1;

    void Update()
    {
        // BẮT BUỘC DÙNG Space.Self & Vector3.up (hoặc down) Tùy vào lúc đẻ đạn
        // Tức là cái mũi nó bị xoay hướng nào, nó sẽ lao theo hướng đó
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MainShipStats stats = collision.GetComponent<MainShipStats>();
            if (stats != null)
            {
                stats.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
