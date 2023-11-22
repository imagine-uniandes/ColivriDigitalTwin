using UnityEngine;
using UnityEngine.EventSystems;

public class DisableObjectsForever : MonoBehaviour
{
    public GameObject[] objectsToDisable;
    public GameObject[] objectToEnableWhenDisable
    ;
    private void OnEnable()
    {
        DisableObjects(objectsToDisable);
    }

    private void OnDisable()
    {
        EnableObjects(objectToEnableWhenDisable);
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
