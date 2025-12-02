using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneUI : MonoBehaviour
{
    public void GoToMainMenu()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ClearRunSnapshot();

        SceneManager.LoadScene("MainMenu"); 
    }
}
