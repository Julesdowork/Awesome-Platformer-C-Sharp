using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadUI : MonoBehaviour
{
    [SerializeField] GameObject container;

    Player player;
    InputManager inputManager;
    LevelManager levelManager;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        inputManager = FindObjectOfType<InputManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnEnable()
    {
        player.OnPlayerDie += DisplayDeadUI;
        inputManager.OnJump += RestartScene;

    }

    private void OnDisable()
    {
        player.OnPlayerDie -= DisplayDeadUI;
        inputManager.OnJump -= RestartScene;
    }

    private void DisplayDeadUI()
    {
        container.SetActive(true);
    }

    private void RestartScene()
    {
        if (container.activeSelf)
            levelManager.ReloadScene();
    }
}
