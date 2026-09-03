using UnityEngine;
using UnityEngine.Events;

public class CheckMoneyWhenBuy : MonoBehaviour
{
    public enum LoaiTien { Vang, KimCuong }

    [Header("=== CÀI ĐẶT GIAO DỊCH ===")]
    public LoaiTien loaiTien;
    public int giaTien;

    [Header("Chạy các hàm này nếu ĐỦ TIỀN")]
    public UnityEvent OnGiaoDichThanhCong;

    [Header("Chạy các hàm này nếu THIẾU TIỀN (Tùy chọn)")]
    public UnityEvent OnGiaoDichThatBai;

    // Kéo hàm này vào sự kiện OnClick() của Button
    public void ThucHienThanhToan()
    {
        if (CurrencyManager.instance == null)
        {
            Debug.LogError("Chưa có CurrencyManager trong Scene!");
            return;
        }

        bool isSuccess = false;

        // Check và trừ tiền theo loại ông chọn ngoài Inspector
        if (loaiTien == LoaiTien.Vang)
        {
            isSuccess = CurrencyManager.instance.SpendGold(giaTien);
        }
        else if (loaiTien == LoaiTien.KimCuong)
        {
            isSuccess = CurrencyManager.instance.SpendDiamonds(giaTien);
        }

        // Xử lý kết quả
        if (isSuccess)
        {
            Debug.Log($"Thanh toán thành công {giaTien} {loaiTien}!");
            OnGiaoDichThanhCong.Invoke();
        }
        else
        {
            Debug.Log($"Không đủ {loaiTien} để thanh toán!");
            OnGiaoDichThatBai.Invoke();
        }
    }
}