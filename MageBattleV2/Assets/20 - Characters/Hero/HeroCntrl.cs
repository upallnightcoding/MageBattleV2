using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroCntrl : MonoBehaviour
{
    [SerializeField] private Transform spellCastPoint;
    [SerializeField] private SpellSO spell;

    private float playerSpeed = 4.0f;
    private Vector2 playerMove;
    private CharacterController charCntrl;
    private Animator animator;
    private Vector3 moveDirection;
    private float maximumSpeed = 5.0f;
    private float rotationSpeed = 400.0f;

    private SpellCasterCntrl spellCaster = null;

    private FiniteStateMachine fsm = null;

    private bool castSpellRequest = false;

    private float timeBetweenCast = 0.0f;

    // Start is called before the first frame update
    private void Start()
    {
        charCntrl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        fsm = new FiniteStateMachine();
        spellCaster = new SpellCasterCntrl();
        spellCaster.Set(spell);
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerMovement(Time.deltaTime);

        CastSpell(Time.deltaTime);
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

    private void PlayerMovement(float dt)
    {
        moveDirection.x = playerMove.x; // Horizontal
        moveDirection.y = 0.0f;
        moveDirection.z = playerMove.y; // Vertical

        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);

        animator.SetFloat("Speed", inputMagnitude, 0.05f, dt);

        moveDirection.Normalize();

        if (moveDirection != Vector3.zero)
        {
            Vector3 velocity = inputMagnitude * maximumSpeed * moveDirection;

            charCntrl.Move(velocity * dt);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * dt);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>();
    }  

    public void OnFire(InputAction.CallbackContext context)
    {
        castSpellRequest = true;
        Debug.Log("OnFire ...");
    }
}
