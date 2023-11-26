using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : FiniteState
{
    public static string TITLE = "Jump";

    private HeroCntrl heroCntrl = null;

    private JumpType state = JumpType.INITIAL;

    public PlayerJumpState(HeroCntrl heroCntrl) : base(TITLE)
    {
        this.heroCntrl = heroCntrl;
    }

    public override void OnEnter()
    {
        Debug.Log("Start jump ...");
        state = JumpType.START_JUMP;
        heroCntrl.ySpeed = heroCntrl.jumpHeight;
    }

    public override void OnExit() { }

    public override string OnUpdate(InputCntrl inputKeys, float dt)
    {
        string nextState = null;

        if (inputKeys.RightKey) nextState = PlayerMoveState.TITLE;

        if (inputKeys.LeftKey) nextState = PlayerMoveState.TITLE;

        switch(state)
        {
            case JumpType.START_JUMP:
                StateStartJump(inputKeys);
                break;
            case JumpType.JUMPING:
                StateJumping(dt);
                break;
            case JumpType.END_JUMP:
                nextState = StateEndJump();
                break;
        }

        return (nextState);
    }

    private void StateStartJump(InputCntrl inputKeys)
    {
        Debug.Log("StateStartJump ...");
        state = JumpType.JUMPING;
        inputKeys.StopJumping();
    }

    private void StateJumping(float dt)
    {
        heroCntrl.ySpeed += Physics.gravity.y * dt;

        if (heroCntrl.charCntrl.isGrounded)
        {
            state = JumpType.END_JUMP;
        } else
        {
            state = JumpType.JUMPING;
        }
    }

    private string StateEndJump()
    {
        state = JumpType.INITIAL;

        heroCntrl.ySpeed = -0.5f;

        return (PlayerMoveState.TITLE);
    }
}

public enum JumpType
{
    START_JUMP,
    JUMPING,
    END_JUMP,
    INITIAL
}
