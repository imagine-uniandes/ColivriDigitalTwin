using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform mainCameraGroup;
    public GameObject buttonsContainer;

    private void Start()
    {
        // Get all buttons within the buttonsContainer
        Button[] buttons = buttonsContainer.GetComponentsInChildren<Button>();

        // Attach button click listeners
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => ActivateCameraInGroup(index));
        }
    }

    private void ActivateCameraInGroup(int index)
    {
        // Check if the index is within valid bounds
        if (index >= 0 && index < mainCameraGroup.childCount)
        {
            // Disable all cameras in the group
            foreach (Transform cameraTransform in mainCameraGroup)
            {
                cameraTransform.gameObject.SetActive(false);
            }

            // Activate the selected camera
            Transform selectedCamera = mainCameraGroup.GetChild(index);
            selectedCamera.gameObject.SetActive(true);
        }
    }
}
