using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneDCntrl : MonoBehaviour
{
    private float playerSpeed = 4.0f;
    private Vector2 playerMove;
    private CharacterController charCntrl;
    private Animator animator;
    private Vector3 moveDirection;
    private float maximumSpeed = 5.0f;
    private float rotationSpeed = 400.0f;

    // Start is called before the first frame update
    void Start()
    {
        charCntrl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection.x = playerMove.x; // Horizontal
        moveDirection.y = 0.0f;
        moveDirection.z = playerMove.y; // Vertical

        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);

        animator.SetFloat("Speed", inputMagnitude, 0.05f, Time.deltaTime);

        float speed = inputMagnitude * maximumSpeed;
        moveDirection.Normalize();

       //Vector3 velocity = moveDirection * speed;

        //charCntrl.Move(velocity * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            Vector3 velocity = moveDirection * speed;

            charCntrl.Move(velocity * Time.deltaTime);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnCommands(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>();
    }
}
