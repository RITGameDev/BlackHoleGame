using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour {

    public Color SpriteColor;

    private Stack<Image> life_images;

	// Use this for initialization
	void Start ()
    {
        life_images = new Stack<Image>();
        // Get all the images on this object
        Image[] images = GetComponentsInChildren<Image>();
        for(int i = 0; i < images.Length; i++)
        {
            images[i].color = SpriteColor;
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

        }
    }
}
