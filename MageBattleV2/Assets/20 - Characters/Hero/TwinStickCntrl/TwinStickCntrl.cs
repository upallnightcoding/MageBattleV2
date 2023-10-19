using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TwinStickCntrl : MonoBehaviour
{
    [SerializeField] private float playerSpeed;

    private Vector2 playerMove;
    private Vector2 playerLook;
    private Vector3 direction;
    private Vector3 rotationTarget;
    private CharacterController charCntrl;

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        playerLook = context.ReadValue<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        charCntrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AimPlayer();
    }

    private void AimPlayer()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(playerLook);

        if (Physics.Raycast(ray, out hit))
        {
            rotationTarget = hit.point;
        }

        Vector3 aim = rotationTarget - transform.position;
        aim.y = 0.0f;
        Quaternion rotation = Quaternion.LookRotation(aim);

        Vector3 aimDirection = new Vector3(rotationTarget.x, 0.0f, rotationTarget.z);

        if (aimDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5.0f);
        }
    }

    private void MovePlayer()
    {
        direction.x = playerMove.x;
        direction.y = 0.0f;
        direction.z = playerMove.y;

        if (direction != Vector3.zero)
        {
            charCntrl.Move(direction * playerSpeed * Time.deltaTime);
        }
    }
}
