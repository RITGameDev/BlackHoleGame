using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will simply allow player movement 
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    public float speed = 1f;

    private string horizontalInputString = "Horizontal";
    private string verticalInputString = "Vertical" ;

    private Rigidbody2D rb;
    private float moveX;
    private float moveY;

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
        // Get the input from the player
        moveX = Input.GetAxis(horizontalInputString);
        moveY = Input.GetAxis(verticalInputString);

        rb.velocity = new Vector2(moveX * speed, moveY * speed);


    }
}
