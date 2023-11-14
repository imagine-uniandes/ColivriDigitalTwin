using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class SecurityCameraController : MonoBehaviour
{
    public GameObject coverageView;
    public GameObject securityCameraGroup;
    public float rotationSpeed = 5f;
    public float movementSpeed = 3.5f;
    private Camera[] cameras;
    private Transform selectedCameraParent;
    private int selectedIndex = -1;
    private bool isRotatingLR = false;
    private bool isRotatingUD = false;
    private bool rotationDirectionInvertedLR = false;
    private bool rotationDirectionInvertedUD = false;
    private float currentRotationY = 0f;
    private float currentRotationX = 0f;
    private bool canMove = false;
    private PlayerInput playerInput;
    private InputAction rotationAction;
    private InputAction zoomAction;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found. Please attach the PlayerInput component to this GameObject.");
        }
        rotationAction = playerInput.actions["Rotation"];
        zoomAction = playerInput.actions["Zoom"];
    }

    private void OnEnable()
    {
        if (coverageView != null)
        {
            coverageView.SetActive(true);
        }
        ActivateLastCamera();
    }

    private void OnDisable()
    {
        foreach (var camera in cameras)
        {
            camera.gameObject.SetActive(true);
        }

        if (coverageView != null)
        {
            coverageView.SetActive(false);
        }
    }

    private void Update()
    {
        if (cameras == null || cameras.Length == 0 || selectedCameraParent == null)
        {
            return;
        }

        if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * movementSpeed * Time.deltaTime;
                selectedCameraParent.Translate(movement);
            }

            // Rotate with joystick
            float Rx = rotationAction.ReadValue<Vector3>().x * rotationSpeed * Time.deltaTime;
            float Ry = rotationAction.ReadValue<Vector3>().y * rotationSpeed * Time.deltaTime;
            float Rz = rotationAction.ReadValue<Vector3>().z * rotationSpeed * Time.deltaTime;
            float zoom = zoomAction.ReadValue<float>() * movementSpeed * Time.deltaTime;

            // Rotate the camera based on joystick input
            selectedCameraParent.Rotate(Rx, -Ry, -Rz);

            // Zoom in/out with the Z axis
            Vector3 move = new Vector3(0, -zoom, 0);
            selectedCameraParent.transform.Translate(move);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SwitchToNextSecurityCamera();
        }
    }

    private void ActivateLastCamera()
    {
        if (securityCameraGroup != null && securityCameraGroup.transform.childCount > 0)
        {
            cameras = securityCameraGroup.GetComponentsInChildren<Camera>();
            if (cameras.Length > 0)
            {
                foreach (var camera in cameras)
                {
                    camera.gameObject.SetActive(false);
                }
                cameras[cameras.Length - 1].gameObject.SetActive(true);
                selectedCameraParent = cameras[cameras.Length - 1].transform.parent;
                selectedIndex = cameras.Length - 1;
                canMove = true;
            }
        }
    }

    private void SwitchToPreviousSecurityCamera()
    {
        if (cameras == null || cameras.Length == 0) return;
        cameras[selectedIndex].gameObject.SetActive(false);
        selectedIndex = (selectedIndex - 1 + cameras.Length) % cameras.Length;
        cameras[selectedIndex].gameObject.SetActive(true);
        selectedCameraParent = cameras[selectedIndex].transform.parent;
    }

    private void SwitchToNextSecurityCamera()
    {
        if (cameras == null || cameras.Length == 0) return;
        cameras[selectedIndex].gameObject.SetActive(false);
        selectedIndex = (selectedIndex + 1) % cameras.Length;
        cameras[selectedIndex].gameObject.SetActive(true);
        selectedCameraParent = cameras[selectedIndex].transform.parent;
    }
}
