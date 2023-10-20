using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TwinStickCntrl : MonoBehaviour
{
    private const string ANIMATION_TURN = "TURN";
    private const string ANIMATION_MOVE = "MOVE";

    [SerializeField] private float playerSpeed;

    private Vector2 playerMove;
    private Vector2 playerAim;
    private Vector3 moveDirection;
    private Vector3 rotationTarget;
    private Vector3 aimDirection;
    private CharacterController charCntrl;
    private Animator anim;

    Transform cam;
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;

    float forwardAmount;
    float turnAmount;
   

    // Start is called before the first frame update
    void Start()
    {
        charCntrl = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AimPlayer();
        AnimatePlayer();
    }

    private void AnimatePlayer()
    {

    }

    private void AimPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(playerAim);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            rotationTarget = hit.point;
        }

        Vector3 aim = rotationTarget - transform.position;
        aim.y = 0.0f;
        Quaternion rotation = Quaternion.LookRotation(aim);

        aimDirection.x = rotationTarget.x; ;
        aimDirection.y = 0.0f;
        aimDirection.z = rotationTarget.z;

        if (aimDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5.0f);
        }
    }

    private void MovePlayer()
    {
        moveDirection.x = playerMove.x; // Horizontal
        moveDirection.y = 0.0f;
        moveDirection.z = playerMove.y; // Vertical

        camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
        move = playerMove.y * camForward + playerMove.x * cam.right;
        move.Normalize();
        moveInput = move;

        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;
        forwardAmount = localMove.z;

        if (moveDirection != Vector3.zero)
        {
            charCntrl.Move(moveDirection * playerSpeed * Time.deltaTime);

            anim.SetFloat(ANIMATION_TURN, turnAmount, 0.1f, Time.deltaTime);
            anim.SetFloat(ANIMATION_MOVE, forwardAmount, 0.1f, Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        playerAim = context.ReadValue<Vector2>();
    }
}
