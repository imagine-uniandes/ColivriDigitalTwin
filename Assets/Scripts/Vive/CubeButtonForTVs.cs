using UnityEngine;
using UnityEngine.UI;

public class CubeButtonForTVs : MonoBehaviour
{
    private int clickCount = 0;
    public Button triggerButton;

    public void Start()
    {
        triggerButton.onClick.AddListener(OnTriggerButtonClick);
    }

    private void OnTriggerButtonClick()
    {
        // Increment the click count
        clickCount++;

        // Call different functions based on the click count
        if (clickCount == 1)
        {
            FirstClick();
        }
        else if (clickCount == 2)
        {
            SecondClick();
        }
    }

    private void FirstClick()
    {
        // Logic for the first click here
        Debug.Log("First click on " + gameObject.name);
        GameObjectManager.instance.StartMovementOnObjects();
    }

    private void SecondClick()
    {
        // Logic for the second click here
        Debug.Log("Second click on " + gameObject.name);
        GameObjectManager.instance.StartMovementOnObjectsRevert();

        // Reset the click count to allow more clicks
        clickCount = 0;
    }
}
