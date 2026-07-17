using UnityEngine;

public class MainShipMove : MonoBehaviour
{
    [Header("Di Chuyển")]
    public float moveSpeed = 15f;
    public float offsetY = 1.0f;

    [Header("Hiệu Ứng Lật Cánh (Banking)")]
    public float tiltAmount = 30f; // Góc lật tối đa (có thể tăng lên 40-50 nếu muốn lật sâu hơn)
    public float tiltSpeed = 10f;  // Tốc độ lật cánh

    private float minX, maxX, minY, maxY;

    void Start()
    {
        // Tự động tính giới hạn màn hình
        Camera cam = Camera.main;
        maxY = cam.orthographicSize;
        minY = -maxY;
        maxX = cam.orthographicSize * cam.aspect;
        minX = -maxX;
    }

    void Update()
    {
        Vector3 inputPosition = Vector3.zero;
        bool isInteracting = false;

        // 1. Nhận lệnh điều khiển từ Cảm ứng hoặc Chuột
        if (Input.touchCount > 0)
        {
            inputPosition = Input.GetTouch(0).position;
            isInteracting = true;
        }
        else if (Input.GetMouseButton(0))
        {
            inputPosition = Input.mousePosition;
            isInteracting = true;
        }

        // Mặc định góc lật cánh là 0 (bay thăng bằng)
        float targetRotationY = 0f;

        // 2. Xử lý Di chuyển và Tính toán góc lật
        if (isInteracting)
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(inputPosition);
            targetPosition.z = 0;
            targetPosition.y += offsetY;

            // Tính khoảng cách tàu đang lướt đi theo trục X
            float deltaX = targetPosition.x - transform.position.x;

            // Khóa deltaX lại để góc nghiêng không bị lố khi vuốt quá xa
            deltaX = Mathf.Clamp(deltaX, -2f, 2f);

            // TÍNH GÓC LẬT TRỤC Y
            // Lưu ý: Nếu tàu lật ngược chiều bạn muốn, hãy xóa dấu trừ (-) đi
            targetRotationY = deltaX * -tiltAmount;

            // Di chuyển vị trí (Lerp cho mượt)
            Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Giữ tàu trong màn hình
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

            transform.position = newPos;
        }

        // 3. ÁP DỤNG GÓC LẬT VÀO TÀU
        // Chú ý: Đưa targetRotationY vào tham số thứ 2 (Trục Y)
        Quaternion targetRotation = Quaternion.Euler(0f, targetRotationY, 0f);

        // Cập nhật góc quay một cách mượt mà
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }
}