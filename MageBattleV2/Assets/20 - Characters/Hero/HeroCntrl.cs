using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroCntrl : MonoBehaviour
{

    [Header("Player Attribute")]
    [SerializeField] private float maximumSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] public float jumpHeight;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private InputCntrl inputCntrl;

    [Header("Spell Casting")]
    [SerializeField] private Transform spellCastPoint;
    [SerializeField] private SpellSO spell;

    // Components
    public CharacterController charCntrl;
    private Animator animator;

    private SpellCasterCntrl spellCaster = null;

    private FiniteStateMachine fsm = null;

    public Vector3 direction = Vector3.zero;

    // Hero Movement
    public Vector3 inputDirection = Vector3.zero;
    public float ySpeed = 0.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        charCntrl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        fsm = new FiniteStateMachine();
        fsm.Add(new PlayerIdleState(this));
        fsm.Add(new PlayerMoveState(this));
        fsm.Add(new PlayerJumpState(this));

        spellCaster = new SpellCasterCntrl();
        spellCaster.Set(spell);
    }

    // Update is called once per frame
    private void Update()
    {
        fsm.OnUpdate(inputCntrl, Time.deltaTime);

        UpdateEveryFrame(Time.deltaTime);

        CastSpell(inputCntrl, Time.deltaTime);
    }

    private void UpdateEveryFrame(float dt)
    {
        direction = inputDirection;
        //direction.Normalize();

        float magnitude = Mathf.Clamp01(direction.magnitude);
        Vector3 velocity = magnitude * maximumSpeed * direction;
        velocity.y = ySpeed;

        animator.SetFloat("Speed", magnitude, 0.05f, dt);

        charCntrl.Move(velocity * dt);

        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = 
                Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * dt);
        }
    }

    private void CastSpell(InputCntrl inputCntrl, float dt)
    {
        if (inputCntrl.FireKey)
        {
            inputCntrl.FireKey = false;
            spellCaster.Cast(spellCastPoint.position, transform.forward);
        } 
    }
}

public class PlayerMoveState : FiniteState
{
    public static string TITLE = "Move";

    private HeroCntrl heroCntrl = null;

    public PlayerMoveState(HeroCntrl heroCntrl) : base(TITLE) 
    {
        this.heroCntrl = heroCntrl;
    }

    public override void OnEnter() { }

    public override void OnExit() { }

    public override string OnUpdate(InputCntrl inputKeys, float dt) 
    {
        string state = null;

        if (inputKeys.RightKey)
        {
            inputKeys.RightKey = false;
            heroCntrl.inputDirection.x = -1.0f;
        }

        if (inputKeys.LeftKey)
        {
            inputKeys.LeftKey = false;
            heroCntrl.inputDirection.x = 1.0f;
        }

        if (inputKeys.UpKey)
        {
            inputKeys.UpKey = false;
            state = PlayerJumpState.TITLE;
        }

        if (inputKeys.DownKey)
        {
            inputKeys.DownKey = false;
            state = PlayerIdleState.TITLE;
        }

        return (state);
    }
}

public class PlayerIdleState : FiniteState
{
    public static string TITLE = "Idle";

    private HeroCntrl heroCntrl = null;

    public PlayerIdleState(HeroCntrl heroCntrl) : base(TITLE) 
    {
        this.heroCntrl = heroCntrl;
    }

    public override void OnEnter() 
    {
        heroCntrl.inputDirection.x = 0.0f;
    }

    public override void OnExit() { }

    public override string OnUpdate(InputCntrl inputKeys, float dt) 
    {
        string state = null;

        if (inputKeys.RightKey) state = PlayerMoveState.TITLE;

        if (inputKeys.LeftKey) state = PlayerMoveState.TITLE;

        if (inputKeys.UpKey) state = PlayerJumpState.TITLE;

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

