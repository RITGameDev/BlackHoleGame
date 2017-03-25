using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add a forward force on enable on this object
/// Require a Rigidbody2D component so that the GetComponent
/// will always work
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BlackHole : Movement {

    #region Fields
    [SerializeField]
    private float lifetime = 5f;    // How long it wll take for this gameobject to go inactive
    [SerializeField]        // Serialze field makes it so that we can see this field in the editor
    private float range = 1f;    // How much force we want to add to this
    [SerializeField]
    private float forwardWeight = 10f;

    // The current size component of this black hole object
    private BlackHole_Size size;

    private Vector3 target;  // The object that we will move towards

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

        // Get the black holes size componenet
        size = GetComponent<BlackHole_Size>();
    }

    /// <summary>
    /// Set the target of movement to whatever directoin we are facing
    /// </summary>
    public void EnableBlackHole()
    {
        // Enable the collider
        size.enabled = true;

        // Play the away animation
        //anim.SetTrigger("MakeHole");

        // Add this object to the black hole manager
        BlackHoleManager.currentBlackHoles.BlackHoles.Add(this);

        // Make the target in front of us by the range
        target = transform.position + transform.up * range;
        // Make sure that we have no other invokes happeneing
        CancelInvoke();
        // Disable this obejct after the given lifetime
        Invoke("DisableMe", lifetime);
    }

    /// <summary>
    /// Move this object towards it's target, and 
    /// the other black holes
    /// </summary>
    public override void CalculateMovement()
    {
        // Seel our target
        moveForce += Seek(target) * forwardWeight;

        // Calculate the attraction forces and flee forces
        moveForce += CalculateAttractions(rb.velocity);
    }

    /// <summary>
    /// Disable this game object in the heirachy
    /// </summary>
    public void DisableMe()
    {
        // Move us out of the way
        transform.position = new Vector3(1000, 1000, 0f);

        // Remove us from the list of black holes
        BlackHoleManager.currentBlackHoles.BlackHoles.Remove(this);
    }

    /// <summary>
    /// Reset the lifetime of this object by canceling the invoke of the 
    /// disable method, and starting it again with it's currnet lifetime
    /// </summary>
    public void ResetLifetime()
    {
        CancelInvoke();
        Invoke("DisableMe", lifetime);
    }

}
