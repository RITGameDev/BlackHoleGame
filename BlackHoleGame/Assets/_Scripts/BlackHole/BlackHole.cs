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
    [SerializeField]
    private float endAnimationTime = 0.5f;
    [SerializeField]
    private float startAnimTime = 0.5f;

    private Animator anim;
    private WaitForSeconds animationTimeWait;
    private Rigidbody2D rb;
    private Vector2 moveForce;
    private BlackHole_Size size;

    private Vector3 target;

    #endregion

    public float Lifetime
    {
        get { return lifetime; }
        set
        {
            lifetime = value;
            ResetLifetime();
        }
    }

    private void Awake()
    {
        // Get the rigidbody componenet
        rb = GetComponent<Rigidbody2D>();
        // Create a animation time waitForSeconds
        animationTimeWait = new WaitForSeconds(endAnimationTime);
        // Get the animator component
        anim = GetComponent<Animator>();
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
    private void FixedUpdate()
    {
        // Reset the move force
        moveForce = Vector2.zero;

        // Seel our target
        moveForce += Seek(target) * forwardWeight;

        // Calculate the attraction forces and flee forces
        moveForce += CalculateAttractions();

        // Have the player warp around the screen
        WrapAroundScreen();

        // Set the velocity to what we calculated
        rb.AddForce(moveForce);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
    }

    /// <summary>
    /// Disable this game object in the heirachy
    /// </summary>
    public void DisableMe()
    {
        // Start the disable coroutine
        if(isActiveAndEnabled)
            StartCoroutine(DisableThis());
    }

    private IEnumerator DisableThis()
    {
        // Disable the collider
        size.enabled = false;

        // Play the animation 
        //anim.SetTrigger("ShrinkHole");

        // Wait for the animation to finish
        yield return animationTimeWait;

        // Remove myself from the list of black holes
        BlackHoleManager.currentBlackHoles.BlackHoles.Remove(this);

        // Set this object as inactive
        gameObject.SetActive(false);
    }

    public void ResetLifetime()
    {
        CancelInvoke();
        Invoke("DisableMe", lifetime);
    }

}
