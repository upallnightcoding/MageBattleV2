using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroCntrl : MonoBehaviour
{
    [SerializeField] private Transform spellCastPoint;
    [SerializeField] private SpellSO spell;
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundMask;

    private float playerSpeed = 4.0f;
    private float floorLevel = -0.056f;
    private Vector2 playerMove;
    private CharacterController charCntrl;
    private Animator animator;
    //private Vector3 moveDirection;

    private SpellCasterCntrl spellCaster = null;

    private FiniteStateMachine fsm = null;

    private bool castSpellRequest = false;

    private float timeBetweenCast = 0.0f;

    private bool onJump = false;

    private InputKeys inputKeys = new InputKeys();

    private PlayerContext playerContext = null;

    // Start is called before the first frame update
    private void Awake()
    {
        charCntrl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerContext = new PlayerContext();

        fsm = new FiniteStateMachine(playerContext);
        fsm.Add(new PlayerIdleState());
        fsm.Add(new PlayerMoveLeftState());
        fsm.Add(new PlayerMoveRightState());
        fsm.Add(new PlayerJumpState());

        spellCaster = new SpellCasterCntrl();
        spellCaster.Set(spell);
    }

    // Update is called once per frame
    private void Update()
    {
        fsm.OnUpdate(inputKeys, Time.deltaTime);

        MovePlayer(playerContext, Time.deltaTime);

        CastSpell(Time.deltaTime);
    }

    public void MovePlayer(PlayerContext playerContext, float dt)
    {
        playerContext.ySpeed += Physics.gravity.y * Time.deltaTime;

        RaycastHit hit;
        bool isGrounded = Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 2.0f);

        Debug.Log($"Ground: {isGrounded}, {gameObject.transform.position}");

        if (isGrounded)
        {
            playerContext.ySpeed = 0.0f;

            if (onJump)
            {
                playerContext.ySpeed = jumpSpeed;
                //isJumping = true;
                onJump = false;
            }
        }

        playerContext.moveDirection.Normalize();

        if (playerContext.moveDirection != Vector3.zero)
        {
            float inputMagnitude = Mathf.Clamp01(playerContext.moveDirection.magnitude);

            Vector3 velocity = inputMagnitude * maximumSpeed * playerContext.moveDirection;
            velocity.y = playerContext.ySpeed;

            animator.SetFloat("Speed", inputMagnitude, 0.05f, dt);

            charCntrl.Move(velocity * dt);

            Quaternion toRotation = Quaternion.LookRotation(playerContext.moveDirection, Vector3.up);

            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, toRotation, rotationSpeed * dt);
        }
    }

    private void CastSpell(float dt)
    {
        if (castSpellRequest)
        {
            spellCaster.Cast(spellCastPoint.position, transform.forward, timeBetweenCast);
            castSpellRequest = false;
            timeBetweenCast = 0.0f;
        } else
        {
            timeBetweenCast += dt;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        castSpellRequest = true;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputKeys.JumpKey = true;
        }
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputKeys.SetLeftKey();
        }
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputKeys.SetRightKey();
        }
    }
}

public class PlayerMoveRightState : FiniteState
{
    public static string TITLE = "MoveRight";

    public PlayerMoveRightState() : base(TITLE) { }

    public override void OnEnter() { }

    public override void OnExit() { }

    public override string OnUpdate(InputKeys inputKeys, float dt) 
    {
        string state = PlayerMoveRightState.TITLE;

        if (inputKeys.LeftKey) state = PlayerMoveLeftState.TITLE;

        if (inputKeys.JumpKey) state = PlayerJumpState.TITLE;

        Context.SetMove(1.0f, 0.0f, 0.0f);

        return (state);
    }
}

public class PlayerMoveLeftState : FiniteState
{
    public static string TITLE = "MoveLeft";

    public PlayerMoveLeftState() : base(TITLE) { }

    public override void OnEnter() { }

    public override void OnExit() { }

    public override string OnUpdate(InputKeys inputKeys, float dt) 
    {
        string state = PlayerMoveLeftState.TITLE;

        if (inputKeys.RightKey) state = PlayerMoveRightState.TITLE;

        if (inputKeys.JumpKey) state = PlayerJumpState.TITLE;

        Context.SetMove(-1.0f, 0.0f, 0.0f);

        return (state);
    }
}

public class PlayerJumpState : FiniteState
{
    public static string TITLE = "Jump";

    private float ySpeed = 10.0f;

    public PlayerJumpState() : base(TITLE) { }

    public override void OnEnter() 
    {
        ySpeed = 20.0f;
        Context.YSpeed(ySpeed);
    }

    public override void OnExit() { }

    public override string OnUpdate(InputKeys inputKeys, float dt)
    {
        string state = PlayerJumpState.TITLE;

        if (inputKeys.RightKey) state = PlayerMoveRightState.TITLE;

        if (inputKeys.LeftKey) state = PlayerMoveLeftState.TITLE;

        return (state);
    }
}

public class PlayerIdleState : FiniteState
{
    public static string TITLE = "Idle";

    public PlayerIdleState() : base(TITLE) { }

    public override void OnEnter() { }

    public override void OnExit() { }

    public override string OnUpdate(InputKeys inputKeys, float dt) 
    {
        string state = PlayerIdleState.TITLE;

        if (inputKeys.RightKey) state = PlayerMoveRightState.TITLE;

        if (inputKeys.LeftKey) state = PlayerMoveLeftState.TITLE;

        Context.SetMove(0.0f, 0.0f, 0.0f);

        return (state);
    }
}

public enum MoveState
{
    MOVE_LEFT,
    IDLE,
    MOVE_RIGHT,
    JUMP
}

