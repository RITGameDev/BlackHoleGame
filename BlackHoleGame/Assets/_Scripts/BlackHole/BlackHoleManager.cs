using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will hole all of the active black hole 
/// objects in the scene, so that the player movement
/// can be calculated from all of them. This will also
/// allow access to all the players to the black holes
/// to shoot them
/// Author: Ben Hoffman
/// </summary>
public class BlackHoleManager : MonoBehaviour {

    // A public static reference to this object
    public static BlackHoleManager currentBlackHoles;

    // A list of all black holes in the scene
    private List<BlackHole> blackHoles;     // List of currently active black holes
    private ObjectPooler blackHolesPool;    // The object pool of black holes

    public List<BlackHole> BlackHoles { get { return blackHoles;  } set { blackHoles = value; } }
    public ObjectPooler BlackHolesObjectPool { get { return blackHolesPool; } }

    /// <summary>
    /// Set up the current
    /// </summary>
    private void Awake()
    {
        // Make sure that we are only have one of these in our scene
        if (currentBlackHoles == null)
        {
            // Set the currenent referece
            currentBlackHoles = this;
        }
        else if (currentBlackHoles != this)
            Destroy(gameObject);

        // Initalize the linked list
        blackHoles = new List<BlackHole>();

        // Get the black hole object pooler
        blackHolesPool = GetComponent<ObjectPooler>();
    }

}
