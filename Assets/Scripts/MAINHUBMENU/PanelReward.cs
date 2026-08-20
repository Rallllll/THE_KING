using UnityEngine;
using System.Collections;

public class PanelReward : MonoBehaviour
{
    [Header("Kéo RewardGacha vào đây")]
    public GameObject rewardPanel;

    [Header("Thời gian chờ (giây)")]
    public float delayTime = 1f;

    // Hàm này tự động chạy ngay khoảnh khắc Object chứa nó được SetActive(true)
    private void OnEnable()
    {
        if (rewardPanel != null)
        {
            StartCoroutine(WaitAndShowRoutine());
        }
    }

    private IEnumerator WaitAndShowRoutine()
    {
        // Đợi 1 giây
        yield return new WaitForSeconds(delayTime);

        // Bật bảng phần thưởng
        if (rewardPanel != null)
        {
            rewardPanel.SetActive(true);
        }
    }
}