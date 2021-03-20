using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    [SerializeField] GameObject container;

    Player player;
    InputManager inputManager;
    GameManager gameManager;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        player.OnPlayerWin += DisplayWinUI;
        inputManager.OnJump += NextLevel;

    }

    private void OnDisable()
    {
        player.OnPlayerWin -= DisplayWinUI;
        inputManager.OnJump -= NextLevel;
    }

    private void DisplayWinUI()
    {
        container.SetActive(true);
    }

    private void NextLevel()
    {
        if (container.activeSelf)
            gameManager.LoadNextLevel();
    }
}
