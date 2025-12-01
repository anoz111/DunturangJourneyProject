using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkingEnemy : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float arriveThreshold = 0.1f;
    [SerializeField] float waitAtEnd = 0f;

    [Header("Visual Flip")]
    [SerializeField] Transform gfx;               // ลากลูกที่มี SpriteRenderer/Animator (เช่น GameObject ชื่อ GFX)
    [SerializeField] bool facesRightByDefault = true; // true = ไฟล์สปริตหันขวาเป็นพื้นฐาน, false = หันซ้าย

    [Header("HP")]
    [SerializeField] int maxHP = 1;
    [SerializeField] int expReward = 1;

    Rigidbody2D rb;
    int currentHP;
    bool movingToB = true;
    float waitTimer = 0f;

    // จดตำแหน่งโลกของทางจุด A/B เพื่อล็อก ไม่ว่ามันจะเป็นลูกของใคร
    Vector2 posA, posB;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!gfx)  // เผื่อไม่ลากใน Inspector
        {
            // หา SpriteRenderer ลูกตัวแรกมาเป็น gfx อัตโนมัติ
            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr) gfx = sr.transform;
        }
    }

    void Start()
    {
        currentHP = maxHP;

        if (!pointA || !pointB)
        {
            Debug.LogWarning($"{name}: pointA/pointB ยังไม่ได้เซ็ต");
            enabled = false;
            return;
        }

        posA = pointA.position;
        posB = pointB.position;

        // เริ่มให้วิ่งไปหาจุดที่ไกลกว่า (เพื่อให้ขยับทันที)
        float dA = Vector2.Distance(transform.position, posA);
        float dB = Vector2.Distance(transform.position, posB);
        movingToB = dB >= dA;
    }

    void FixedUpdate()
    {
        Patrol();
    }

    void Patrol()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        Vector2 target = movingToB ? posB : posA;
        float dx = target.x - transform.position.x;

        // ถึงปลายทาง?
        if (Mathf.Abs(dx) <= arriveThreshold)
        {
            movingToB = !movingToB;
            if (waitAtEnd > 0f) waitTimer = waitAtEnd;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // วิ่งตามแกน X เท่านั้น
        float dirX = Mathf.Sign(dx); // -1 ซ้าย / +1 ขวา
        rb.linearVelocity = new Vector2(dirX * moveSpeed, rb.linearVelocity.y);

        // พลิกเฉพาะชั้นกราฟิก
        if (gfx)
        {
            // ถ้าสปริตต้นฉบับ "หันขวา" ใช้ค่าตรงไปตรงมา
            // ถ้าสปริตหันซ้ายโดยกำเนิด ให้คูณ -1
            int faceMul = facesRightByDefault ? 1 : -1;
            var ls = gfx.localScale;
            ls.x = Mathf.Abs(ls.x) * (dirX * faceMul > 0 ? 1f : -1f);
            gfx.localScale = ls;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Die();
    }

    void Die()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.AddExp(expReward);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (pointA) Gizmos.DrawWireSphere(pointA.position, 0.08f);
        if (pointB) Gizmos.DrawWireSphere(pointB.position, 0.08f);
        if (pointA && pointB) Gizmos.DrawLine(pointA.position, pointB.position);
    }
}
