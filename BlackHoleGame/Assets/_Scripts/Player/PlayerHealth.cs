using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will control the player health/what happens
/// when they 
/// </summary>
public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    private  float spawnTime = 3f;
    private WaitForSeconds spawnWait;
    public Transform deathSpot;
    public Transform[] spawnPoints;

    /// <summary>
    /// Create a new wait for second object so that we have time between death and spawns
    /// </summary>
    private void Start()
    {
        // Initalize the wait for seconds object
        spawnWait = new WaitForSeconds(spawnTime);   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If we are colliding with a trap object...
        if (other.CompareTag("Trap"))
        {
            // We want to splat the player on the ground and respawn them
            StartCoroutine(Die());
        }
    }

    /// <summary>
    /// Author: Ben Hoffman
    /// Kill the player, and then respawn thems
    /// </summary>
    private IEnumerator Die()
    {
        // Move the player off screen, and disable player movement
        PlayerMovement pMovement = GetComponent<PlayerMovement>();
        PlayerFireController pFire = GetComponent<PlayerFireController>();

        
        // Disable player movement
        pMovement.enabled = false;
        // Disable player firing
        pFire.enabled = false;

        // Move the player out of the play area
        pMovement.gameObject.transform.position = deathSpot.position;

        // Wait for the spawn time
        yield return spawnWait;

        // Move the player back to the play area again
        pMovement.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        // Enable player movement again
        pMovement.enabled = true;

        // Enable player shooting again
        pFire.enabled = true;
    }


}
