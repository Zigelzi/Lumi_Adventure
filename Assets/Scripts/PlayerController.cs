using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Tooltip("Adjust the movement speed of the player")]
    [SerializeField] float movementSpeed = 20f;

    [Tooltip("Adjust what is the maximum speed that the character can move with")]
    [SerializeField] float maxMovementSpeed = 10f;

    [Tooltip("Adjust the responsiveness of the movement")]
    [SerializeField] float maxAcceleration = 1f;

    Camera playerCamera;

    PlayerInputActions playerInputActions;
    InputAction movement;
    InputAction look;
    Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerRb = GetComponent<Rigidbody>();
        movement = playerInputActions.Player.Movement;
        look = playerInputActions.Player.Look;

        playerCamera = Camera.main;
        
        movement.Enable();
        look.Enable();
    }

    void OnDestroy()
    {
        movement.Disable();
        look.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        SetPlayerVelocity();
        LimitToMaxMovementSpeed();
        LookAtMovementDirection();
    }

    void SetPlayerVelocity()
    {
        Vector2 movementInput = movement.ReadValue<Vector2>();
        Vector3 alignedXInput = movementInput.x * GetCameraRight();
        Vector3 alignedYInput = movementInput.y * GetCameraForward();

        Vector3 currentVelocity = playerRb.velocity;
        Vector3 newVelocity = new Vector3();
        Vector3 desiredVelocity = Vector3.zero;

        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        desiredVelocity += alignedXInput * movementSpeed;
        desiredVelocity += alignedYInput * movementSpeed;

        playerRb.AddForce(desiredVelocity, ForceMode.Impulse);

        //newVelocity.x = Mathf.MoveTowards(currentVelocity.x, desiredVelocity.x, maxSpeedChange);
        //newVelocity.z = Mathf.MoveTowards(currentVelocity.z, desiredVelocity.z, maxSpeedChange);

        //playerRb.velocity = newVelocity;
    }

    Vector3 GetCameraForward()
    {
        // Align the forward component of the camera to horizontal plane
        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0;

        return cameraForward.normalized;
    }

    Vector3 GetCameraRight()
    {
        // Align the forward component of the camera to horizontal plane
        Vector3 cameraRight = playerCamera.transform.right;
        cameraRight.y = 0;

        return cameraRight;
    }

    void LimitToMaxMovementSpeed()
    {
        Vector3 horizontalVelocity = playerRb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxMovementSpeed * maxMovementSpeed)
        {
            playerRb.velocity = horizontalVelocity.normalized * maxMovementSpeed + Vector3.up * playerRb.velocity.y;
        }
    }

    void LookAtMovementDirection()
    {
        Vector3 direction = playerRb.velocity;
        direction.y = 0;

        if (IsMoving())
        {
            playerRb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            // Prevent rotation if no input is given to reduce the floatiness
            playerRb.angularVelocity = Vector3.zero;
        }
    }

    bool IsMoving()
    {
        Vector3 direction = playerRb.velocity;
        direction.y = 0;
        if (movement.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            return true;
        }
        else 
        { 
            return false;
        }
    }
}
