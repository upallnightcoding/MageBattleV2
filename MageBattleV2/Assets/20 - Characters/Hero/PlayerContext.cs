using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext 
{
    public Vector3 moveDirection = Vector3.zero;
    public float ySpeed = 0.0f;

    public void SetMove(float x, float y, float z)
    {
        moveDirection.x = x;
        moveDirection.y = y;
        moveDirection.z = z;
    }

    public void YSpeed(float value)
    {
        ySpeed = value;
    }
}
