using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will allow the aiming device to be rotated around and follow the mouse positoin
/// </summary>
public class AimRotation : MonoBehaviour {

    #region  Fields
    public Transform center;
    private Vector3 v;

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
        centerScreenPos = Camera.main.WorldToScreenPoint(center.position);
        dir = Input.mousePosition - centerScreenPos;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position = center.position + q * v;
        transform.rotation = q;
    }
}
