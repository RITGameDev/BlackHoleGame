using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will control the player health/what happens
/// when they 
/// </summary>
public class PlayerHealth : MonoBehaviour {

    #region Fields

    [SerializeField]
    private  float spawnTime = 3f;  // The spawn time, default of 3f
    private float animTime = 0.5f;  // How long the death animation, fefault is 0.5f

    private WaitForSeconds spawnWait;
    public Transform deathSpot;
    public Transform[] spawnPoints;
    private Animator anim;
    private PlayerNumber _myPlayerNum;

    #endregion

    /// <summary>
    /// Create a new wait for second object so that we have time between death and spawns
    /// </summary>
    private void Start()
    {
        // Initalize the wait for seconds object
        spawnWait = new WaitForSeconds(spawnTime);
        // Get the animator component
        anim = GetComponent<Animator>();
        // Get the player number component
        _myPlayerNum = GetComponent<PlayerNumber>();
    }

    /// <summary>
    /// Detects if we are in a trap or not
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // If we are colliding with a trap object...
        if (other.CompareTag("Trap") || other.CompareTag("BlackHole"))
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

        // Play the animation for getting suck in
        anim.SetTrigger("Shrink");

        // Tell the score keep that we died
        ScoreKeeper._current_score.AddScore(_myPlayerNum._PlayerNumber, 1);

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

        // Set the animation trigger to pop the player back up
        anim.SetTrigger("Pop");
    }
}
