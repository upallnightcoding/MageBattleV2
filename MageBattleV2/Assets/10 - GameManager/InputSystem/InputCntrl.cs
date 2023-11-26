using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputCntrl : MonoBehaviour
{
    public bool LeftKey { get; set; }
    public bool RightKey { get; set; }

    public bool FireKey { get; set; }

    public bool UpKey { get; set; }
    public bool DownKey { get; set; }

    public InputCntrl()
    {
        LeftKey = false;
        RightKey = false;
        UpKey = false;
        DownKey = false;
    }

    public void StopJumping() => UpKey = false;

    public void OnFireAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireKey = true;
        }
    }

    public void OnDownAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DownKey = true;
            UpKey = false;
        }
    }

    public void OnUpAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DownKey = false;
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
