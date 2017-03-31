using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script will be in controll of telling the images
/// on the player life UI when to go away and play their animations
/// Author: Ben Hoffman
/// </summary>
public class PlayerLives : MonoBehaviour {

    public Color SpriteColor;       // The color that the players are that these images will be

    private Stack<Image> life_images;       // A stack of the images that represent the player lives

	// Use this for initialization
	void Start ()
    {
        //Instatniate our stack
        life_images = new Stack<Image>();
        // Get all the images on this object
        Image[] images = GetComponentsInChildren<Image>();
        // Iterate through those images and set their colors, and push them to our stack for later
        for(int i = 0; i < images.Length; i++)
        {
            // Change their color
            images[i].color = SpriteColor;
            // Push them to our stack for later
            life_images.Push(images[i]);
        }
    }
	
    /// <summary>
    /// Remove a image from the stack, and play the animation
    /// </summary>
	public void RemoveLife()
    {
        try
        {
            life_images.Pop().GetComponent<Animator>().SetTrigger("GoAway");
        }
        catch
        {
#if UNITY_EDITOR
            Debug.Log("There are not the same amount of UI elements as there are player lives!");
#endif
        }
    }
}
