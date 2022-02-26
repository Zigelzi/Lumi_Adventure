using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [Tooltip("Adjust the movement speed of the playerå")]
    [SerializeField] float movementSpeed = 20f;

    [Tooltip("Adjust the responsiveness of the movement")]
    [SerializeField] float maxAcceleration = 1f;

    PlayerInputActions playerInputActions;
    InputAction movement;
    Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerRb = GetComponent<Rigidbody>();
        movement = playerInputActions.Player.Movement;
        movement.Enable();
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        movement.Disable();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 movementInput = movement.ReadValue<Vector2>();

        Vector3 currentVelocity = playerRb.velocity;
        Vector3 newVelocity = new Vector3();
        Vector3 desiredVelocity = new Vector3(movementInput.x, 0f, movementInput.y) * movementSpeed;

        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        if (movementInput.magnitude != 0)
        {
            newVelocity.x = Mathf.MoveTowards(currentVelocity.x, desiredVelocity.x, maxSpeedChange);
            newVelocity.z = Mathf.MoveTowards(currentVelocity.z, desiredVelocity.z, maxSpeedChange);

            playerRb.velocity = newVelocity;
        }
    }
}
