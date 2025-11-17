using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinValue = 1;
    [SerializeField] private GameObject pickupEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCoins(coinValue);
        }

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddScore(coinValue);
        }

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
