using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext 
{
    public Vector3 moveDirection = Vector3.zero;
    private float jumpHeight = 0.0f;
    public bool IsGrounded { get; set; } = true;
    private Animator animator;
    private CharacterController charCntrl;
    private GameObject gameObject;

    public void SetMove(float x)
    {
        moveDirection.x = x;
        moveDirection.y = jumpHeight;
        moveDirection.z = 0.0f;
    }

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
