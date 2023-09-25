using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public GameObject mainCameraGroup;
    public GameObject buttonsContainer;
    public float rotationSpeed = 5f; // Speed of rotation in degrees per second
    public float movementSpeed = 1f; // Speed of movement
    private Transform selectedGX;
    private int selectedIndex = -1; // Currently selected GX index
    private bool isRotatingLR = false; // Flag to track left-right rotation state
    private bool isRotatingUD = false; // Flag to track top-down rotation state
    private bool rotationDirectionInvertedLR = false; // Flag to track if rotation direction is inverted for left-right rotation
    private bool rotationDirectionInvertedUD = false; // Flag to track if rotation direction is inverted for top-down rotation
    private float currentRotationY = 0f; // Current rotation angle for left-right rotation
    private float currentRotationX = 0f; // Current rotation angle for top-down rotation

    private void Start()
    {
        // Get all buttons within the buttonsContainer
        Button[] buttons = buttonsContainer.GetComponentsInChildren<Button>();

        // Attach button click listeners
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => ActivateGX(index));
        }
    }

    private void Update()
    {
        if (selectedGX == null)
        {
            // No GX is selected, return
            return;
        }

        if (isRotatingLR)
        {
            // Rotate left-right (yaw)
            float step = rotationSpeed * Time.deltaTime * (rotationDirectionInvertedLR ? -1f : 1f);
            currentRotationY += step;
            selectedGX.rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        }
        else if (isRotatingUD)
        {
            // Rotate top-down (pitch)
            float step = rotationSpeed * Time.deltaTime * (rotationDirectionInvertedUD ? -1f : 1f);
            currentRotationX += step;
            selectedGX.rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        }
        else
        {
            // Move GX with arrow keys only when the keys are pressed
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * movementSpeed * Time.deltaTime;
                selectedGX.Translate(movement);
            }

            // Go back to the selection menu with 'b'
            if (Input.GetKeyDown(KeyCode.B))
            {
                DeselectGX();
            }
        }

        // Check for 'r' and 'u' key presses to toggle rotation direction and reset inversion flags
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRotatingLR = true;
            rotationDirectionInvertedLR = !rotationDirectionInvertedLR; // Invert rotation direction
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            isRotatingUD = true;
            rotationDirectionInvertedUD = !rotationDirectionInvertedUD; // Invert rotation direction
        }

        // Check for 'r' and 'u' key releases to stop rotation
        if (Input.GetKeyUp(KeyCode.R))
        {
            isRotatingLR = false;
        }

        if (Input.GetKeyUp(KeyCode.U))
        {
            isRotatingUD = false;
        }
    }

    private void ActivateGX(int index)
    {
        // Check if the index is within valid bounds
        if (index >= 0 && index < mainCameraGroup.transform.childCount)
        {
            // Deselect the current GX if one is selected
            DeselectGX();

            // Activate the selected GX
            selectedGX = mainCameraGroup.transform.GetChild(index);
            selectedGX.gameObject.SetActive(true);
            selectedIndex = index;

            // Update the current rotation angles
            currentRotationY = selectedGX.eulerAngles.y;
            currentRotationX = selectedGX.eulerAngles.x;
        }
    }

    private void DeselectGX()
    {
        // Deselect the currently selected GX
        if (selectedGX != null)
        {
            selectedGX.gameObject.SetActive(false);
            selectedGX = null;
            selectedIndex = -1;
            isRotatingLR = false;
            isRotatingUD = false;
        }
    }
}
