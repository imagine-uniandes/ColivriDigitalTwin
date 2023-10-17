using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{
    public GameObject coverageView;
    public GameObject securityCameraGroup;
    public float rotationSpeed = 5f;
    public float movementSpeed = 1f;
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

        if (isRotatingLR && canMove)
        {
            float step = rotationSpeed * Time.deltaTime * (rotationDirectionInvertedLR ? -1f : 1f);
            currentRotationY += step;
            selectedCameraParent.rotation = Quaternion.Euler(0f, currentRotationY, 0f);
        }
        else if (isRotatingUD && canMove)
        {
            float step = rotationSpeed * Time.deltaTime * (rotationDirectionInvertedUD ? -1f : 1f);
            currentRotationX += step;
            selectedCameraParent.rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        }
        else if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * movementSpeed * Time.deltaTime;
                selectedCameraParent.Translate(movement);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && canMove)
        {
            isRotatingLR = true;
            rotationDirectionInvertedLR = !rotationDirectionInvertedLR;
        }
        else if (Input.GetKeyDown(KeyCode.U) && canMove)
        {
            isRotatingUD = true;
            rotationDirectionInvertedUD = !rotationDirectionInvertedUD;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            isRotatingLR = false;
        }

        if (Input.GetKeyUp(KeyCode.U))
        {
            isRotatingUD = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToPreviousSecurityCamera();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
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
