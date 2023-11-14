using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class CameraController : MonoBehaviour
{
    public GameObject mainCameraGroup;
    public GameObject buttonsContainer; // Use a single GameObject to hold all buttons
    public Button button1;
    public Button button2;
    public float rotationSpeed = 5f; // Speed of rotation in degrees per second
    public float movementSpeed = 3.5f; // Speed of movement
    public Button[] arrowButtons; // Array of arrow buttons
    private Transform selectedGX;
    private int selectedIndex = -1; // Currently selected GX index
    private Vector3 initialPosition; // Initial position of the selected GX
    private Quaternion initialRotation; // Initial rotation of the selected GX
    private float currentRotationY = 0f; // Current rotation angle for left-right rotation
    private float currentRotationX = 0f; // Current rotation angle for top-down rotation
    private bool canMove = false; // Flag to allow movement when a button is clicked
    public float fadeDuration = 0.5f; // Duration of the fade-in effect
    private float fadeStartTime; // Start time of the fade-in effect
    private bool isFading = false; // Flag to track if fading is in progress
    private Camera[] cameras; // Array of cameras to adjust fading
    private PlayerInput playerInput;
    private ButtonNavigator buttonNavigator;
    private InputAction rotationAction;
    private InputAction zoomAction;
    private bool returnPressedOnce = false;

    private void OnEnable()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found. Please attach the PlayerInput component to this GameObject.");
        }
        rotationAction = playerInput.actions["Rotation"];
        zoomAction = playerInput.actions["Zoom"];

        buttonNavigator = GetComponentInParent<ButtonNavigator>();
        if (buttonNavigator == null)
            Debug.LogError("ButtonNavigator script not found in the parent GameObject.");
        buttonNavigator.SetButtons(buttonsContainer.GetComponentsInChildren<Button>());

        RestoreInitialPositionAndRotation();
        StartFadeIn(selectedGX.gameObject);
    }

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
        buttonNavigator.NavigateButtons(rotationAction.ReadValue<Vector3>().y);

        if (selectedGX == null)
        {
            // No GX is selected, return
            return;
        }

        if (isFading)
        {
            HandleFading();
        }

        if (canMove)
        {
            // Move GX with arrow keys only when the keys are pressed
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * movementSpeed * Time.deltaTime;
                selectedGX.Translate(movement);
            }

            // Rotate with joystick
            float Rx = rotationAction.ReadValue<Vector3>().x * rotationSpeed * Time.deltaTime;
            float Ry = rotationAction.ReadValue<Vector3>().y * rotationSpeed * Time.deltaTime;
            float Rz = rotationAction.ReadValue<Vector3>().z * rotationSpeed * Time.deltaTime;
            float zoom = zoomAction.ReadValue<float>() * movementSpeed * Time.deltaTime;

            // Rotate the camera based on joystick input
            selectedGX.Rotate(Rx, -Ry, -Rz);

            // Zoom in/out with the Z axis
            Vector3 move = new Vector3(0, -zoom, 0);
            mainCameraGroup.transform.Translate(move);

            // Go back to the selection menu
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!returnPressedOnce)
                {
                    returnPressedOnce = true;
                }
                else
                {
                    RestoreInitialPositionAndRotation();
                    StartFadeIn(selectedGX.gameObject);
                    returnPressedOnce = false;
                }
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
            StartFadeIn(selectedGX.gameObject);
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
        StartFadeIn(selectedGX.gameObject);
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

    private void StartFadeIn(GameObject group)
    {
        // Get all the cameras within this GameObject
        cameras = group.GetComponentsInChildren<Camera>();

        // Initialize the cameras' far properties to 0
        foreach (Camera camera in cameras)
        {
            camera.farClipPlane = 0f;
        }
        fadeStartTime = Time.time;
        isFading = true;
    }

    private void HandleFading()
    {
        float elapsedTime = Time.time - fadeStartTime;
        float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

        // Update the cameras' far properties to create the fade-in effect
        foreach (Camera camera in cameras)
        {
            camera.farClipPlane = Mathf.Lerp(0f, 30f, alpha);
        }

        if (alpha >= 1f)
        {
            // Stop the fading effect when the fade is complete
            isFading = false;
        }
    }
}
