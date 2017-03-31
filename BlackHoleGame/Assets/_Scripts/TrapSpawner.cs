using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will just create the specfied
/// number of objects ON AWAKE
/// Author: Ben Hoffman
/// </summary>
public class TrapSpawner : MonoBehaviour {

    public GameObject asteroidObject;   // The trap "asteroid" prefab
    public int amount;      // How many of these that we want to spawn

    /// <summary>
    /// Iterate through the number of objects that we want to create,
    /// and instantiate that number of object sin the game world
    /// </summary>
    void Awake()
    {
        // Loop through the specified number of times and create that many prefab instances
        for (int i = 0; i < amount; i++)
        {
            // Instantiate a prefab instance
            Instantiate(asteroidObject);
        }
    }

}
