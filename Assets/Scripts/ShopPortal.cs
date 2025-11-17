using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopPortal : MonoBehaviour
{
    [SerializeField] private string shopSceneName = "ShopScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(shopSceneName);
        }
    }
}
