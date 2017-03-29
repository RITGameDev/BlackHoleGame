using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will control the player health/what happens
/// when they 
/// </summary>
[RequireComponent(typeof(PlayerNumber))]
public class PlayerHealth : MonoBehaviour {

    #region Fields
    public PlayerLives playerLives_UI; // The UI componenet of this object

    private bool isDead = false;    // If this is true then we are dead.

    [SerializeField]
    private  float spawnTime = 3f;  // The spawn time, default of 3f
    private float animTime = 0.5f;  // How long the death animation, default is 0.5f

    private WaitForSeconds spawnWait; // The wait object for how long we will wait after death 
    public Transform deathSpot;       // Where this object will be moved to when the 
    public Transform[] spawnPoints;   // Array of possible spawn points

    private Animator anim;              // The animator component, so that when we die/spawn we can play our animations
    private PlayerNumber _myPlayerNum;  // The player number component, so that we can tell the score keeper when we are kileld

    private PlayerMovement p_movement;  // The player movement component, so that we can stop moving when we die
        
    #endregion

    /// <summary>
    /// Allows other scripts to see if we are dead or not
    /// </summary>
    public bool IsDead { get { return isDead; } }

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

        // Get the player movement componenet so that we can stop when we die
        p_movement = GetComponent<PlayerMovement>();
    }

    /// <summary>
    /// Detects if we are in a trap or not
    /// </summary>
    /// <param name="other">The colliding object</param>
    void OnCollisionEnter2D (Collision2D other)
    {
        // If we are colliding with a trap object...
        if (other.gameObject.CompareTag("BlackHole"))
        {
            // We want to splat the player on the ground and respawn them
            StartCoroutine(Die());

            // Pop a life out of the animtaion
            playerLives_UI.RemoveLife();
        }
    }

    /// <summary>
    /// Author: Ben Hoffman
    /// Kill the player, and then respawn thems
    /// </summary>
    private IEnumerator Die()
    {
        // Mark ourselves as dead
        isDead = true;

        // Disableplayer movement
        p_movement.enabled = false;
    
        // Play the animation for getting suck in
        anim.SetTrigger("Shrink");

        // Tell the score keep that we died
        ScoreKeeper._current_score.AddScore(_myPlayerNum._PlayerNumber, 1);

        // Wait fro the animation to finish
        yield return new WaitForSeconds(animTime);

        // Move the player out of the play area
        transform.position = deathSpot.position;

        // Wait for the spawn time
        yield return spawnWait;

        // If the game is over, then don't spawn a player again

        if (GameManager.gameManager.CurrentGameState == GameState.GameOver) yield break;

        // If the game is pasued, then do nothing until it becomes unpaused
        while(GameManager.gameManager.CurrentGameState == GameState.Paused)
        {
            yield return null;
        }

        // Move the player back to the play area again
        transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

        // Set the animation trigger to pop the player back up
        anim.SetTrigger("Pop");

        // Enable player movement
        p_movement.enabled = true;

        // We are not dead anymore
        isDead = false;
    }
}
