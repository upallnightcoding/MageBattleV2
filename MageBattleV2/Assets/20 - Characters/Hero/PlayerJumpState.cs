using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : FiniteState
{
    public static string TITLE = "Jump";

    private float jumpHeight = 0.0f;

    public PlayerJumpState(float jumpHeight) : base(TITLE)
    {
        this.jumpHeight = jumpHeight;
    }

    public override void OnEnter()
    {
        Context.SetJumpHeight(jumpHeight);
    }

    public override void OnExit() { }

    public override string OnUpdate(InputCntrl inputKeys, float dt)
    {
        string state = null;

        if (inputKeys.RightKey) state = PlayerMoveRightState.TITLE;

        if (inputKeys.LeftKey) state = PlayerMoveLeftState.TITLE;

        Context.UpdateJumpHeight(Physics.gravity.y * dt);

        return (state);
    }
}
