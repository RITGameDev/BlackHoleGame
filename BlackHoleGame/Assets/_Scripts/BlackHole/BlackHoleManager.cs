using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will hole all of the active black hole 
/// objects in the scene, so that the player movement
/// can be calculated from all of them.
/// </summary>
public class BlackHoleManager : MonoBehaviour {

    // A public static reference to this object
    public static BlackHoleManager currentBlackHoles;

    // A linked list of all black holes in the scene
    private List<BlackHole> blackHoles;
    private ObjectPooler blackHolesPool;
    private bool isWorking;

    public List<BlackHole> BlackHoles { get { return blackHoles;  } set { blackHoles = value; } }
    public ObjectPooler BlackHolesObjectPool { get { return blackHolesPool; } }

    private void Awake()
    {
        // Make sure that we are only have one of these in our scene
        if (currentBlackHoles == null)
        {
            // Set the currenent referece
            currentBlackHoles = this;
        }
        else if (currentBlackHoles != this)
            Destroy(gameObject);

        // Initalize the linked list
        blackHoles = new List<BlackHole>();
        // Get the black hole object pooler
        blackHolesPool = GetComponent<ObjectPooler>();
    }

    /// <summary>
    /// This method will take in two black holes, randomly pick one,
    /// and tell that one to destroy itself, and tell the other to merge
    /// </summary>
    /// <param name="a">One black holes</param>
    /// <param name="b">Another black hole</param>
    public void MergeTwo(BlackHole_Size a, BlackHole_Size b)
    {
        //if (!isWorking)
      //  {
            StartCoroutine(MergeTwo(a.CurrentSize + b.CurrentSize, a.gameObject.transform.position));
      //  }
    }

    private IEnumerator MergeTwo(float size, Vector3 position)
    {
        // Make sure that we know that we are currently working on something
        isWorking = true;

        // Get a new black hole object at the 
        BlackHole_Size temp = blackHolesPool.GetPooledObject().GetComponent<BlackHole_Size>();

        // Set the position to the old on the a object
        temp.transform.position = position;
        // set the size 
        temp.CurrentSize = size;
        
        // we are done working on something
        isWorking = false;
        yield return null;
    }

}
