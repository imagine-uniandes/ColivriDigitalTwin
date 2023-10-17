using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{
    public GameObject coverageView;
    public GameObject securityCameraGroup;
    public float rotationSpeed = 5f;
    public float movementSpeed = 1f;
    private Transform selectedGX;
    private int selectedIndex = -1;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
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
        ActivateLastGX();
    }

    private void OnDisable()
    {
        if (coverageView != null)
        {
            coverageView.SetActive(false);
        }
    }

    private void Update()
    {
        if (selectedGX == null)
        {
            return;
        }

        if (isRotatingLR && canMove)
        {
            float step = rotationSpeed * Time.deltaTime * (rotationDirectionInvertedLR ? -1f : 1f);
            currentRotationY += step;
            selectedGX.rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        }
        else if (isRotatingUD && canMove)
        {
            float step = rotationSpeed * Time.deltaTime * (rotationDirectionInvertedUD ? -1f : 1f);
            currentRotationX += step;
            selectedGX.rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
        }
        else if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
            {
                Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * movementSpeed * Time.deltaTime;
                selectedGX.Translate(movement);
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

private void ActivateLastGX()
    {
        DeselectGX();
        if (securityCameraGroup != null && securityCameraGroup.transform.childCount > 0)
        {
            int lastGXIndex = securityCameraGroup.transform.childCount - 1;
            selectedGX = securityCameraGroup.transform.GetChild(lastGXIndex);
            if (selectedGX != null)
            {
                selectedGX.gameObject.SetActive(true);
                selectedIndex = lastGXIndex;
                initialPosition = selectedGX.position;
                initialRotation = selectedGX.rotation;
                currentRotationY = selectedGX.eulerAngles.y;
                currentRotationX = selectedGX.eulerAngles.x;
                canMove = true;
            }
        }
    }

    private void SwitchToPreviousSecurityCamera()
    {
        int totalCameras = securityCameraGroup.transform.childCount;
        int previousIndex = (selectedIndex - 1 + totalCameras) % totalCameras;
        SelectSecurityCamera(previousIndex);
    }

    private void SwitchToNextSecurityCamera()
    {
        int totalCameras = securityCameraGroup.transform.childCount;
        int nextIndex = (selectedIndex + 1) % totalCameras;
        SelectSecurityCamera(nextIndex);
    }

    private void SelectSecurityCamera(int index)
    {
        DeselectGX();
        selectedGX = securityCameraGroup.transform.GetChild(index);
        Camera selectedCamera = selectedGX.GetComponent<Camera>();
        if (selectedCamera != null)
        {
            selectedCamera.enabled = true;
        }
        selectedIndex = index;
        initialPosition = selectedGX.position;
        initialRotation = selectedGX.rotation;
        currentRotationY = selectedGX.eulerAngles.y;
        currentRotationX = selectedGX.eulerAngles.x;
        canMove = true;
    }

    private void DeselectGX()
    {
        if (selectedGX != null)
        {
            Camera selectedCamera = selectedGX.GetComponent<Camera>();
            if (selectedCamera != null)
            {
                selectedCamera.enabled = false;
            }
            selectedGX.gameObject.SetActive(false);
            selectedGX = null;
            selectedIndex = -1;
            isRotatingLR = false;
            isRotatingUD = false;
        }
    }
}