using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Camera control class
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private SettingsController settingsController;

    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 5.0f;
    [SerializeField] private float fastSpeed = 10.0f;
    [SerializeField] private float slowSpeed = 2.0f;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float lookUpLimit = 90.0f;

    private float rotationX = 0.0f;
    private float currentSpeed;

    void Start()
    {
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        // Gravity
        if (settingsController.gravityIsOn) HandleGravity();

        // Input
        bool isOverUI = EventSystem.current.IsPointerOverGameObject();

        if (Input.GetMouseButton(0) && !isOverUI)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // Camera rotation (mouse look)
        HandleCameraRotation();

        // Camera movement
        HandleCameraMovement();
    }

    private void HandleGravity()
    {
        transform.position += settingsController.mathController.gravityVelocityScaled;
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookUpLimit, lookUpLimit);

        transform.Rotate(Vector3.up, mouseX, Space.World);

        Vector3 currentRotation = transform.localEulerAngles;
        transform.localRotation = Quaternion.Euler(rotationX, currentRotation.y, 0);
    }

    private void HandleCameraMovement()
    {
        // Set movement speed based on key modifiers
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = fastSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            currentSpeed = slowSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        // Calculate movement direction
        Vector3 moveDirection = CalculateMoveDirection();

        // Apply vertical movement
        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection.y += 1;
        }
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            moveDirection.y -= 1;
        }

        // Apply movement
        transform.position += moveDirection * currentSpeed * Time.unscaledDeltaTime;
    }

    private Vector3 CalculateMoveDirection()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;

        // Normalize movement vector to prevent diagonal movement being faster
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        return moveDirection;
    }
}