using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Tốc độ cuộn")]
    public float scrollSpeed = 0.5f;

    private Material bgMaterial;
    private Vector2 offset;
    private string texturePropertyName = "_MainTex"; // Mặc định của Unity cũ

    void Start()
    {
        // Lấy Material đang gắn trên vật thể
        bgMaterial = GetComponent<Renderer>().material;

        // Tự động quét xem Unity có đang dùng URP không
        if (bgMaterial.HasProperty("_BaseMap"))
        {
            texturePropertyName = "_BaseMap"; // Đổi sang chuẩn URP
        }
    }

    void Update()
    {
        // Tính toán độ trượt
        offset.y += scrollSpeed * Time.deltaTime;

        // Ép bề mặt ảnh trượt đi theo đúng chuẩn Shader
        bgMaterial.SetTextureOffset(texturePropertyName, offset);
    }
}