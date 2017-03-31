using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move this traps around the screen, attracting them towards the black holes
/// Author: Ben hoffman
/// </summary>
public class TrapMover : Movement {

    private Vector2 camLimits;  // The limits of the camera so that we know where to spawn these

    public override void Awake()
    {
        // Call the base movement object
        base.Awake();

        // Get the size of the camera
        camLimits.x = Camera.main.pixelWidth;
        camLimits.y = Camera.main.pixelHeight;

        // Put us at a random position to start
        ResetMovement();
	}

    public override void CalculateMovement()
    { 
        // Calculate the attraction forces and flee forces
        moveForce += CalculateAttractions();
    }

    /// <summary>
    /// Get sucked into a black hole if we should
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // If we are colliding with a black hole...
        if (other.CompareTag("BlackHole"))
        {
            // Move us out off the screen
            ResetMovement();
        }
    }

    /// <summary>
    /// Move this object outside of the play area,
    /// aim it towards the center of the screen
    /// Assign the max speed to something with a little variation
    /// so that they are not all the same
    /// </summary>
    private void ResetMovement()
    {
        // Randomly determine if our movement veloicty wll me negatice or now
        int beNegative = Random.Range(0, 1);

        // Generate a ranbd
        float randomVal = 1 + Random.Range(0.5f, 0.7f);

        // Randomly make this negative
        if(beNegative != 0)
        {
            // If we are supposed to be negative (50,50 cahnge) then multiply by -1
            randomVal *= -1f;
        }

        // Create a new position for use to have that is somewhere outside of the screen space so that we float in from outside
        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(1.5f + randomVal, 1.5f + randomVal, 0f));

        // Set our position to the new calcualted position
        transform.position = v3Pos;
    }
}
