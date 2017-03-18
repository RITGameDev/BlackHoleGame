using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simply set the color of all the sprite renderers on this
/// object to the one color specified by the player number
/// script
/// </summary>
[RequireComponent(typeof(PlayerNumber))]
public class PlayerColorSet : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // Get all the sprite renderers
        SpriteRenderer[] spRend = GetComponentsInChildren<SpriteRenderer>();
        // The the color componenet that we want
        Color myCol = GetComponent<PlayerNumber>()._PlayerColor; 

        // Loop through all sprite renderers and set their color to this
        for (int i = 0; i < spRend.Length; i++)
        {
            spRend[i].color = myCol;
        }
    }
	
	
}
