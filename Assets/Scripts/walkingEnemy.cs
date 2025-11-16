using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [Header("HP Settings")]
    [SerializeField] int maxHP = 1;
    int currentHP;

    [Header("Reward")]
    [SerializeField] int expReward = 1;   // EXP ที่ให้ผู้เล่นตอนตาย

    bool movingToB = true;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (pointA == null || pointB == null) return;

        Transform target = movingToB ? pointB : pointA;

        // เดินไปทางเป้าหมาย
        Vector2 dir = (target.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

        // พลิกตัวให้หันไปทางเดิน
        if (dir.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // เช็คว่าถึงจุด A/B ยัง
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance < 0.1f)
        {
            movingToB = !movingToB;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // เพิ่มเลเวลให้ GameManager โดยตรง
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddLevel(1);
        }

        Destroy(gameObject);
    }

}
