using UnityEngine;

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
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }
    }

    void Update()
    {
        if (spriteRenderer == null || rb == null)
            return;

        float vx = rb.linearVelocity.x;

        if (Mathf.Abs(vx) < flipThreshold)
            return;

        bool movingRight = vx > 0f;

        if (defaultFacingRight)
        {
            spriteRenderer.flipX = !movingRight;
        }
        else
        {
            spriteRenderer.flipX = movingRight;
        }
    }
}
