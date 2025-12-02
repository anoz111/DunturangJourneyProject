using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeadArea : MonoBehaviour
{
    [SerializeField] int damage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        TryDamage(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        TryDamage(other.gameObject);
    }

    void TryDamage(GameObject go)
    {
        if (!go.CompareTag("Player")) return;

        var player = go.GetComponent<Player>() ?? go.GetComponentInParent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage); 
        }
    }
}
