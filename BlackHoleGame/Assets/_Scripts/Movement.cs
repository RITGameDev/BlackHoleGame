using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The basis of every moving object in the scene, that will have
/// all the calcualtions that a moving object should be able to do
/// Author: Ben Hoffman
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {

    #region Fields

    [SerializeField]
    private float seekWeight = 1f;      // How much weight the things that we are seeking (mostly black holes) will have
    [SerializeField]
    private float wrapPadding = 0.5f;   // How much wrap padding we want when things wrap the screen, makes it look smoother
    [SerializeField]
    private float maxSpeed = 1f;        // The max speed that this objcet can have. Used to clamp the magnitude of velocity vector

    public Vector2 moveForce;   // How much we want to move
    public Rigidbody2D rb;      // Our 2d rigidbody object

    // Use these for calculations so that we do not have to make temporary variables
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Vector2 steeringForce;
    private Vector2 currentPosition;
    private Vector2 attractionForce;

    private Vector2 velocityWhenPaused; // This will store our movement when pause, so that we can set it back when we resume

    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

    #endregion

    /// <summary>
    /// Get the RB componenet, and intialize the velocity when
    /// paused
    /// </summary>
    public virtual void Awake()
    {
        // Get the rigidibody copmponeent
        rb = GetComponent<Rigidbody2D>();

        // Initalize the velocity while paused
        velocityWhenPaused = Vector2.zero;
    }

    /// <summary>
    /// Handle the movement of this object by wrapping it aronud the screen,
    /// stoping if we are paused, and resuming if we need to. Call the 
    /// calculate movement method so that any children can put in 
    /// custom movement patterns
    /// </summary>
    private void FixedUpdate()
    {
        // If the game is paused....
        if(GameManager.gameManager.CurrentGameState == GameState.Paused)
        {
            // Make sure that our movement is paused
            PauseMovement();

            // return out of this method so that we don't waste time
            return;
        }


        // Call the overriden method of the calculate the movement of this object, if we are playing
        if (GameManager.gameManager.CurrentGameState == GameState.Playing)
        {
            // Make sure that our RB is awake
            ResumeMovement();

            // Set the move force to zero to reset the movement
            moveForce = Vector2.zero;

            // Calculate the movement of this object
            CalculateMovement();

        }

        // Wrap all objects around the screen
        WrapAroundScreen();

        // Set the velocity to what we calculated
        rb.AddForce(moveForce);

        // Clamp the velocity to the max speed, and add it to our rigidbody
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
    }

    /// <summary>
    /// This will be where each individual movement object calculates their movement
    /// </summary>
    public virtual void CalculateMovement() { }

    /// <summary>
    /// Sleep the rigid body object and store the velocity 
    /// </summary>
    public void PauseMovement()
    {
        // If this RB is awake...
        if (rb.IsAwake())
        {
            // Store the velocity when we paused so that we can set it back later
            velocityWhenPaused = rb.velocity;
            // This will sleep the rigidbody
            rb.Sleep();
        }
    }

    /// <summary>
    /// Wake up the rigidbody object and reusme moving
    /// </summary>
    public void ResumeMovement()
    {
        // If the RB is asleep...
        if (!rb.IsAwake())
        {
            // Set teh velocity to what it was when we paused
            rb.velocity = velocityWhenPaused;
            // This will wake up the rigid body
            rb.WakeUp();
        }
    }

    /// <summary>
    /// Simple wrap around the screen class
    /// </summary>
    public void WrapAroundScreen()
    {
        // Get the current position of this object
        currentPosition = transform.position;

        // Get the camera postiion of this object with WorldToViewport
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(currentPosition);

        // Check the Y 
        if (viewportPosition.y > 1) // Past the top of the screen
        {
            currentPosition.y = -currentPosition.y + wrapPadding;
            transform.position = currentPosition;
        }
        if (viewportPosition.y < 0)   // Past the bottom of the screen
        {
            currentPosition.y = -currentPosition.y - wrapPadding;
            transform.position = currentPosition;
        }

        // Check the X positoin
        if (viewportPosition.x > 1) // Past the right of the screen
        {
            currentPosition.x = -currentPosition.x + wrapPadding;
            transform.position = currentPosition;
        }
        if (viewportPosition.x < 0) // Past the left of the screen
        {
            currentPosition.x = -currentPosition.x - wrapPadding;
            transform.position = currentPosition;
        }

    }

    /// <summary>
    /// Loop through the black holes in the scene and add attraction force to everything
    /// </summary>
    /// <returns>The total attraction force towards all black holes</returns>
    public Vector2 CalculateAttractions()
    {
        // Set the current velocity to the rididbody velocity 
        velocity = rb.velocity;

        // Create a new Vector 2 attraction force
        attractionForce = Vector2.zero;

        // how many things do we want to be attracted towards?
        int count = BlackHoleManager.currentBlackHoles.BlackHoles.Count;

        // Seek towards all the objects that we have collided with
        for (int i = 0; i < count; i++)
        {
            // Seet the attracted position, and pop it off the stack as we do so
            attractionForce += Seek(
                BlackHoleManager.currentBlackHoles.BlackHoles[i].transform.position)
                * seekWeight * BlackHoleManager.currentBlackHoles.BlackHoles[i].Size;
        }

        // Return the calculated attraction force to all the black hoels
        return attractionForce;
    }

    /// <summary>
    /// Calculate a steering force that will seek the given target
    /// </summary>
    /// <param name="targetPos">The target position that we want to seek</param>
    /// <returns> The steering force what should be applied to the velocity, scaled to max speed</returns>
    public Vector2 Seek(Vector2 targetPos)
    {
        // Calculate desired velocity
        desiredVelocity = (targetPos - currentPosition);

        // In order to get faster as we get closer, because the distance will get smaller, thus
        // resulting in a larger multiplier
        desiredVelocity *= (1 / desiredVelocity.magnitude);

        // Normalize this vector so that it is just a length of 1, and represents a direction
        desiredVelocity.Normalize();

        // Scale by max speed
        desiredVelocity *= maxSpeed;

        // Calculate the steering force 
        // steering force = desired velocity - current velocity
        // Return the steering force
        steeringForce = desiredVelocity - velocity;

        // Calculate the steering force 
        return steeringForce;
    }

}
