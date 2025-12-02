using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainStage");
    }
    public void ShowCredit()
    {
        SceneManager.LoadScene("Credit");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
            GameManager.Instance.RestoreRunSnapshot();

        string lastStage = PlayerPrefs.GetString("LAST_STAGE", "MainStage");
        SceneManager.LoadScene(lastStage);
    }

}
