using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class CubeButtonForTVs : MonoBehaviour
{
    private int clickCount = 0;
    public Button triggerButton;

    private string ledServerURL = "http://172.24.100.110:8080/data/led";
    private bool isCheckingState = false;

    private void Start()
    {
        triggerButton.onClick.AddListener(OnTriggerButtonClick);
        StartCoroutine(CheckLedStatePeriodically());
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

        // Send the updated LED state to the server
        SendLedState(clickCount);
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

    private void SendLedState(int state)
    {
        // Send the updated LED state to the server using a POST request
        StartCoroutine(SendLedStateCoroutine(state));
    }

    private IEnumerator SendLedStateCoroutine(int state)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(state.ToString());

        UnityWebRequest request = new UnityWebRequest(ledServerURL, "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending LED state: " + request.error);
        }
    }

    private IEnumerator CheckLedStatePeriodically()
    {
        while (true)
        {
            // Perform a GET request to check the LED state
            yield return StartCoroutine(GetLedStateAndUpdateClicks());

            // Wait for one second before checking again
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator GetLedStateAndUpdateClicks()
    {
        UnityWebRequest request = UnityWebRequest.Get(ledServerURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            int ledState = int.Parse(request.downloadHandler.text);

            // If the LED state has changed, update the local click count
            if (ledState != clickCount)
            {
                clickCount = ledState;

                // Perform logic for the next click based on the updated state
                if (clickCount == 1)
                {
                    FirstClick();
                }
                else if (clickCount == 0)
                {
                    SecondClick();
                }
            }
        }
        else
        {
            Debug.LogError("Error fetching LED state: " + request.error);
        }
    }
}
