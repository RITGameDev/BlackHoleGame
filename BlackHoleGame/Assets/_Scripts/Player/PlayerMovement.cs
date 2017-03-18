using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will simply allow player movement 
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    #region Fields
    [SerializeField]
    private float playerWeight = 1f;
    [SerializeField]
    private float maxSpeed = 1f;
    [SerializeField]
    private float seekWeight = 1f;
    [SerializeField]
    private float fleeWeight = 1f;

    private string horizontalInputString = "Horizontal";
    private string verticalInputString = "Vertical" ;

    private Rigidbody2D rb;
    private float moveX;
    private float moveY;
    private Vector2 moveForce;

    private Vector2 desiredVelocity;
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 steeringForce;
    #endregion

    /// <summary>
    /// Get the proper components
    /// </summary>
    void Start ()
    {
        // Get the 2D rigidbody component of this object
        rb = GetComponent<Rigidbody2D>();

        // Initialize the stacks that hold the things that we should be attracted to
        //attractedTo = new Stack<Vector2>();
        //fleeFrom = new Stack<Vector2>();
    }
	
	/// <summary>
    /// Check for input from the player
    /// </summary>
	void Update ()
    {
        // Reset the move force
        moveForce = Vector2.zero;

        position = transform.position;

        // Do the movement calcluations
        Move();

        // Calculate the attraction forces and flee forces
        CalculateAttractions();

        // Set the velocity to what we calculated
        rb.velocity = moveForce.normalized * maxSpeed * Time.deltaTime;
    }


    /// <summary>
    /// Calculate the player input
    /// </summary>
    private void Move()
    {
        // Get the input from the player
        moveX = Input.GetAxis(horizontalInputString);
        moveY = Input.GetAxis(verticalInputString);

        // calculate the x and y directions
        moveForce.x += moveX * playerWeight;
        moveForce.y += moveY * playerWeight;
    }

    /// <summary>
    /// Loop through our attracted forces and add to our move force each one
    /// </summary>
    private void CalculateAttractions()
    {
        // how many things do we want to be attracted towards?
        int count = BlackHoleManager.currentBlackHoles.BlackHoles.Count;

        // Seek towards all the objects that we have collided with
        for(int i = 0; i < count; i++)
        {
            // Seet the attracted position, and pop it off the stack as we do so
            moveForce += Seek(BlackHoleManager.currentBlackHoles.BlackHoles[i].transform.position) * seekWeight * Time.deltaTime;
        }

        // Flee from all the ojbects that we want to avoid
    }

    /// <summary>
    /// Author: Ben Hoffman
    /// Purpose of method: To calculate the steering force
    /// </summary>
    /// <param name="targetPos"></param>
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
    public Vector2 Flee(Vector2 fleeingFromThis)
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
