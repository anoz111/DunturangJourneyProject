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

    [Header("HP")]
    [SerializeField] int maxHP = 1;
    [SerializeField] int expReward = 1;

    Rigidbody2D rb;
    int currentHP;
    bool movingToB = true;
    float waitTimer = 0f;

    Vector2 posA, posB;


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

        if (Mathf.Abs(dx) <= arriveThreshold)
        {
            movingToB = !movingToB;
            if (waitAtEnd > 0f) waitTimer = waitAtEnd;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
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
