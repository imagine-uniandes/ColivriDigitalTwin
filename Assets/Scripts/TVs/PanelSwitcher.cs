using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{
    [SerializeField] private Button buttonToEnablePanel;
    [SerializeField] private GameObject panelToEnable;
    [SerializeField] private GameObject[] panelsToHide;

    private void Start()
    {
        // Attach a click event listener to the button
        buttonToEnablePanel.onClick.AddListener(EnablePanelAndHideOthers);
    }

    private void EnablePanelAndHideOthers()
    {
        // Enable the target panel
        if (panelToEnable != null)
            panelToEnable.SetActive(true);

        // Hide all other panels
        foreach (var panel in panelsToHide)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }
}
