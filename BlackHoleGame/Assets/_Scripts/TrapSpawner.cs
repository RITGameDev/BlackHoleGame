using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will just create the specfied
/// number of objects ON AWAKE
/// </summary>
public class TrapSpawner : MonoBehaviour {

    public GameObject asteroidObject;
    public int amount;

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
