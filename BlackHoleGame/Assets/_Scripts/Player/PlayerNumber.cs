using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumber : MonoBehaviour {

    [SerializeField]
    private int playerNumber = 1;
    public Color playerColor;

    public int _PlayerNumber { get { return playerNumber; } }
    public Color _PlayerColor { get { return playerColor; } }
    
}
