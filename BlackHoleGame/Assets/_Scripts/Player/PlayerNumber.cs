using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be 
/// </summary>
public class PlayerNumber : MonoBehaviour {

    [SerializeField]
    private int playerNumber = 1;
    public Color playerColor;

    public int _PlayerNumber { get { return playerNumber; } }
    public Color _PlayerColor { get { return playerColor; } }

    /// <summary>
    /// Set the color of all the sprites to our player color
    /// </summary>
    void Start()
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
