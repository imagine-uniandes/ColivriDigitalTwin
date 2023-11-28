using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ButtonNavigator : MonoBehaviour
{
    private Button[] buttons;
    private int selectedIndex = 0;
    private float lastRotationTime;

    public float rotationThreshold = 0.25f;
    public float rotationCooldown = 0.25f;

    public void SetButtons(Button[] newButtons)
    {
        buttons = newButtons;
    }

    public void NavigateButtons(float rotation)
    {
        if (buttons == null || buttons.Length == 0)
        {
            Debug.LogError("No buttons assigned to ButtonNavigator.");
            return;
        }

        if (Mathf.Abs(rotation) > rotationThreshold && Time.time - lastRotationTime >= rotationCooldown)
        {
            // Move to the next or previous button based on the rotation direction
            if (rotation > rotationThreshold)
            {
                MoveToNextButton();
            }
            else if (rotation < -rotationThreshold)
            {
                MoveToPreviousButton();
            }

            lastRotationTime = Time.time;
        }
    }

    private void MoveToNextButton()
    {
        selectedIndex = (selectedIndex + 1) % buttons.Length;
        buttons[selectedIndex].Select();
    }

    private void MoveToPreviousButton()
    {
        selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
        buttons[selectedIndex].Select();
    }
}
