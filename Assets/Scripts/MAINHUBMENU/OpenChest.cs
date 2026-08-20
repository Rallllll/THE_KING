using UnityEngine;
using System.Collections;

public class OpenChest : MonoBehaviour
{
    [Header("1. Panel Đen (Bật NGAY khi bấm nút)")]
    public GameObject panelToShow;

    [Header("2. Panel Trắng / Nhận Thưởng (Bật SAU KHI mở xong)")]
    public GameObject rewardPanel;

    [Header("Animator GỐC")]
    public Animator chestAnimator;

    [Header("Cấu hình Thời gian")]
    public float introDuration = 1.0f;     // Thời gian chạy animation Intro
    public float autoOpenTime = 5.0f;      // Chờ tối đa 5s ở trạng thái Idle
    public float openAnimDuration = 1.2f;  // Thời gian chạy animation Open

    // Biến kiểm soát trạng thái
    private bool isIdleReady = false;
    private bool isOpened = false;
    private Coroutine autoOpenCoroutine;

    // ========================================================
    // LUỒNG 1: BẤM NÚT ĐỂ BẮT ĐẦU TOÀN BỘ SỰ KIỆN GACHA
    // ========================================================
    public void OnChestButtonClicked()
    {
        if (isIdleReady || isOpened) return;

        if (panelToShow != null) panelToShow.SetActive(true);

        // ÉP ANIMATOR CHẠY LẠI STATE INTRO TỪ ĐẦU (0f là frame 0)
        if (chestAnimator != null)
        {
            // Lưu ý: "ANIM_Chest_Energy_Intro" phải đúng y hệt tên cái cục màu cam trong Animator của ông
            chestAnimator.Play("ANIM_Chest_Energy_Intro", -1, 0f);
        }

        StartCoroutine(IntroSequenceRoutine());
    }

    private IEnumerator IntroSequenceRoutine()
    {
        // Mặc định lúc rương bật lên là nó tự chạy clip Intro (từ Entry -> Intro).
        // Ta chỉ việc ngồi chờ Intro chạy cho xong.
        yield return new WaitForSeconds(introDuration);

        // 3. Hết Intro -> Bắn trigger chuyển sang Idle
        if (chestAnimator != null) chestAnimator.SetTrigger("Intro");

        // Đánh dấu là đã rơi xong, bắt đầu lắng nghe chuột trái
        isIdleReady = true;

        // 4. Khởi động đồng hồ đếm ngược 5s
        autoOpenCoroutine = StartCoroutine(AutoOpenRoutine());
    }

    private IEnumerator AutoOpenRoutine()
    {
        // Cứ lặp lại đếm giờ cho đến khi hết 5s
        yield return new WaitForSeconds(autoOpenTime);

        // Nếu người dùng chưa mở thì tự ép chuyển sang Open
        if (!isOpened) ExecuteOpenSequence();
    }

    // ========================================================
    // LUỒNG 2: LẮNG NGHE CHUỘT TRÁI (MOUSE 0) ĐỂ SKIP CHỜ
    // ========================================================
    private void Update()
    {
        // Chỉ nhận chuột khi đang ở trạng thái Idle và chưa từng mở
        if (isIdleReady && !isOpened)
        {
            // Bắt sự kiện click chuột trái
            if (Input.GetMouseButtonDown(0))
            {
                // Người dùng đã bấm -> Hủy cái đếm ngược 5s tự mở đi
                if (autoOpenCoroutine != null) StopCoroutine(autoOpenCoroutine);

                ExecuteOpenSequence();
            }
        }
    }

    // ========================================================
    // LUỒNG 3: CHẠY ANIMATION MỞ RƯƠNG & HIỆN PHẦN THƯỞNG
    // ========================================================
    private void ExecuteOpenSequence()
    {
        isOpened = true; // Khóa lại, không cho nhận thêm click hay tự mở nữa

        // 5. Sang animation Open
        if (chestAnimator != null) chestAnimator.SetTrigger("Open");

        // Bắt đầu chờ nó mở xong
        StartCoroutine(WaitAndShowRewardPanel());
    }

    private IEnumerator WaitAndShowRewardPanel()
    {
        // 6. Chờ chạy hết thời lượng của clip Open
        yield return new WaitForSeconds(openAnimDuration);

        // 7. Hiện Panel nhận thưởng lên
        if (rewardPanel != null) rewardPanel.SetActive(true);
    }

    private void OnEnable()
    {
        // 1. Mở khóa các trạng thái về như lúc mới mua
        isIdleReady = false;
        isOpened = false;

        // 2. Dọn dẹp cái đồng hồ đếm giờ cũ (nếu có)
        if (autoOpenCoroutine != null)
        {
            StopCoroutine(autoOpenCoroutine);
            autoOpenCoroutine = null;
        }
    }
}