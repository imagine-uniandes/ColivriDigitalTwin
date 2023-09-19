using UnityEngine;
using UnityEngine.UI;

public class HeaderPanelNavigation : MonoBehaviour
{
    public Button[] headerButtons;
    private int selectedIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            headerButtons[selectedIndex].Select();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            selectedIndex = Mathf.Min(headerButtons.Length - 1, selectedIndex + 1);
            headerButtons[selectedIndex].Select();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            headerButtons[selectedIndex].onClick.Invoke();
        }
    }
}
