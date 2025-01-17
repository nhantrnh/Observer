using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;

    public GameObject PauseMenuUI;
    public GameObject OptionMenuUI;

    public void OnPausePressed()
    {
        Debug.Log("Pause!");

        if (GameIsPause)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    public void OptionsMenu()
    {
        OptionMenuUI.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameIsPause = false;
        SceneManager.LoadScene("MainMenu");
    }
}
