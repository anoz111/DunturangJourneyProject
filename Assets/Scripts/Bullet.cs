using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ชนอะไรที่ Tag = Enemy
        if (!other.CompareTag("Enemy"))
            return;

        // 1) กบ (Enemy.cs ตัวเดิม)
        Enemy frog = other.GetComponent<Enemy>();
        if (frog != null)
        {
            frog.TakeDamage(damage);
        }

        // 2) ศัตรูเดิน (WalkingEnemy.cs)
        WalkingEnemy walking = other.GetComponent<WalkingEnemy>();
        if (walking != null)
        {
            walking.TakeDamage(damage);
        }

        // ทำลายกระสุน
        Destroy(gameObject);
    }
}
