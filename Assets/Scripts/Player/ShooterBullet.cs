using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f; // Tốc độ bay của đạn
    private float screenTop;

    void Start()
    {
        // Tự động tính toán mép trên của camera để biết khi nào đạn bay ra ngoài
        screenTop = Camera.main.orthographicSize + 1f;
    }

    void Update()
    {
        // Đạn luôn bay thẳng lên trên
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // NẾU đạn bay vượt quá màn hình -> KHÔNG XÓA (Destroy), mà ẨN ĐI (để tái sử dụng)
        if (transform.position.y > screenTop)
        {
            gameObject.SetActive(false);
        }
    }
}