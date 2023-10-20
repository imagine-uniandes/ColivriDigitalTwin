using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class CameraPosition
{
    public float x;
    public float y;
    public float z;
}

public class CameraPositionSender : MonoBehaviour
{
    private string serverURL = "http://18.188.1.225:8080/data/meta1";
    public float updateInterval = 3f; // Intervalo de actualizaci�n en segundos

    private bool isApplicationPaused = false;

    private void Start()
    {
        // Inicia la actualizaci�n peri�dica
        InvokeRepeating("SendCameraPosition", 0f, updateInterval);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        isApplicationPaused = pauseStatus;

        if (isApplicationPaused)
        {
            // La aplicaci�n est� en pausa o en segundo plano
            // Env�a un JSON con posici�n nula al servidor
            CameraPosition nullPosition = new CameraPosition
            {
                x = 0f,
                y = 0f,
                z = 0f
            };
            string nullPositionJSON = JsonUtility.ToJson(nullPosition);

            StartCoroutine(SendPositionToServer(nullPositionJSON));
        }
    }

    private void OnApplicationQuit()
    {
        // La aplicaci�n se est� cerrando
        // Env�a un JSON con posici�n nula al servidor antes de cerrar la aplicaci�n
        CameraPosition nullPosition = new CameraPosition
        {
            x = 0f,
            y = 0f,
            z = 0f
        };
        string nullPositionJSON = JsonUtility.ToJson(nullPosition);

        StartCoroutine(SendPositionToServer(nullPositionJSON));
    }

    void SendCameraPosition()
    {
        if (!isApplicationPaused)
        {
            // La aplicaci�n no est� en pausa o en segundo plano
            // Obt�n la posici�n de la c�mara principal
            Vector3 cameraPosition = Camera.main.transform.position;

            // Crea un objeto CameraPosition y asigna la posici�n
            CameraPosition position = new CameraPosition
            {
                x = cameraPosition.x,
                y = cameraPosition.y,
                z = cameraPosition.z
            };

            // Convierte el objeto en JSON
            string json = JsonUtility.ToJson(position);

            // Env�a el JSON al servidor
            StartCoroutine(SendPositionToServer(json));
        }
    }

    IEnumerator SendPositionToServer(string json)
    {
        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error sending camera position: " + request.error);
        }
    }
}