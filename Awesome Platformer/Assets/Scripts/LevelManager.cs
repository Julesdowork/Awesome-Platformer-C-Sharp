using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameScene()
    {
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
}
