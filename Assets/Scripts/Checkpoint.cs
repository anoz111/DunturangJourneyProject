using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint activated at: " + transform.position);
            }
        }
    }
}
