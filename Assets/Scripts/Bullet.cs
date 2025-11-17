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
        if (!other.CompareTag("Enemy"))
            return;

        Enemy frog = other.GetComponent<Enemy>();
        if (frog != null)
        {
            frog.TakeDamage(damage);
        }

        WalkingEnemy walking = other.GetComponent<WalkingEnemy>();
        if (walking != null)
        {
            walking.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
