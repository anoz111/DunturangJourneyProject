using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopPortal : MonoBehaviour
{
    [SerializeField] private string shopSceneName = "ShopScene"; // ตั้งชื่อให้ตรงกับชื่อ Scene ร้าน

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // เปลี่ยนไป Scene ร้านค้า
            SceneManager.LoadScene(shopSceneName);
        }
    }
}
