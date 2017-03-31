using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will simply allow player movement by getting input
/// from the movement axis.
/// Author: Ben Hoffman
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerMovement : Movement
{
    #region Fields
    [SerializeField]
    private float playerWeight = 1f;        // How much weight the player movement will have
    private float dashWeight = 1f;          // How much weight the dash will have when 
    
    private string horizontalInputString = "Horizontal";    // The horizontal input string
    private string verticalInputString = "Vertical";        // The vertical input string
    private string dashInputString = "Dash";                // The dash input string

    private CircleCollider2D collider;      // The collider on this object, so that we can disable it, this way we wont die like 5 times in a row

    private float moveX;    // The X input from the player
    private float moveY;    // The Y input from the player
    private Vector2 userInput;  // Used for the calculations of the player input

    private float minTimeBetweenDashes = 3f;    // The amount of time between dashes

    private float currentDash;      // How it is has been since we dashed
    private float dashTimeLengh = 0.3f;  // How long the dash will last
    private float timeSinceLastDash;     // How long it has been since we last dashed
    private TrailRenderer trailRend;     // The trail renderer componenet that will tell the player when we are dashing
    #endregion

    /// <summary>
    /// Get the proper components
    /// </summary>
    public override void Awake()
    {
        // Call the base awake function
        base.Awake();

        // Get the collider component of this object
        collider = GetComponent<CircleCollider2D>();

        // Set up the player number input
        int playerNum = GetComponent<PlayerNumber>()._PlayerNumber;

        // Add the player number to all the input strings
        horizontalInputString += playerNum.ToString();
        verticalInputString += playerNum.ToString();
        dashInputString += playerNum.ToString();

        // Set the current dash weight to the starting dash weight
        currentDash = dashWeight;
        // Allow the player to dash right away by setting the time since last dash to the max
        timeSinceLastDash = minTimeBetweenDashes;

        // Get the current trail renderer componenet
        trailRend = GetComponent<TrailRenderer>();
        // Disable the trail renderer to start
        trailRend.enabled = false;

    }

    /// <summary>
    /// Check for input from the player
    /// </summary>
    void Update()
    {
        // Get player input
        GetPlayerInput();
    }

    /// <summary>
    /// Calculate the movement of the player
    /// </summary>
    public override void CalculateMovement()
    {
        // Do the movement calcluations
        moveForce += Move();

        // Calculate the attraction forces and flee forces
        moveForce += CalculateAttractions() * currentDash;
    }

    /// <summary>
    /// Get the unput from the player
    /// </summary>
    private void GetPlayerInput()
    {
        // Get the input from the player
        moveX = Input.GetAxis(horizontalInputString);
        moveY = Input.GetAxis(verticalInputString);

        // Check if the player wants to dash
        if(Input.GetAxis(dashInputString) > 0.1f && timeSinceLastDash >= minTimeBetweenDashes)
        {
            // Start to use the dash
            StartCoroutine(UseDash());
            // It has been 0 seconds since we last dashed
            timeSinceLastDash = 0f;
        }
        else
        {
            // Add to how long it has been since we dashed
            timeSinceLastDash += Time.deltaTime;
        }
    }

    /// <summary>
    /// This method will change the dash value, then wait, and change 
    /// it back to what it was
    /// </summary>
    /// <returns></returns>
    private IEnumerator UseDash()
    {
        // Change the dash wieght
        currentDash = 0f;
        
        // Enable the trail renderer
        trailRend.enabled = true;

        // Cleaer the trail renderer componenet
        trailRend.Clear();

        // Wait the length of the dash
        yield return new WaitForSeconds(dashTimeLengh);

        // Remove the dash weight
        currentDash = dashWeight;
        // Disable the trail renderer
        trailRend.enabled = false;
    }

    /// <summary>
    /// Calculate the player input
    /// </summary>
    private Vector2 Move()
    {
        // Reset the user input vector
        userInput = Vector2.zero;
        // calculate the x and y directions
        userInput.x += moveX * playerWeight;
        userInput.y += moveY * playerWeight;
        // Return the user input
        return userInput;
    }

    /// <summary>
    /// Make sure that the rigidbody is asleep and we are not moving
    /// when we disable this component
    /// </summary>
    public void OnDisable()
    {
        // Sleep the rigidbodyvoid OnDisable()
        rb.Sleep();
        // Set velocity to 0 so that we stop moving
        rb.velocity = Vector2.zero;
        // Disable the collider
        collider.enabled = false;
    }

    /// <summary>
    /// Make sure the rigidbody and collider are enabled when we
    /// wake up this component
    /// </summary>
    private void OnEnable()
    {
        // Wake up the rigidbody
        rb.WakeUp();
        // Set velocity to 0, in case something happened and we are moving
        rb.velocity = Vector2.zero;
        // Enable the collider
        collider.enabled = true;
    }

}
