using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;

    Player player;

    void Awake() 
    {
        player = FindObjectOfType<Player>();
    }

    void OnEnable()
    {
        player.OnPlayerGrabCoin += UpdateText;
    }

    void OnDisable()
    {
        player.OnPlayerGrabCoin -= UpdateText;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        coinText.text = player.coinAmount.ToString();
    }
}
