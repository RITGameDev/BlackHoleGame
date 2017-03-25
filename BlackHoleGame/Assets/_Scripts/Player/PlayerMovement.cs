using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will simply allow player movement 
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerMovement : Movement
{
    #region Fields
    [SerializeField]
    private float playerWeight = 1f;
    private float dashWeight = 1f;
    
    private string horizontalInputString = "Horizontal";
    private string verticalInputString = "Vertical";
    private string dashInputString = "Dash";

    private CircleCollider2D collider;

    private float moveX;    // The X input from the player
    private float moveY;    // The Y input from the player
    private Vector2 userInput;  // Used for the calculations of the player input

    private float minTimeBetweenDashes = 5f;

    private float currentDash;
    private float dashTimeLengh = 0.3f;
    private float timeSinceLastDash;
    private TrailRenderer trailRend;
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

        horizontalInputString += playerNum.ToString();
        verticalInputString += playerNum.ToString();
        dashInputString += playerNum.ToString();

        currentDash = dashWeight;
        timeSinceLastDash = minTimeBetweenDashes;
        trailRend = GetComponent<TrailRenderer>();
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
        moveForce += CalculateAttractions(rb.velocity) * currentDash;
    }

    /// <summary>
    /// Get the unput from the player
    /// </summary>
    private void GetPlayerInput()
    {
        // Get the input from the player
        moveX = Input.GetAxis(horizontalInputString);
        moveY = Input.GetAxis(verticalInputString);

        if(Input.GetAxis(dashInputString) > 0 && timeSinceLastDash >= minTimeBetweenDashes)
        {
            StartCoroutine(UseDash());
            timeSinceLastDash = 0f;
        }
        else
        {
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
        trailRend.enabled = true;

        // Wait
        yield return new WaitForSeconds(dashTimeLengh);

        currentDash = dashWeight;
        trailRend.enabled = false;
    }

    /// <summary>
    /// Calculate the player input
    /// </summary>
    private Vector2 Move()
    {
        userInput = Vector2.zero;
        // calculate the x and y directions
        userInput.x += moveX * playerWeight;
        userInput.y += moveY * playerWeight;
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
