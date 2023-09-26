using UnityEngine;
using System.Collections;

public class ActivateAllDisplays : MonoBehaviour
{
    [SerializeField] private GameObject[] componentsToActivate;

    void Start()
    {
        foreach (GameObject component in componentsToActivate)
        {
            if (component != null)
            {
                component.SetActive(true);
            }
        }

        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
}
