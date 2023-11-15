using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKeys 
{
    public bool LeftKey { get; private set; }
    public bool RightKey { get; private set; }

    public bool JumpKey { get; set; }

    public InputKeys()
    {
        LeftKey = false;
        RightKey = false;
        JumpKey = false;
    }

    public void SetLeftKey()
    {
        LeftKey = true;
        RightKey = false;
    }

    public void SetRightKey()
    {
        LeftKey = false;
        RightKey = true;
    }
}
