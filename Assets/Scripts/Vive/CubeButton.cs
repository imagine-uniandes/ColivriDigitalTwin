using UnityEngine;
using UnityEngine.Networking;
using System.Collections; 

public class CubeButton : MonoBehaviour
{
    private int clickCount = 0;
    private int keyPressCount = 0;

    private string ledServerURL = "http://172.24.100.110:8080/data/led";

    //0 = estado actual
    //1 = estado futuro
    private void OnMouseDown()
    {
        StartCoroutine(GetLedStateAndUpdateClicks());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(GetLedStateAndUpdateClicks());
        }
    }

    private IEnumerator GetLedStateAndUpdateClicks()
    {
        UnityWebRequest request = UnityWebRequest.Get(ledServerURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            int ledState = int.Parse(request.downloadHandler.text);

            // Si el estado del LED es igual al número de clics local, realiza el siguiente clic
            if (ledState == clickCount)// 0 - 0 (cambio a futuro) / 1 - 1 (devolver al presente)
            {
                // Invierte el estado local del clic
                clickCount = (clickCount == 0) ? 1 : 0; // 0 -> 1 (cambio a futuro)/ 1 -> 0 (devolver al presente)
            }
            PerformNextClick();// 1 (cambio a futuro)/ 0 (devolver al presente)
        }
        else
        {
            Debug.LogError("Error fetching LED state: " + request.error);
        }
    }

    private void PerformNextClick()
    {
        // Realiza la lógica correspondiente al siguiente clic
        if (clickCount == 1)
        {
            FirstClick();
        }
        else if (clickCount == 0)
        {
            SecondClick(); 
        }

        // Envía el estado actualizado del LED al servidor
        SendLedState(clickCount);
    }



    private void FirstClick()
    {
        // Lógica para el primer clic aquí
        Debug.Log("Primer clic en " + gameObject.name);
        GameObjectManager.instance.StartMovementOnObjects();
    }

    private void SecondClick()
    {
        // Lógica para el segundo clic aquí
        Debug.Log("Segundo clic en " + gameObject.name);
        GameObjectManager.instance.StartMovementOnObjectsRevert();
    }

    private void SendLedState(int state)
    {
        // Envía el estado actualizado del LED al servidor
        StartCoroutine(SendLedStateCoroutine(state));
    }

    private IEnumerator SendLedStateCoroutine(int state)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(state.ToString());

        UnityWebRequest request = new UnityWebRequest(ledServerURL, "PUT");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending LED state: " + request.error);
        }
    }
}
