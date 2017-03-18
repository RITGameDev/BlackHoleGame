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
    private float animTime = 0.5f;
    private WaitForSeconds spawnWait;
    public Transform deathSpot;
    public Transform[] spawnPoints;
    private Animator anim;
    [SerializeField]
    private float blackHoleLifetime = 1f;
    private float timeInBlackHole = 0f;

    /// <summary>
    /// Create a new wait for second object so that we have time between death and spawns
    /// </summary>
    private void Start()
    {
        // Initalize the wait for seconds object
        spawnWait = new WaitForSeconds(spawnTime);
        // Get the animator component
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Detects if we are in a trap or not
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // If we are colliding with a trap object...
        if (other.CompareTag("Trap"))
        {
            // We want to splat the player on the ground and respawn them
            StartCoroutine(Die());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("BlackHole"))
        {
            // Add to the amount of time that has been 
            timeInBlackHole += Time.deltaTime;

            // If we have exceeded our lifetime
            if(timeInBlackHole >= blackHoleLifetime)
            {
                // Die
                StartCoroutine(Die());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BlackHole"))
        {
            // Reset the black hole time after we leave it
            timeInBlackHole = 0f;
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

        // Play the animation for getting suck in
        anim.SetTrigger("Shrink");

        // Wait fro the animation to finish
        yield return new WaitForSeconds(animTime);

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

        anim.SetTrigger("Pop");
 
    }


}
