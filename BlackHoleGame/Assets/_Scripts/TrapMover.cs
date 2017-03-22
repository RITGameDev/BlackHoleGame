using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMover : Movement {

    private Rigidbody2D rb;
    private Vector2 moveForce;
    private Vector2 camLimits;

	// Use this for initialization
	void Awake() {
        rb = GetComponent<Rigidbody2D>();

        // Get the size of the camera
        camLimits.x = Camera.main.pixelWidth;
        camLimits.y = Camera.main.pixelHeight;

        // Put us at a random position to start
        ResetMovement();
	}


    private void FixedUpdate()
    {
        // If we are outside of the screen view, then seek the center

        // Do the movement calculations
        moveForce = Vector2.zero;    

        // Calculate the attraction forces and flee forces
        moveForce += CalculateAttractions();

        // Have the player warp around the screen
        WrapAroundScreen();

        // Set the velocity to what we calculated
        rb.AddForce(moveForce);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
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
