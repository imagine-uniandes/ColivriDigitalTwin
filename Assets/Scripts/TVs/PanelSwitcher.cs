using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelSwitcher : MonoBehaviour
{
    [SerializeField] private Button buttonToEnablePanel;
    [SerializeField] private GameObject panelToEnable;
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject[] panelsToHide;

    // Variable to control whether going back to the home panel is allowed
    private bool canGoBackToHome = true;

    private void Start()
    {
        // Attach a click event listener to the button
        buttonToEnablePanel.onClick.AddListener(EnablePanelAndHideOthers);
    }

    private void EnablePanelAndHideOthers()
    {
        // Enable the target panel
        if (panelToEnable != null)
        {
            panelToEnable.SetActive(true);

            // Focus on a selectable element in the panel
            SelectDefaultButton(panelToEnable);
        }

        // Hide all other panels
        foreach (var panel in panelsToHide)
        {
            if (panel != null)
                panel.SetActive(false);
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

    private void Update()
    {
        // Check if the M key is pressed to go back to the home panel
        if (Input.GetKeyDown(KeyCode.M) && canGoBackToHome)
        {
            GoBackToHomePanel();
        }
    }

    private void GoBackToHomePanel()
    {
        // Hide the current active panel (if any)
        if (panelToEnable != null)
        {
            panelToEnable.SetActive(false);
        }

        // Show the home panel
        if (homePanel != null)
        {
            homePanel.SetActive(true);

            // Focus on a selectable element in the home panel
            SelectDefaultButton(homePanel);
        }
    }

    // Method to set whether going back to the home panel is allowed
    public void SetCanGoBackToHome(bool value)
    {
        canGoBackToHome = value;
    }
}
