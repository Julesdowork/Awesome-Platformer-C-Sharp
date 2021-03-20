using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameWinUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;

    // Start is called before the first frame update
    void Start()
    {
        coinText.text = GlobalVariables.totalCoins.ToString();
    }
}
