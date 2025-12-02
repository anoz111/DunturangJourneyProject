using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class WalkingEnemy : MonoBehaviour
{
    [Header("Move A <-> B")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float arriveThreshold = 0.1f;
    [SerializeField] float waitAtEnd = 0f;

    [Header("HP / Reward")]
    [SerializeField] int maxHP = 1;
    [SerializeField] int expReward = 1;

    Rigidbody2D rb;
    int currentHP;
    bool movingToB = true;
    float waitTimer = 0f;

    Vector2 posA, posB;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        currentHP = maxHP;

        if (!pointA || !pointB)
        {
            Debug.LogWarning($"{name}: pointA/pointB ยังไม่ได้เซ็ต");
            enabled = false; return;
        }

        posA = pointA.position;
        posB = pointB.position;

        float dA = Mathf.Abs(transform.position.x - posA.x);
        float dB = Mathf.Abs(transform.position.x - posB.x);
        movingToB = dB >= dA;
    }

    void FixedUpdate()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float targetX = movingToB ? posB.x : posA.x;

        float step = moveSpeed * Time.fixedDeltaTime;
        float prevX = rb.position.x;
        float newX = Mathf.MoveTowards(prevX, targetX, step);

        if (Mathf.Abs(newX - targetX) <= arriveThreshold)
        {
            newX = targetX;
            movingToB = !movingToB;
            if (waitAtEnd > 0f) waitTimer = waitAtEnd;
        }

        Vector2 newPos = new Vector2(newX, rb.position.y);
        rb.MovePosition(newPos);

        float vx = (newX - prevX) / Time.fixedDeltaTime;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.CompareTag("Player")) return;
        var player = other.collider.GetComponent<Player>() ?? other.collider.GetComponentInParent<Player>();
        if (player != null) player.TakeDamage(1);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Die();
    }

    void Die()
    {
        if (GameManager.Instance != null) GameManager.Instance.AddExp(expReward);
        Destroy(gameObject);
    }
}
