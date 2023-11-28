using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class JoystickVisualizer : MonoBehaviour
{
    public Image arrowImage;
    public Image rotationImage;
    private PlayerInput playerInput;
    private InputAction rotationAction;
    public float rotationSpeed = 180f;

    private void OnEnable()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found. Please attach the PlayerInput component to this GameObject.");
        }
        rotationAction = playerInput.actions["Rotation"];
    }

    void Update()
    {
        // Arrow update
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // Check if there is joystick movement
        if (Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f)
        {
            // Calculate the angle based on joystick input
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

            // Rotate the arrow image
            RotateArrow(angle);

            // Enable the arrow image
            arrowImage.enabled = true;
        }
        else
        {
            // Disable the arrow image when there is no movement
            arrowImage.enabled = false;
        }

        // Rotation update
        float Rx = rotationAction.ReadValue<Vector3>().x * rotationSpeed * Time.deltaTime;
        float Ry = rotationAction.ReadValue<Vector3>().y * rotationSpeed * Time.deltaTime;
        float Rz = rotationAction.ReadValue<Vector3>().z * rotationSpeed * Time.deltaTime;

        // Check if there is joystick movement
        if (Mathf.Abs(Rx) > 0.1f || Mathf.Abs(Ry) > 0.1f || Mathf.Abs(Rz) > 0.1f)
        {
            // Calculate the angle based on joystick input
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

            // Rotate the rotation image
            if (rotationImage != null)
            {
                // Set the rotation of the arrow image
                rotationImage.rectTransform.rotation = Quaternion.Euler(Rx, Ry, Rz);
            }

            // Enable the rotation image
            rotationImage.enabled = true;
        }
        else
        {
            // Disable the arrow image when there is no movement
            rotationImage.enabled = false;
        }
    }

    void RotateArrow(float angle)
    {
        if (arrowImage != null)
        {
            // Set the rotation of the arrow image
            arrowImage.rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
