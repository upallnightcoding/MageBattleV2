using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputCntrl : MonoBehaviour
{
    public bool LeftKey { get; private set; }
    public bool RightKey { get; private set; }

    public bool UpKey { get; private set; }

    public bool DownKey { get; private set; }

    public InputCntrl()
    {
        LeftKey = false;
        RightKey = false;
        UpKey = false;
        DownKey = false;
    }

    public void StopJumping() => UpKey = false;

    public void OnDownAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DownKey = true;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UpKey = true;
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
