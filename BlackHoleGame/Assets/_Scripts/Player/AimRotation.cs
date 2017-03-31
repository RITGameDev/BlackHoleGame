using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will allow the aiming device to be rotated around and follow the mouse positoin
/// </summary>
public class AimRotation : MonoBehaviour {

    #region  Fields
    private string horiz_input = "RS_HORIZ_";   // Input axis names for the right stick 
    private string vert_input = "RS_VERT_";

    public Transform center;

    private Vector3 v;

    // Used for calculations
    private float yRot, xRot;
    private Vector3 centerScreenPos;
    private Vector3 dir;
    private float angle;
    private Quaternion q;

    private PlayerHealth ourHealth; // A reference to our health so that we can check if we are dead or not
    #endregion

    /// <summary>
    /// Set the positoin to be centered at start
    /// </summary>
    void Awake()
    {
        // Makes the block positoin be at the center at start
        v = (transform.position - center.position);

        // Set up the player number input
        int playerNum = GetComponentInParent<PlayerNumber>()._PlayerNumber;
        // Set up the input strings
        horiz_input += playerNum.ToString();
        vert_input += playerNum.ToString();

        // Get our health component
        ourHealth = GetComponentInParent<PlayerHealth>();
    }

    /// <summary>
    /// Update the positoin of this object based on the mouse rotation 
    /// </summary>
    void Update()
    {
        // If we are head, then do nothing
        if (ourHealth.IsDead || GameManager.gameManager.CurrentGameState != GameState.Playing) return;

        // Get the screen point of the center of the screeen
        centerScreenPos = Camera.main.WorldToScreenPoint(center.position).normalized;

        // Get player inpput from the joysticks
        yRot = Input.GetAxis(horiz_input) * 60f;
        xRot = -Input.GetAxis(vert_input) * 45f;

        // Create an input vector based on the playe rinput that we have
        Vector3 INPUT = new Vector3(yRot, xRot, 0F);
        
        // If we are not giving input then ditch this
        if(INPUT.magnitude <= 0f)
        {
            return;
        }
        // Our directino is our input minus the center of the screen poition
        dir =  INPUT - centerScreenPos;

        // Calculate the angle that we should be facing
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        // Create a quaterionon based on that angle
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        // Set our position 
        transform.position = center.position + q * v;
        // Set our rotiation
        transform.rotation = q;
    }
}
