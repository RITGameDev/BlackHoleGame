using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move this traps around the screen, attracting them towards the black holes
/// and 
/// </summary>
public class TrapMover : Movement {

    private Vector2 camLimits;

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
        moveForce += CalculateAttractions(rb.velocity);
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
            // Play the animation of getting sucked in
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
        int beNegative = Random.Range(0, 1);

        // Generate a ranbd
        float randomVal = 1 + Random.Range(0.5f, 0.7f);
        // Randomly make this negative
        if(beNegative != 0)
        {
            randomVal *= -1f;
        }

        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(1.5f + randomVal, 1.5f + randomVal, 0f));

        transform.position = v3Pos;
    }
}
