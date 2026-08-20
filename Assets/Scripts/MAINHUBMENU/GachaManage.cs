using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GachaController : MonoBehaviour
{
    [Header("Giao diện UI")]
    [Tooltip("Kéo thả lần lượt 10 cái Image ông đã tạo sẵn vào đây")]
    public List<GameObject> itemSlots;

    [Header("Kho Đồ Ngẫu Nhiên")]
    [Tooltip("Kéo các hình ảnh vật phẩm (súng, tàu, vàng...) vào đây")]
    public List<Sprite> itemDatabase;

    [Header("Nút Exit")]
    [Tooltip("Kéo cái RewardGacha (Bảng phần thưởng) vào đây để tắt")]
    public GameObject rewardPanel;
    [Tooltip("Kéo cái OpenChestAnimationPanel (Cục cha chứa cả rương) vào đây để tắt luôn")]
    public GameObject mainGachaPanel;

    [Header("Reset Animator")]
    [Tooltip("Kéo Animator của rương vào đây để reset sau khi quay xong")]
    public Animator chestAnimator;

    // Ổ cắm cho Nút x1
    public void RollX1()
    {
        GenerateReward(1);
    }

    // Ổ cắm cho Nút x10
    public void RollX10()
    {
        GenerateReward(10);
    }

    // ===================================================
    // Ổ CẮM CHO NÚT EXIT (DẤU X) TRÊN BẢNG PHẦN THƯỞNG
    // ===================================================
    public void OnExitButtonClicked()
    {
        // 1. Ẩn toàn bộ vật phẩm đi cho sạch sẽ
        foreach (GameObject slot in itemSlots)
        {
            slot.SetActive(false);
        }

        // 2. Tắt bảng phần thưởng
        if (rewardPanel != null) rewardPanel.SetActive(false);

        // 3. QUAN TRỌNG NHẤT: Bắt thằng Animator quay ngược thời gian về lúc chưa mở
        if (chestAnimator != null)
        {
            chestAnimator.Rebind();
            chestAnimator.Update(0f);
        }

        // 4. Tắt luôn màn hình chứa Rương
        if (mainGachaPanel != null) mainGachaPanel.SetActive(false);
    }

    // Hàm Lõi: Xử lý hiển thị
    private void GenerateReward(int amount)
    {
        foreach (GameObject slot in itemSlots)
        {
            slot.SetActive(false);
        }

        for (int i = 0; i < amount; i++)
        {
            if (i >= itemSlots.Count) break;

            itemSlots[i].SetActive(true);
            int randomIndex = Random.Range(0, itemDatabase.Count);
            itemSlots[i].GetComponent<Image>().sprite = itemDatabase[randomIndex];
        }
    }
}