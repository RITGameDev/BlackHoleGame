using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Author: Ben Hoffman
/// This method will allow the player to fire a pooled object when 
/// they give fire input
/// </summary>
public class PlayerFireController : MonoBehaviour {

    public Transform bulletSpawn;   // The location that we want to spawn our bullet
    [SerializeField]
    private float fireRate = .5f;   // How much time is in between each shot at a minimum

    private ObjectPooler objPool;   // Our object pooler
    private float timeSinceLastFire = 0f;   // How long has it been since we have shot?

    private string fireInput = "Fire";      // The input string for this 

    private PlayerHealth ourHealth;     // Our health component, so that when we are dead we don't do anything

	/// <summary>
    /// Get the reference to the object poole, set up the input string, and get the health componenet
    /// </summary>
	void Start ()
    {
        // Get the reference to the object pooler
        objPool = BlackHoleManager.currentBlackHoles.BlackHolesObjectPool;

        // Set up the player number input
        int playerNum = GetComponentInParent<PlayerNumber>()._PlayerNumber;

        // Set up the input string byu adding the player number to it
        fireInput += playerNum.ToString();

        // Get our health componenet
        ourHealth = GetComponent<PlayerHealth>();
    }
	
	/// <summary>
    /// Check for input from the player, shoot a black hole
    /// if needed
    /// </summary>
	void Update ()
    {
        // If we are dead, then do nothing
        if (ourHealth.IsDead || GameManager.gameManager.CurrentGameState != GameState.Playing) return;

        // If the player is giving input and the time since last fire is greater then the minimum we need...
        if (Input.GetAxis(fireInput) > 0.1f && timeSinceLastFire >= fireRate)
        {
            // ================= Fire a black hole ===================
            // Get a bullet object off of the object pooler
            GameObject bullet = objPool.GetPooledObject();
            // Set the position of the bullet to that of the spawn point
            bullet.transform.position = bulletSpawn.position;

            // Set the rotiation of the bullet to that of the spawn point
            bullet.transform.rotation = bulletSpawn.rotation;

            // Set the object as active
            bullet.SetActive(true);

            // Enable the black hole that we just shot
            bullet.GetComponent<BlackHole>().EnableBlackHole();

            // The last time that we fired is now, so set the time since last fire to 0
            timeSinceLastFire = 0f;
        }
        else
        {
            // Add to the time since we fired
            timeSinceLastFire += Time.deltaTime;
        }
	}
}
