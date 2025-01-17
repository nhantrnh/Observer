using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        DataPersistenceManager.instance.NewGame();
        // SceneManager.LoadScene(1); //Scene 1 map 0
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!!!");
        Application.Quit();
    }

    public void ContinueGame(){        
        DataPersistenceManager.instance.LoadGame();
    }
}
