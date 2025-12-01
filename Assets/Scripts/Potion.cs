using UnityEngine;

public class HealPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;          // ฮีลกี่หน่วย
    [SerializeField] private GameObject pickupEffect;     // เอฟเฟกต์ตอนเก็บ (ถ้าไม่มีก็ปล่อยว่างได้)

    // ใช้ Collider2D ที่ติ๊ก IsTrigger เอาไว้
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // ฮีลเลือด
            player.Heal(healAmount);

            // สร้างเอฟเฟกต์ (ถ้ามี)
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // ลบโพชั่นออกจากฉาก
            Destroy(gameObject);
        }
    }
}
