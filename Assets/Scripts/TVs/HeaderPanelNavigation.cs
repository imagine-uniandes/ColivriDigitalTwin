using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HeaderPanelNavigation : MonoBehaviour
{
    public Button[] headerButtons;
    private int selectedIndex = 0;
    public GameObject homePanel;
    [SerializeField] private GameObject[] panelsToHide;

    private bool isHeldM = false;
    private float holdTimerM = 0f;
    private float holdDurationM = 3f; // 3 seconds hold duration for going back to home

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            isHeldM = true;
            holdTimerM += Time.deltaTime;
            if (holdTimerM >= holdDurationM)
            {
                GoBackToHome();
            }
        }
        else
        {
            if (isHeldM)
            {
                isHeldM = false;
                if (holdTimerM < holdDurationM)
                {
                    selectedIndex = (selectedIndex + 1) % headerButtons.Length;
                    headerButtons[selectedIndex].Select();
                }
                holdTimerM = 0f;
            }
        }
    }

    private void GoBackToHome()
    {
        if (homePanel != null)
        {
            homePanel.SetActive(true);
            SelectDefaultButton(homePanel);
            // Hide all other panels
            foreach (var panel in panelsToHide)
            {
                if (panel != null)
                    panel.SetActive(false);
            }
        }
    }

    private void SelectDefaultButton(GameObject targetPanel)
    {
        // Get the first selectable element in the panel
        Selectable[] selectables = targetPanel.GetComponentsInChildren<Selectable>();
        if (selectables.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
        }
    }
}
