using UnityEngine;

// ⭐️ ไฟล์: Coin.cs
// ⭐️ ลากสคริปต์นี้ไปใส่ "Prefab" ของเหรียญ
public class Coin : MonoBehaviour
{
    [SerializeField] int coinValue = 1;
    [SerializeField] private GameObject pickupEffect; // (ทางเลือก) เอฟเฟกต์ตอนเก็บ

    // เราจะใช้ Is Trigger บน Collider 2D ของเหรียญ
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. เช็คว่าชน "Player" หรือไม่
        if (!other.CompareTag("Player"))
            return;

        // 2. ดึงสคริปต์ Player
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // ให้ Player เป็นคนจัดการเพิ่มเหรียญ + อัปเดต UI + Sync ข้ามซีน
            player.AddScore(coinValue);
        }

        // (ทางเลือก) แสดงเอฟเฟกต์
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        // 4. ทำลายเหรียญทิ้ง
        Destroy(gameObject);
    }
}
