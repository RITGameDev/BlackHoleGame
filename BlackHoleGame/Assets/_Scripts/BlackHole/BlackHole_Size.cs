using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Size : MonoBehaviour {

    private float startSize = 1f;
    private float currentSize;
    [SerializeField]
    private float smoothing = 1f;
    private IEnumerator currentSizingRoutine;

    public float CurrentSize
    {
        get { return currentSize; }
        set
        {
            currentSize = value;
            // Start sizing this
            if(currentSizingRoutine != null)
            {
                StopCoroutine(currentSizingRoutine);
            }
            if (isActiveAndEnabled)
            {
                currentSizingRoutine = ChangeSize(currentSize);
                StartCoroutine(currentSizingRoutine);
            }

        }
    }

	// Use this for initialization
	void Awake ()
    {
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
            float otherSize = other.GetComponent<BlackHole_Size>().CurrentSize;
            // If the object is bigger or the same, then destroy us.
            if(otherSize > currentSize)
            {
                GetComponent<BlackHole>().DisableMe();
            }
            else if(otherSize < currentSize)
            {
                // Disable the other black hole if we are bigger
                other.GetComponent<BlackHole>().DisableMe();

                // Increase my size, thus increasing how much things are attracted to me
                if (currentSizingRoutine != null)
                {
                    StopCoroutine(currentSizingRoutine);
                }
                // Create a new coroutine
                currentSizingRoutine = ChangeSize(currentSize + otherSize);
                // Start it
                StartCoroutine(currentSizingRoutine);
            }
            else
            {
                // Tell the black hole manager to merge us both and create a new one
                BlackHoleManager.currentBlackHoles.MergeTwo(this, other.GetComponent<BlackHole_Size>());
            }
          
        }
    }

    private IEnumerator ChangeSize(float addToValue)
    {
        // Create a new target size based on the value
        Vector3 newScale = new Vector3(addToValue, addToValue, addToValue);

        while (transform.localScale.x < addToValue + 0.3f)
        {
            // Lerp between what we have now and what we want, with the smoothing variable
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * smoothing);
            // Return null to make smooth frames
            yield return null;
        }

        // Reset our lifetime
        GetComponent<BlackHole>().ResetLifetime();

        yield return null;
    }

    /// <summary>
    /// Reset the size when this object is disabled
    /// </summary>
    void OnDisable()
    {
        Vector3 newScale = new Vector3(startSize, startSize, startSize);
        transform.localScale = newScale;

        CurrentSize = startSize;
    }

    void OnEnable()
    {
        // Change the current size to something with some random variation
        CurrentSize = startSize + Random.Range(-0.5f, .5f);
        // Using the property instead of the field will automatically start the 
        // scalling of this
    }
}
