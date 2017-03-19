using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMover : Movement {

    private Rigidbody2D rb;
    private Vector2 moveForce;

	// Use this for initialization
	void Awake() {
        rb = GetComponent<Rigidbody2D>();
	}

    private void FixedUpdate()
    {
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
}
