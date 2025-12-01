using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] int value = 1;
    [SerializeField] GameObject pickupEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance != null)
            GameManager.Instance.AddGems(value);

        if (pickupEffect) Instantiate(pickupEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
