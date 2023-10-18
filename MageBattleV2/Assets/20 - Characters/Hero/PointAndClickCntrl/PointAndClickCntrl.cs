using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointAndClickCntrl : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayers;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private CharacterController charCntrl;
    private Vector2 movement;
    private Vector3 direction = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        charCntrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Left Button Clicked ...");
            direction = ClickToMove(Mouse.current.position.ReadValue());
        }

        charCntrl.Move(direction * moveSpeed * Time.deltaTime);

        Quaternion directionRotation = Quaternion.LookRotation(direction);
        Quaternion rotation =
            Quaternion.RotateTowards(transform.rotation, directionRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = rotation;
    }

    private Vector3 ClickToMove(Vector2 targetMousePosition)
    {
        Vector3 direction = Vector3.zero;
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(targetMousePosition), out hit, 100))
        {
            direction = (hit.point - transform.position).normalized;
        }

        return (direction);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log($"Mouse: {context.ReadValue<Vector2>()}");
            movement = context.ReadValue<Vector2>();
        }
    }
}
