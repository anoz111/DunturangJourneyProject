using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] int amount = 1;               
    [SerializeField] GameObject pickupEffect;        
    [SerializeField] AudioClip pickupSfx;            
    [SerializeField] float destroyDelay = 0f;        

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance != null)
            GameManager.Instance.AddOrbs(amount);

        if (pickupEffect) Instantiate(pickupEffect, transform.position, Quaternion.identity);
        if (pickupSfx)    AudioSource.PlayClipAtPoint(pickupSfx, transform.position);

        if (destroyDelay <= 0f) Destroy(gameObject);
        else Destroy(gameObject, destroyDelay);
    }
}
