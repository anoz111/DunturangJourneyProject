using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMarker : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetString("LAST_STAGE", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save(); // กันลืม
        if (GameManager.Instance != null) GameManager.Instance.SaveRunSnapshot();
    }
}
