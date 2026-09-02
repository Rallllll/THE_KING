using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("--- KHO QUÁI VẬT ---")]
    [Tooltip("2 con quái chỉ rớt lẻ tẻ")]
    public GameObject[] scatteredEnemies;

    [Tooltip("2 con quái chỉ bay theo đội hình")]
    public GameObject[] formationEnemies;

    [Tooltip("2 con Mini-Boss cho đợt 1 và đợt 2")]
    public GameObject[] miniBosses;

    [Tooltip("Boss cuối cùng cho đợt 3")]
    public GameObject finalBoss;

    [Header("--- ĐIỂM SPAWN (TỌA ĐỘ) ---")]
    [Tooltip("4 điểm cho quái rớt lẻ tẻ")]
    public Transform[] scatteredSpawnPoints;

    [Tooltip("7 điểm cho quái đội hình (Xếp từ Trái -> Phải)")]
    public Transform[] formationSpawnPoints;

    [Tooltip("1 điểm ở giữa trên cùng cho Boss xuất hiện")]
    public Transform bossSpawnPoint;

    [Header("--- CẤU HÌNH NHỊP ĐỘ ---")]
    public float waveDuration = 20f;     // Mỗi đợt farm quái kéo dài bao lâu (giây)
    public float delayBetweenWaves = 3f; // Nghỉ ngơi trước khi sang đợt mới
    public float scatteredRate = 1.5f;   // Tốc độ rớt quái lẻ

    // Biến để quản lý luồng, giúp tắt đẻ quái khi Boss ra
    private Coroutine scatteredRoutine;
    private Coroutine formationRoutine;

    private void Start()
    {
        StartCoroutine(CampaignLoop());
    }

    // ==========================================
    // CHIẾN DỊCH CHÍNH (3 ĐỢT)
    // ==========================================
    private IEnumerator CampaignLoop()
    {
        yield return new WaitForSeconds(2f);

        for (int wave = 1; wave <= 3; wave++)
        {
            Debug.Log($"=== BẮT ĐẦU ĐỢT {wave} ===");

            // 1. Bật máy đẻ quái nhỏ
            scatteredRoutine = StartCoroutine(SpawnScatteredRoutine());
            formationRoutine = StartCoroutine(SpawnRandomFormationRoutine());

            // 2. Chạy thanh tiến trình chờ Boss
            float timer = 0f;
            float segmentSize = 1f / 3f; // Khúc 1 là 0-0.33, khúc 2 là 0.33-0.66...
            float startFill = (wave - 1) * segmentSize;

            while (timer < waveDuration)
            {
                timer += Time.deltaTime;
                float currentFill = startFill + (timer / waveDuration) * segmentSize;

                if (WaveProgressBar.instance != null)
                {
                    WaveProgressBar.instance.UpdateProgress(currentFill);
                }

                yield return null;
            }

            // 3. Báo động Boss xuất hiện
            if (WaveProgressBar.instance != null)
                WaveProgressBar.instance.HighlightBossNode(wave - 1);

            yield return new WaitForSeconds(2f);

            // 4. Sinh ra Boss tương ứng với từng đợt
            GameObject activeBoss = null;
            if (wave == 1)
                activeBoss = Instantiate(miniBosses[0], bossSpawnPoint.position, miniBosses[0].transform.rotation);
            else if (wave == 2)
                activeBoss = Instantiate(miniBosses[1], bossSpawnPoint.position, miniBosses[1].transform.rotation);
            else if (wave == 3)
                activeBoss = Instantiate(finalBoss, bossSpawnPoint.position, finalBoss.transform.rotation);

            // 5. Chờ đến khi Boss chết (Máy đẻ quái nhỏ vẫn đang chạy ngầm)
            while (activeBoss != null)
            {
                yield return null;
            }

            Debug.Log($"=== ĐÃ TIÊU DIỆT BOSS ĐỢT {wave} ===");

            // 6. TẮT MÁY ĐẺ QUÁI (Đưa xuống sau khi Boss chết)
            StopCoroutine(scatteredRoutine);
            StopCoroutine(formationRoutine);

            if (WaveProgressBar.instance != null)
                WaveProgressBar.instance.ClearBossNode(wave - 1);

            // 7. Nghỉ vài giây trước khi sang đợt tiếp theo
            if (wave < 3) yield return new WaitForSeconds(delayBetweenWaves);
        }

        Debug.Log("CHÚC MỪNG! PHÁ ĐẢO GAME!");
    }

    // ==========================================
    // LUỒNG 1: RỚT LẺ TẺ (4 Điểm, 2 Loại quái)
    // ==========================================
    private IEnumerator SpawnScatteredRoutine()
    {
        while (true)
        {
            // Bốc 1 trong 4 điểm rải rác
            Transform randomPoint = scatteredSpawnPoints[Random.Range(0, scatteredSpawnPoints.Length)];

            // Bốc 1 trong 2 loại quái lẻ
            GameObject randomEnemy = scatteredEnemies[Random.Range(0, scatteredEnemies.Length)];

            Instantiate(randomEnemy, randomPoint.position, randomEnemy.transform.rotation);

            yield return new WaitForSeconds(scatteredRate);
        }
    }

    // ==========================================
    // LUỒNG 2: RỚT BẦY ĐÀN (7 Điểm, 2 Loại quái)
    // ==========================================
    private IEnumerator SpawnRandomFormationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // Cứ 3s thả 1 bầy

            int formationType = Random.Range(0, 4);
            float pace = 0.25f;

            // Bốc 1 trong 2 loại quái đội hình làm Đội trưởng
            GameObject squadPrefab = formationEnemies[Random.Range(0, formationEnemies.Length)];
            Quaternion spawnRot = squadPrefab.transform.rotation;

            // Đội hình 7 điểm (Từ 0 đến 6, Chính giữa là 3)
            switch (formationType)
            {
                case 0: // BỨC TƯỜNG (Ra 7 con ngang)
                    for (int i = 0; i < 7; i++) Instantiate(squadPrefab, formationSpawnPoints[i].position, spawnRot);
                    break;

                case 1: // MŨI KHOAN CHỮ V (Mở rộng 7 điểm)
                    Instantiate(squadPrefab, formationSpawnPoints[3].position, spawnRot); // Giữa
                    yield return new WaitForSeconds(pace);
                    Instantiate(squadPrefab, formationSpawnPoints[2].position, spawnRot);
                    Instantiate(squadPrefab, formationSpawnPoints[4].position, spawnRot);
                    yield return new WaitForSeconds(pace);
                    Instantiate(squadPrefab, formationSpawnPoints[1].position, spawnRot);
                    Instantiate(squadPrefab, formationSpawnPoints[5].position, spawnRot);
                    yield return new WaitForSeconds(pace);
                    Instantiate(squadPrefab, formationSpawnPoints[0].position, spawnRot);
                    Instantiate(squadPrefab, formationSpawnPoints[6].position, spawnRot);
                    break;

                case 2: // GỌNG KÌM (Ép từ ngoài vào trong 7 điểm)
                    Instantiate(squadPrefab, formationSpawnPoints[0].position, spawnRot);
                    Instantiate(squadPrefab, formationSpawnPoints[6].position, spawnRot);
                    yield return new WaitForSeconds(pace);
                    Instantiate(squadPrefab, formationSpawnPoints[1].position, spawnRot);
                    Instantiate(squadPrefab, formationSpawnPoints[5].position, spawnRot);
                    yield return new WaitForSeconds(pace);
                    Instantiate(squadPrefab, formationSpawnPoints[2].position, spawnRot);
                    Instantiate(squadPrefab, formationSpawnPoints[4].position, spawnRot);
                    yield return new WaitForSeconds(pace);
                    Instantiate(squadPrefab, formationSpawnPoints[3].position, spawnRot); // Giữa chốt đuôi
                    break;

                case 3: // ĐƯỜNG CHÉO (Ziczac lướt 7 con)
                    for (int i = 0; i < 7; i++)
                    {
                        Instantiate(squadPrefab, formationSpawnPoints[i].position, spawnRot);
                        yield return new WaitForSeconds(pace);
                    }
                    break;
            }
        }
    }
}