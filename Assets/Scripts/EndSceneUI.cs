using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneUI : MonoBehaviour
{
    // ปุ่ม "Menu"
    public void GoToMainMenu()
    {
        // เผื่ออยากล้าง snapshot/run state ก่อนกลับเมนู
        if (GameManager.Instance != null)
            GameManager.Instance.ClearRunSnapshot();

        SceneManager.LoadScene("MainMenu"); // ชื่อซีนเมนูของคุณ
    }
}
