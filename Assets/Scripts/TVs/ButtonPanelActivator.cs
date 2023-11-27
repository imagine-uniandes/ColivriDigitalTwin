using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPanelActivator : MonoBehaviour
{
    public GameObject buttonGroup;
    public GameObject panelGroup;

    private void Start()
    {
        // Attach select event listeners to the buttons
        Button[] buttons = buttonGroup.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            EventTrigger trigger = buttons[i].gameObject.GetComponent<EventTrigger>();

            // Ensure EventTrigger is attached
            if (trigger == null)
            {
                trigger = buttons[i].gameObject.AddComponent<EventTrigger>();
            }

            // Attach the OnSelect event
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
            entry.callback.AddListener((data) => { ActivatePanel(index); });
            trigger.triggers.Add(entry);
        }
    }

    private void ActivatePanel(int panelIndex)
    {
        // Ensure that the panelIndex is within the valid range
        if (panelIndex >= 0 && panelIndex < panelGroup.transform.childCount)
        {
            // Activate the corresponding panel
            for (int i = 0; i < panelGroup.transform.childCount; i++)
            {
                panelGroup.transform.GetChild(i).gameObject.SetActive(i == panelIndex);
            }

            // Focus on a selectable element in the activated panel
            SelectDefaultButton(panelGroup.transform.GetChild(panelIndex).gameObject);
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
