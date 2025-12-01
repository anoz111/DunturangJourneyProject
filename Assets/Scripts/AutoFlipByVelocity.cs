using UnityEngine;

/// <summary>
/// สคริปต์สำหรับพลิกตัวละครซ้าย/ขวา อัตโนมัติจากความเร็วแกน X
/// แปะตัวนี้ตัวเดียว ใช้ได้กับทุกตัวที่มี Rigidbody2D + SpriteRenderer
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class AutoFlipByVelocity : MonoBehaviour
{
    [Header("ตัว Sprite ที่จะพลิก (ถ้าไม่ใส่จะหาในตัวเองหรือเด็กคนแรก)")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("หันขวาเป็นทิศเริ่มต้นหรือเปล่า?")]
    [SerializeField] private bool defaultFacingRight = true;

    [Header("ความเร็วขั้นต่ำที่ถือว่ากำลังหัน (กันกรณีเลื่อนเบา ๆ แล้วกระตุก)")]
    [SerializeField] private float flipThreshold = 0.01f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
        {
            // ลองหาในตัวเองก่อน
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                // ถ้าไม่มี ลองหาในลูก (เช่น ตัวละครมี child ถือ sprite)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }
    }

    void Update()
    {
        if (spriteRenderer == null || rb == null)
            return;

        // ใช้ linearVelocity.x (Unity 2022+) ถ้าของหมู่เฮาเป็น .velocity ก็เปลี่ยนตามเวอร์ชั่น
        float vx = rb.linearVelocity.x;

        // ถ้าเร็วไม่ถึง threshold ให้ถือว่าไม่ได้หันไปทิศใดทิศหนึ่งชัดเจน → ไม่ต้องพลิก
        if (Mathf.Abs(vx) < flipThreshold)
            return;

        bool movingRight = vx > 0f;

        // ถ้าสปไรต์ปกติหันขวา → เวลาเดินขวาไม่ flip / เดินซ้ายให้ flipX = true
        if (defaultFacingRight)
        {
            spriteRenderer.flipX = !movingRight;
        }
        else
        {
            // ถ้าสปไรต์ปกติหันซ้าย → กลับตรรกะ
            spriteRenderer.flipX = movingRight;
        }
    }
}
