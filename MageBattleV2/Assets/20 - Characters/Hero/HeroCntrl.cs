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
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private InputCntrl inputCntrl;

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
        fsm.Add(new PlayerJumpState(jumpHeight));

        spellCaster = new SpellCasterCntrl();
        spellCaster.Set(spell);
    }

    // Update is called once per frame
    private void Update()
    {
        fsm.OnUpdate(inputCntrl, Time.deltaTime);

        MovePlayer(playerContext, Time.deltaTime);

        CastSpell(Time.deltaTime);
    }

    public void MovePlayer(PlayerContext playerContext, float dt)
    {
        RaycastHit hit;
        playerContext.IsGrounded = Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 2.0f);

        /*Debug.Log($"Ground: {isGrounded}, {gameObject.transform.position}");

        if (isGrounded)
        {
            playerContext.SetJumpHeight(0.0f);

            if (onJump)
            {
                playerContext.SetJumpHeight(jumpHeight);
                onJump = false;
            }
        }*/

        playerContext.moveDirection.Normalize();

        if (playerContext.moveDirection != Vector3.zero)
        {
            float inputMagnitude = Mathf.Clamp01(playerContext.moveDirection.magnitude);

            Vector3 velocity = inputMagnitude * maximumSpeed * playerContext.moveDirection;
            velocity.y = playerContext.GetJumpHeight();

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
}

public class PlayerMoveRightState : FiniteState
{
    public static string TITLE = "MoveRight";

    public PlayerMoveRightState() : base(TITLE) { }

    public override void OnEnter() { }

    public override void OnExit() { }

    public override string OnUpdate(InputCntrl inputKeys, float dt) 
    {
        string state = null;

        if (inputKeys.LeftKey) state = PlayerMoveLeftState.TITLE;

        if (inputKeys.JumpKey) state = PlayerJumpState.TITLE;

        Context.SetMove(1.0f);

        return (state);
    }
}

public class PlayerMoveLeftState : FiniteState
{
    public static string TITLE = "MoveLeft";

    public PlayerMoveLeftState() : base(TITLE) { }

    public override void OnEnter() { }

    public override void OnExit() { }

    public override string OnUpdate(InputCntrl inputKeys, float dt) 
    {
        string state = null;

        if (inputKeys.RightKey) state = PlayerMoveRightState.TITLE;

        if (inputKeys.JumpKey) state = PlayerJumpState.TITLE;

        Context.SetMove(-1.0f);

        return (state);
    }
}

public class PlayerIdleState : FiniteState
{
    public static string TITLE = "Idle";

    public PlayerIdleState() : base(TITLE) { }

    public override void OnEnter() { }

    public override void OnExit() { }

    public override string OnUpdate(InputCntrl inputKeys, float dt) 
    {
        string state = PlayerIdleState.TITLE;

        if (inputKeys.RightKey) state = PlayerMoveRightState.TITLE;

        if (inputKeys.LeftKey) state = PlayerMoveLeftState.TITLE;

        Context.SetMove(0.0f);

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

