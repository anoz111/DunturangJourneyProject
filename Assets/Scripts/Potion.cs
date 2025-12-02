using UnityEngine;

public class HealPotion : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;          
    [SerializeField] private GameObject pickupEffect;     


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Player player = other.GetComponent<Player>();
        if (player != null)
        {

            player.Heal(healAmount);

            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
