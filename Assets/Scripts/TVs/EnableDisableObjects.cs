using UnityEngine;
using UnityEngine.EventSystems;

public class EnableDisableObjects : MonoBehaviour
{
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    private void OnEnable()
    {
        EnableObjects(objectsToEnable);
        DisableObjects(objectsToDisable);
    }

    private void OnDisable()
    {
        DisableObjects(objectsToEnable);
        EnableObjects(objectsToDisable);
    }

    private void EnableObjects(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    private void DisableObjects(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
