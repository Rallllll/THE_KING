using UnityEngine;
using System.Collections;

public class EliteEnemy : MonoBehaviour
{
    [Header("Chỉ số")]
    public int hp = 15;
    public float moveDownSpeed = 5f;
    public float moveXSpeed = 2f;
    public float stopY = 4f;
    public float moveRangeX = 2f;

    private Vector3 startPos;
    private int direction = 1;
    private bool isArrived = false;

    [Header("Vị trí nòng súng (Fire Positions)")]
    public Transform leftGunPos;        // Nòng trái (Dùng cho sấy đạn thẳng)
    public Transform rightGunPos;       // Nòng phải (Dùng cho sấy đạn thẳng)
    public Transform centerChargePos;   // Nòng giữa (Dùng riêng để tụ đạn to)

    [Header("Prefab Đạn")]
    public GameObject straightBulletPrefab; // Đạn bay thẳng
    public GameObject clusterBulletPrefab;  // Đạn to (AoE)

    [Header("Hiệu ứng & Hoạt ảnh")]
    public GameObject engineObject;
    public float explosionDuration = 0.5f;

    private bool isDead = false;
    private Animator anim;
    private Collider2D col;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isDead) return;

        // 1. Bay thẳng xuống đến mốc
        if (!isArrived)
        {
            transform.Translate(Vector3.down * moveDownSpeed * Time.deltaTime, Space.World);
            if (transform.position.y <= stopY)
            {
                isArrived = true;
                startPos = transform.position;

                StartCoroutine(AttackCombo());
            }
        }
        // 2. Lách qua lách lại
        else
        {
            transform.Translate(Vector3.right * direction * moveXSpeed * Time.deltaTime, Space.World);

            if (transform.position.x > startPos.x + moveRangeX) direction = -1;
            else if (transform.position.x < startPos.x - moveRangeX) direction = 1;
        }
    }

    IEnumerator AttackCombo()
    {
        while (!isDead)
        {
            // --- CHIÊU 1: SẤY 5 VIÊN (Dùng nòng Trái và Phải) ---
            for (int i = 0; i < 5; i++)
            {
                // Đẻ đạn ở nòng trái
                if (leftGunPos != null) Instantiate(straightBulletPrefab, leftGunPos.position, Quaternion.identity);
                // Đẻ đạn ở nòng phải
                if (rightGunPos != null) Instantiate(straightBulletPrefab, rightGunPos.position, Quaternion.identity);

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(1.5f);

            // --- CHIÊU 2: GỒNG ĐẠN TO (Dùng nòng Giữa) ---
            if (centerChargePos != null)
            {
                // Đẻ quả đạn to ra, gắn nó dính chặt vào nòng giữa
                GameObject chargeFX = Instantiate(clusterBulletPrefab, centerChargePos.position, Quaternion.identity, centerChargePos);

                ClusterBullet cb = chargeFX.GetComponent<ClusterBullet>();
                if (cb != null) cb.enabled = false; // Tạm tắt script bay để nó đứng im gồng

                float chargeTime = 1.5f;
                float timer = 0;

                Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);
                Vector3 endScale = new Vector3(1.5f, 1.5f, 1.5f);

                while (timer < chargeTime)
                {
                    if (isDead) { Destroy(chargeFX); yield break; }
                    timer += Time.deltaTime;
                    chargeFX.transform.localScale = Vector3.Lerp(startScale, endScale, timer / chargeTime);
                    yield return null;
                }

                // Gồng xong -> Nhả đạn ra khỏi tàu -> Bật script cho bay xuống
                chargeFX.transform.SetParent(null);
                if (cb != null) cb.enabled = true;
            }

            yield return new WaitForSeconds(2.5f);
        }
    }

    // ==========================================
    // PHẦN VA CHẠM VÀ CHẾT
    // ==========================================
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Player"))
        {
            Die();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        hp -= damageAmount;
        if (hp <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        if (col != null) col.enabled = false;

        // TẮT ĐỘNG CƠ NGAY LẬP TỨC 
        if (engineObject != null) engineObject.SetActive(false);

        if (anim != null) anim.SetTrigger("Die");
        Destroy(gameObject, explosionDuration);
    }
}