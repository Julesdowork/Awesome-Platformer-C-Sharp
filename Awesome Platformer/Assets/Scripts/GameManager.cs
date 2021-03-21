using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] List<GameObject> levelGameObjectList;
    [SerializeField] List<Sprite> levelBackgroundList;
    [SerializeField] SpriteRenderer background;
    [SerializeField] List<GameObject> lights;

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

        foreach (GameObject light in lights)
            light.SetActive(false);

        levelGameObjectList[GlobalVariables.currentLevel].SetActive(true);
        lights[GlobalVariables.currentLevel].SetActive(true);
        // background.sprite = levelBackgroundList[GlobalVariables.currentLevel];
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
