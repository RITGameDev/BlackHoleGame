using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add a forward force on enable on this object
/// Require a Rigidbody2D component so that the GetComponent
/// will always work
/// Author: Ben Hoffman
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BlackHole : Movement {

    #region Fields
    [SerializeField]
    private float lifetime = 5f;    // How long it wll take for this gameobject to go inactive
    [SerializeField]        // Serialze field makes it so that we can see this field in the editor
    private float range = 1f;    // How much force we want to add to this
    [SerializeField]
    private float seekWieght = 10f;  // How much weight we will have towards our original target

    // The current size component of this black hole object
    private BlackHole_Size size;

    private Vector3 target;  // The object that we will move towards
    private float timeSinceInvoke;  // This will be used to keep track of how long it has been since we started
    private bool isAlive;   // Bool determineint if this is alive or not
    private Vector3 deadArea;

    #endregion

    public float Size { get { return size.CurrentSize; } }

    public float Lifetime
    {
        get { return lifetime; }
        set
        {
            lifetime = value;
            ResetLifetime();
        }
    }

    public override void Awake()
    {
        // Call the base movement 
        base.Awake();

        deadArea = new Vector3(1000, 1000, 0);

        // Get the black holes size componenet
        size = GetComponent<BlackHole_Size>();

        // We are not alive to start
        isAlive = false;
        
        // It has been 0 seconds sine we have invoked this object
        timeSinceInvoke = 0f;
    }

    /// <summary>
    /// Set the target of movement to whatever directoin we are facing
    /// </summary>
    public void EnableBlackHole()
    {
        // Enable the collider
        size.enabled = true;

        // Add this object to the black hole manager
        BlackHoleManager.currentBlackHoles.BlackHoles.Add(this);

        // Make the target in front of us by the range
        target = transform.position + transform.up * range;

        // We are alive
        isAlive = true;
        // Time since we have invoked this object is now 0
        timeSinceInvoke = 0f;
    }


    private void Update()
    {
        // If we are playing that game and this black hole is alive OR the game is over....
        if(GameManager.gameManager.CurrentGameState == GameState.Playing && isAlive || GameManager.gameManager.CurrentGameState == GameState.GameOver)
        {
            // Add the delta time to how long it has been since we were active
            timeSinceInvoke += Time.deltaTime;
            // If we have exceeded our lifetime...
            if(timeSinceInvoke >= lifetime)
            {
                // Disable us
                DisableMe();
            }
        }
    }

    /// <summary>
    /// Move this object towards it's target, and 
    /// the other black holes
    /// </summary>
    public override void CalculateMovement()
    {
        // Seek our target
        moveForce += Seek(target) * seekWieght;

        // Calculate the attraction forces and flee forces
        moveForce += CalculateAttractions();
    }

    /// <summary>
    /// Disable this game object in the heirachy
    /// </summary>
    public void DisableMe()
    {
        // Move us out of the way into the dead area
        transform.position = deadArea;

        // Remove us from the list of black holes
        BlackHoleManager.currentBlackHoles.BlackHoles.Remove(this);
        
        // Set this object as dead
        isAlive = false;
    }

    /// <summary>
    /// Reset the lifetime of this object by canceling the invoke of the 
    /// disable method, and starting it again with it's currnet lifetime
    /// </summary>
    public void ResetLifetime()
    {
        // The time since we have invoked this object is 0
        timeSinceInvoke = 0f;
        // We are alive now
        isAlive = true;
    }

}
