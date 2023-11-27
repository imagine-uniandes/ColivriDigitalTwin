using UnityEngine;
using UnityEngine.UI;

public class JoystickVisualizer : MonoBehaviour
{
    public Image arrowImage;
    public float rotationSpeed = 180f;

    void Update()
    {
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
