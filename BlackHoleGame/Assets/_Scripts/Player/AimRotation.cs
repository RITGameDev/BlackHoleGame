using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will allow the aiming device to be rotated around and follow the mouse positoin
/// </summary>
public class AimRotation : MonoBehaviour {

    #region  Fields
    private string horiz_input = "RS_HORIZ_1";
    private string vert_input = "RS_VERT_1";

    public Transform center;
    private Vector3 v;

    private float yRot, xRot;
    private Vector3 centerScreenPos;
    private Vector3 dir;
    private float angle;
    private Quaternion q;
    #endregion

    /// <summary>
    /// Set the positoin to be centered at start
    /// </summary>
    void Start()
    {
        // Makes the block positoin be at the center at start
        v = (transform.position - center.position);
    }

    /// <summary>
    /// Update the positoin of this object based on the mouse rotation 
    /// </summary>
    void Update()
    {
        centerScreenPos = Camera.main.WorldToScreenPoint(center.position).normalized;

        yRot = Input.GetAxis(horiz_input) * 60f;
        xRot = -Input.GetAxis(vert_input) * 45f;

        Vector3 INPUT = new Vector3(yRot, xRot, 0F);
        
        // If we are not giving input then ditch this
        if(INPUT.magnitude <= 0f)
        {
            return;
        }
        dir =  INPUT - centerScreenPos;

        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position = center.position + q * v;
        transform.rotation = q;
    }
}
