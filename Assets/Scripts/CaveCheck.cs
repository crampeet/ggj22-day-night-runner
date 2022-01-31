using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveCheck : MonoBehaviour
{
    public bool diesIfHot = true;
    public ChangeSun sun;
    public PlayerController player;

    // when the GameObjects collider arrange for this GameObject to travel to the left of the screen
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("TriggerEnter2D: " + col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        if (sun.isSunHot == diesIfHot)
        {
            var playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                if (player.audioSource && player.ouchAudio)
                    player.audioSource.PlayOneShot(player.ouchAudio);
                playerHealth.Decrement();
                Debug.Log("Player Health: " + playerHealth.GetHP());
            }
        }
    }
}
