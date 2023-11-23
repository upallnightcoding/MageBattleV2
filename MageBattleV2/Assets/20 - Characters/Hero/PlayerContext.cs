using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext 
{
    
    private float jumpHeight = 0.0f;
    public bool IsGrounded { get; set; } = true;

   

    public void SetJumpHeight(float value)
    {
        jumpHeight = value;
    }

    public float GetJumpHeight() => jumpHeight;

    public void UpdateJumpHeight(float value)
    {
        jumpHeight += value;
    }
}
