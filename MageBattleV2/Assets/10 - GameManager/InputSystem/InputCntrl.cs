using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputCntrl : MonoBehaviour
{
    public bool LeftKey { get; private set; }
    public bool RightKey { get; private set; }

    public bool JumpKey { get; private set; }

    public InputCntrl()
    {
        LeftKey = false;
        RightKey = false;
        JumpKey = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpKey = true;
        }
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftKey = true;
            RightKey = false;
        }
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftKey = false;
            RightKey = true;
        }
    }
}
