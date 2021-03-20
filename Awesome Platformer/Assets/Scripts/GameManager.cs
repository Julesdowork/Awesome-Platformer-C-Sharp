using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] List<GameObject> levelGameObjectList;

    LevelManager levelManager;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
            
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
