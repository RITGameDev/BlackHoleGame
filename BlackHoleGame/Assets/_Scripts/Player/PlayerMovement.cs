using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will simply allow player movement 
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    public float acceleration = 1f;
    public float maxSpeed = 1f;

    private string horizontalInputString = "Horizontal";
    private string verticalInputString = "Vertical" ;

    private Rigidbody2D rb;
    private float moveX;
    private float moveY;
    private Vector2 moveForce;

	/// <summary>
    /// Get the proper components
    /// </summary>
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
	/// <summary>
    /// Check for input from the player
    /// </summary>
	void FixedUpdate ()
    {
        // Reset the move force
        moveForce = Vector2.zero;

        // Do the movement calcluations
        Move();

        // Set the velocity to what we calculated
        rb.velocity = Vector2.ClampMagnitude(moveForce, maxSpeed);
    }

    private void Move()
    {
        // Get the input from the player
        moveX = Input.GetAxis(horizontalInputString);
        moveY = Input.GetAxis(verticalInputString);

        // calculate the x and y directions
        moveForce.x += moveX * acceleration;
        moveForce.y += moveY * acceleration;
    }

    /// <summary>
    /// On trigger enter with a black hole, calculate the attraction force
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    /// <summary>
    /// On trigger exit with a black hole, stop calculating the the attraction force
    /// /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
