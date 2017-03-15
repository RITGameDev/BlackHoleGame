using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {

    public float force = 1f;


    void OnCollisionEnter2D(Collision2D col)
    { 
        // If we are colliding with a player, then push it away
        if (col.gameObject.CompareTag("Player"))
        {
            // Make the player attracted to the blackhole

        }
    }
}
