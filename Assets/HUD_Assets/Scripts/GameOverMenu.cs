using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;
    public GameObject mainChar;

    private void Update()
    {
        //Lấy vị trí của mainChar
        if (mainChar != null)
        {
            MainController mainController = mainChar.GetComponent<MainController>();

            // Kiểm tra nếu nhân vật rơi xuống quá thấp (ví dụ dưới -10)
            if (mainController.rb.position.y < -10f || mainController.HealthPoint <= 0)
            {
                GameOver();
            }
        }
        else
        {
            Debug.LogWarning("MainChar not assigned in GameOverMenu script!");
        }
    }
    public void GameOver()
    {
        gameOverMenuUI.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
