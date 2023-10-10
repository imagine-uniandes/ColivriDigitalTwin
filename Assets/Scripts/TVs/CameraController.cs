using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public GameObject mainCameraGroup;
    public GameObject buttonsContainer; // Use a single GameObject to hold all buttons
    public Button button1;
    public Button button2;
    public float rotationSpeed = 5f; // Speed of rotation in degrees per second
    public float movementSpeed = 1f; // Speed of movement
    public Button[] arrowButtons; // Array of arrow buttons
    private Transform selectedGX;
    private int selectedIndex = -1; // Currently selected GX index
    private Vector3 initialPosition; // Initial position of the selected GX
    private Quaternion initialRotation; // Initial rotation of the selected GX
    private bool isRotatingLR = false; // Flag to track left-right rotation state
    private bool isRotatingUD = false; // Flag to track top-down rotation state
    private bool rotationDirectionInvertedLR = false; // Flag to track if rotation direction is inverted for left-right rotation
    private bool rotationDirectionInvertedUD = false; // Flag to track if rotation direction is inverted for top-down rotation
    private float currentRotationY = 0f; // Current rotation angle for left-right rotation
    private float currentRotationX = 0f; // Current rotation angle for top-down rotation
    private bool canMove = false; // Flag to allow movement when a button is clicked

    private void Start()
    {
        // Attach button click listeners
        Button[] buttons = buttonsContainer.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => ActivateMovement(index));
        }

        // Disable the arrow buttons initially
        DisableArrowButtons();
    }

    private void Update()
    {
        if (selectedGX == null)
        {
            // No GX is selected, return
            return;
        }

        if (isRotatingLR && canMove)
        {
            // Rotate left-right (yaw)
            float step = rotationSpeed * Time.deltaTime * (rotationDirectionInvertedLR ? -1f : 1f);
            currentRotationY += step;
            selectedGX.rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        }
        else if (isRotatingUD && canMove)
        {
            // Rotate top-down (pitch)
            float step = rotationSpeed * Time.deltaTime * (rotationDirectionInvertedUD ? -1f : 1f);
            currentRotationX += step;
            selectedGX.rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        }
        else if (canMove)
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
                RestoreInitialPositionAndRotation();
            }
        }

        // Check for 'r' and 'u' key presses to toggle rotation direction and reset inversion flags
        if (Input.GetKeyDown(KeyCode.R) && canMove)
        {
            isRotatingLR = true;
            rotationDirectionInvertedLR = !rotationDirectionInvertedLR; // Invert rotation direction
        }
        else if (Input.GetKeyDown(KeyCode.U) && canMove)
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

        // Check for '1' key press to activate the top view GX and disable others
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateLastGX();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DisableLastGX();
            Selectable[] selectables = buttonsContainer.GetComponentsInChildren<Selectable>();
            if (selectables.Length > 0)
            {
                EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
            }
        }
    }

    private void ActivateMovement(int index)
    {
        // Check if the index is within valid bounds
        if (index >= 0 && index < mainCameraGroup.transform.childCount)
        {
            // Deselect the current GX if one is selected
            DeselectGX();

            // Enable the arrow buttons for movement
            EnableArrowButtons();

            // Activate the selected GX
            selectedGX = mainCameraGroup.transform.GetChild(index);
            selectedGX.gameObject.SetActive(true);
            selectedIndex = index;

            // Store the initial position and rotation
            initialPosition = selectedGX.position;
            initialRotation = selectedGX.rotation;

            // Update the current rotation angles
            currentRotationY = selectedGX.eulerAngles.y;
            currentRotationX = selectedGX.eulerAngles.x;

            // Allow movement when a button is clicked
            canMove = true;

            // Disable the buttons within buttonsContainer when arrowButtons are enabled
            Button[] buttons = buttonsContainer.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
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

    private void RestoreInitialPositionAndRotation()
    {
        // Restore the initial position and rotation
        int lastGXIndex = mainCameraGroup.transform.childCount - 1;
        if (selectedGX != null && selectedGX != mainCameraGroup.transform.GetChild(lastGXIndex))
        {
            selectedGX.position = initialPosition;
            selectedGX.rotation = initialRotation;
            canMove = false; // Disallow movement when deselected

            // Enable the buttons within buttonsContainer when returning to the selection menu
            Button[] buttons = buttonsContainer.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.interactable = true;
            }

            DisableArrowButtons();
        }
    }

    private void EnableArrowButtons()
    {
        // Enable the arrow buttons for movement
        for (int i = 0; i < arrowButtons.Length; i++)
        {
            arrowButtons[i].interactable = true;
        }
        Selectable[] selectables = arrowButtons[0].GetComponentsInChildren<Selectable>();
        if (selectables.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
        }
    }

    private void DisableArrowButtons()
    {
        // Disable the arrow buttons for movement
        for (int i = 0; i < arrowButtons.Length; i++)
        {
            arrowButtons[i].interactable = false;
        }

        // Set the first button in buttonsContainer as the selected object
        Button[] buttons = buttonsContainer.GetComponentsInChildren<Button>();
        if (buttons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        }
    }

    private void ActivateLastGX()
    {
        // Deselect the current GX if one is selected
        DeselectGX();

        // Activate the last GX
        int lastGXIndex = mainCameraGroup.transform.childCount - 1;
        selectedGX = mainCameraGroup.transform.GetChild(lastGXIndex);
        selectedGX.gameObject.SetActive(true);
        selectedIndex = lastGXIndex;

        // Disable all buttons except button2
        Button[] buttons = buttonsContainer.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
        button1.interactable = false;
        button2.interactable = true;
    }

    private void DisableLastGX()
    {
        // Enable the buttons within buttonsContainer when returning to the selection menu
        Button[] buttons = buttonsContainer.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        button1.interactable = true;
        button2.interactable = false;

        DeselectGX();
        selectedIndex = 0;
        selectedGX = mainCameraGroup.transform.GetChild(selectedIndex);
        selectedGX.gameObject.SetActive(true);
    }
}
