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
        if (other.CompareTag("Player"))
        {
            // 2. ดึงสคริปต์ Player (ตามชื่อคลาสที่คุณใช้)
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // 3. เรียกฟังก์ชัน AddScore (ที่เราเตรียมไว้ใน Player.cs)
                player.AddScore(coinValue);

                // (ทางเลือก) แสดงเอฟเฟกต์
                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                // 4. ทำลายเหรียญทิ้ง
                Destroy(gameObject);
            }
        }
    }
}