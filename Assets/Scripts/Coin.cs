using UnityEngine;

// ⭐️ ลากสคริปต์นี้ไปใส่ "Prefab" ของเหรียญ
public class Coin : MonoBehaviour
{
    [SerializeField] int coinValue = 1;
    [SerializeField] private GameObject pickupEffect; // เอฟเฟกต์ตอนเก็บ (ถ้ามี)

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // เพิ่มเหรียญเข้า GameManager (ใช้ข้ามซีน)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoins(coinValue);
        }

        // อัปเดต UI ฝั่ง Player (ให้มันไปอ่านค่าใน GameManager อีกที)
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddScore(coinValue); // ตอนนี้จะใช้เพื่ออัปเดต UI อย่างเดียว
        }

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
