using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will resize black holes when they collide with each other
/// Author: Ben hoffman
/// </summary>
public class BlackHole_Size : MonoBehaviour {

    private float startSize = 1f;       // The starting size of the black holes
    private float maxSize = 2.5f;       // The max size of the black holes
    private float currentSize;          // The current size of the black hole
    private float smoothing = 10f;      // Smoothing transition for when we resize
    private IEnumerator currentSizingRoutine;   // The current resizing coroutine

    public float CurrentSize
    {
        get { return currentSize; }
        set
        {
            // Actually set the current size variable
            currentSize = value;

            // Start start the size up routine
            if(currentSizingRoutine != null)
            {
                StopCoroutine(currentSizingRoutine);
            }
            if (isActiveAndEnabled)
            {
                // start a new sizing coroutine based on our new current size
                currentSizingRoutine = ChangeSize(currentSize, smoothing, true);
                StartCoroutine(currentSizingRoutine);
            }

        }
    }

	/// <summary>
    /// Set our current size to the start size
    /// </summary>
	void Awake ()
    {
        // Set the current size to the start size
        currentSize = startSize;
	}

    /// <summary>
    /// Detects if we are in a trap or not
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // If we are colliding with a trap object...
        if (other.CompareTag("BlackHole"))
        {
            // Get the size of the other object
            float otherSize = other.GetComponentInParent<BlackHole_Size>().CurrentSize;

            // If the object is bigger, then destroy us.
            if(otherSize > currentSize)
            {
                // Set my size to 0
                CurrentSize = 0f;

                // Disable our attraction because we are smaller
                GetComponent<BlackHole>().DisableMe();
                return;
            }
            // If we are bigger then the o
            else if(otherSize < currentSize)
            {
                // Increase my size, thus increasing how much things are attracted to me
                CurrentSize = Mathf.Clamp(currentSize + otherSize, 1f, maxSize);
            }
        }
    }

    /// <summary>
    /// Lerp between sizes of this object
    /// </summary>
    /// <param name="newSize">The new size that we want</param>
    /// <param name="smoothingValue">How quickly do we want this to happen</param>
    /// <param name="resetLifetime">Do we want to reset the lifetime of this object?</param>
    /// <returns></returns>
    private IEnumerator ChangeSize(float newSize, float smoothingValue, bool resetLifetime)
    {
        // If we are above the max size then stop
        if(currentSize == maxSize)
        {
            yield break;
        }
        // If we want to reset our lifetime on this merge (sometime in the future we may not )
        if (resetLifetime)
        {
            // Reset our lifetime since now we are getting bigger
            GetComponent<BlackHole>().ResetLifetime();
        }

        // Create a new target size based on the value
        Vector3 newScale = new Vector3(newSize, newSize, newSize);

        // Loop while we are smaller then the new scale that we want
        while (transform.localScale.x < newScale.x + 0.5f)
        {
            // Lerp between what we have now and what we want, with the smoothing variable
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * smoothingValue);
            // Return null to make smooth frames
            yield return null;
        }
    }

    /// <summary>
    /// Reset the scale of this on enable, so that when we pool objects
    /// they do not spawn again as max size
    /// </summary>
    void OnEnable()
    {
        // Create a new sclae variable 
        Vector3 newScale = new Vector3(0f, 0f, 0f);
        transform.localScale = newScale;

        // Change the current size to something with some random variation
        CurrentSize = startSize + Random.Range(0f, .4f);
    }
}
