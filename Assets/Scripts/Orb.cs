using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] int amount = 1;                 // เก็บได้กี่ลูกต่อครั้ง (ส่วนใหญ่ 1)
    [SerializeField] GameObject pickupEffect;        // (ทางเลือก) เอฟเฟกต์ตอนเก็บ
    [SerializeField] AudioClip pickupSfx;            // (ทางเลือก) เสียงตอนเก็บ
    [SerializeField] float destroyDelay = 0f;        // ดีเลย์ก่อนลบวัตถุ (0 = ทันที)

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance != null)
            GameManager.Instance.AddOrbs(amount);

        if (pickupEffect) Instantiate(pickupEffect, transform.position, Quaternion.identity);
        if (pickupSfx)    AudioSource.PlayClipAtPoint(pickupSfx, transform.position);

        if (destroyDelay <= 0f) Destroy(gameObject);
        else Destroy(gameObject, destroyDelay);
    }
}
