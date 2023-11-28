using UnityEngine;
using UnityEngine.EventSystems;

public class EnableObjectsForever : MonoBehaviour
{
    public GameObject[] objectsToEnable;
    public GameObject[] objectToEnableWhenDisable;
    private void OnEnable()
    {
        EnableObjects(objectsToEnable);
    }

    private void OnDisable()
    {
        DisableObjects(objectToEnableWhenDisable);
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
