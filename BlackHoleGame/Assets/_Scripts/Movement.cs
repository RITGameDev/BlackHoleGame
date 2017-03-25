using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The basis of every moving object in the scene, that will have
/// all the calcualtions that a moving object should be able to do
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {

    #region Fields
    [SerializeField]
    private float seekWeight = 1f;
    //[SerializeField]
    //private float fleeWeight = 1f;
    [SerializeField]
    private float wrapPadding = 0.5f;
    [SerializeField]
    private float maxSpeed = 1f;

    public Vector2 moveForce;  // How much we want to move
    public Rigidbody2D rb;      // Our 2d rigidbody object

    // Use these for calculations
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Vector2 steeringForce;
    private Vector2 position;

    private Vector2 velocityWhenPaused; // This will store our movement when pause, so that we can set it back when we resume

    public bool Paused { get; set; }
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

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
    }

    /// <summary>
    /// This will be where each individual movement object calculates their movement
    /// </summary>
    public virtual void CalculateMovement() { }

    /// <summary>
    /// Sleep the rigid body object
    /// </summary>
    public void PauseMovement()
    {
        // If this RB is awake...
        if (rb.IsAwake())
        {
            velocityWhenPaused = rb.velocity;
            // This will sleep the rigidbody
            rb.Sleep();
        }
    }

    /// <summary>
    /// Wake up the rigidbody object
    /// </summary>
    public void ResumeMovement()
    {
        // If the RB is asleep...
        if (!rb.IsAwake())
        {
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
        position = transform.position;

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

        // Check the Y 
        if (viewportPosition.y > 1)
        {
            position.y = -position.y + wrapPadding;
            // Having this set transform statement will make it so that I only have to call it when I
            // know that I moved this object. This will reduce the number of times that this method
            // is called.
            transform.position = position;
        }
        if (viewportPosition.y < 0)
        {
            position.y = -position.y - wrapPadding;
            transform.position = position;
        }

        // Check the X
        if (viewportPosition.x > 1)
        {
            position.x = -position.x + wrapPadding;
            transform.position = position;
        }
        if (viewportPosition.x < 0)
        {
            position.x = -position.x - wrapPadding;
            transform.position = position;
        }

    }

    /// <summary>
    /// Loop through our attracted forces and add to our move force each one
    /// </summary>
    public Vector2 CalculateAttractions(Vector2 currentVelocity)
    {
        velocity = currentVelocity;
        Vector2 attractionForce = Vector3.zero;

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

        // Flee from all the ojbects that we want to avoid

        return attractionForce;
    }

    /// <summary>
    /// Author: Ben Hoffman
    /// Purpose of method: To calculate the steering force
    /// </summary>
    /// <param name="targetPos">The target position that we want to seek</param>
    /// <returns> The steering force</returns>
    public Vector2 Seek(Vector2 targetPos)
    {
        // Calculate desired velocity
        desiredVelocity = (targetPos - position);

        // In order to get faster as we get closer, because the distance will get smaller, thus
        // resulting in a larger multiplier
        desiredVelocity *= (1 / desiredVelocity.magnitude);

        // Scale the magnitude to teh max speed
        // so that I move as quickly as possible
        desiredVelocity.Normalize();

        desiredVelocity *= maxSpeed;

        steeringForce = desiredVelocity - velocity;

        // Calculate the steering force 
        return steeringForce;
    }

    /// <summary>
    /// Author: Ben Hoffman
    /// Purpose of method: TO have this obejct flee from 
    /// the given Vector3
    /// </summary>
    /// <param name="fleeingFromThis"></param>
    /// <returns></returns>
    private Vector2 Flee(Vector2 fleeingFromThis)
    {
        // Calculate desired velocity
        desiredVelocity = position - fleeingFromThis;

        // Scale the magnitude to teh max speed
        // so that I move as quickly as possible
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        // Calculate the steering force 
        // steering force = desired velocity - current velocity
        // Return the steering force
        steeringForce = desiredVelocity - velocity;

        return steeringForce;
    }
}
