using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] List<GameObject> levelGameObjectList;

    LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject level in levelGameObjectList)
            level.SetActive(false);

        levelGameObjectList[GlobalVariables.currentLevel].SetActive(true);
    }

    public void AddToTotalCoins(int amount)
    {
        GlobalVariables.totalCoins += amount;
    }

    public void LoadNextLevel()
    {
        GlobalVariables.currentLevel++;

        if (GlobalVariables.currentLevel >= levelGameObjectList.Count)
            levelManager.LoadWinScene();
        else
            levelManager.ReloadScene();
    }
}
