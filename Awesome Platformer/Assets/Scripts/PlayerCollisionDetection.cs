using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    public event Action<int> OnPlayerGrabCoin;
    public event Action OnPlayerWin;

    [SerializeField] GameObject winEffect;

    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            Destroy(other.gameObject);
            player.AddCoin(1);
            OnPlayerGrabCoin?.Invoke(player.coinAmount);
            // Play coin sound
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Star"))
        {
            Destroy(other.gameObject);
            player.gameObject.SetActive(false);
            GameManager.instance.AddToTotalCoins(player.coinAmount);
            OnPlayerWin?.Invoke();
            Instantiate(winEffect, player.transform.position, Quaternion.identity);
            // Play star sound
        }
    }
}
