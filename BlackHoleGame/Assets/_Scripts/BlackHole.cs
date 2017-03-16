using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add a forward force on enable on this object
/// Require a Rigidbody2D component so that the GetComponent
/// will always work
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BlackHole : MonoBehaviour {
    [SerializeField]
    private float lifetime = 5f;    // How long it wll take for this gameobject to go inactive
    [SerializeField]        // Serialze field makes it so that we can see this field in the editor
    private float range = 1f;    // How much force we want to add to this
    [SerializeField]
    private float smoothing = 1f;   // How much smoothing is added to the movement of this object
    private Rigidbody2D rb;     // The 2D rigidbody component of this object
    private IEnumerator currentMovement;

    /// <summary>
    /// Get the reference to the rigidbody
    /// </summary>
    private void Awake()
    {
        // Get the 2D rigidbody component and add an impulse force to it
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Set the target of movement to whatever directoin we are facing
    /// </summary>
    private void OnEnable()
    {
        // Make the target in front of us by the range
        Vector3 target = transform.position + transform.up * range;

        // If the movement coroutine we have is not null, then stop it.
        if(currentMovement != null)
        {
            // Stop the movement coroutine
            StopCoroutine(currentMovement);
        }

        // Store the coroutine of movement so that we can keep track of it, and stop it if we need to
        currentMovement = MoveTo(target);
        // Start the corouine
        StartCoroutine(currentMovement);

        // Disable this obejct after the given lifetime
        Invoke("DisableMe", lifetime);
    }

    /// <summary>
    /// Set up a method to stop moving the object
    /// </summary>
    public void StopMovement()
    {
        // As long as our movement coroutine exists, then stop it
        if(currentMovement != null)
        {
            // Stop the movement coroutine
            StopCoroutine(currentMovement);
        }
    }

    /// <summary>
    /// slowly move from one spot to another
    /// </summary>
    /// <param name="target">The spot that this object wants to be at</param>
    /// <returns></returns>
    private IEnumerator MoveTo(Vector3 target)
    {
        // While we are not at the target position...
        while((transform.position - target).magnitude > 0.5f)
        {
            // Move this rigidbody towards that positoin
            rb.MovePosition(Vector3.Lerp(transform.position, target, Time.deltaTime * smoothing));
            // Wait until the next frame, so that we don't studder
            yield return null;
        }    
    }

    /// <summary>
    /// Disable this game object in the heirachy
    /// </summary>
    private void DisableMe()
    {
        gameObject.SetActive(false);
    }

}
