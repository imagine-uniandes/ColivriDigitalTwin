using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
}
