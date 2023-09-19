using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public GameObject mainCameraGroup; // Reference to the MainCameraGroup
    public GameObject buttonsContainer; // Reference to the GameObject containing the buttons

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

    private void ActivateGX(int index)
    {
        // Check if the index is within valid bounds
        if (index >= 0 && index < mainCameraGroup.transform.childCount)
        {
            // Disable all GXs in the group
            foreach (Transform gxTransform in mainCameraGroup.transform)
            {
                gxTransform.gameObject.SetActive(false);
            }

            // Activate the selected GX
            Transform selectedGX = mainCameraGroup.transform.GetChild(index);
            selectedGX.gameObject.SetActive(true);
        }
    }
}
