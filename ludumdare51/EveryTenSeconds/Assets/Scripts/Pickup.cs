using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    public UnityEvent onPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onPickup.Invoke();

            Destroy(gameObject);
        }

    }

    public void HACK_GiveCoin(int coinCount)
    {
        GameRunner.GetInstance().gameStateComponent.GetGameState().coin += coinCount;
    }
}
