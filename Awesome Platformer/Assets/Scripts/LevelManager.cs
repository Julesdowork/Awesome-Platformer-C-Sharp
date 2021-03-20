using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameScene()
    {
        GlobalVariables.currentLevel = 0;
        GlobalVariables.totalCoins = 0;
        SceneManager.LoadScene("Game");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadWinScene()
    {
        SceneManager.LoadScene("Win");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
