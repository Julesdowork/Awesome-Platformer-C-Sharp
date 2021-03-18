using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    public event Action<int> OnPlayerGrabCoin;

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
    }
}
