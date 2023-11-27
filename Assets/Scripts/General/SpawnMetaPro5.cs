using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class QuestPro5CameraData
{
    public float x;
    public float y;
    public float z;
    public float rotx;
    public float roty;
    public float rotz;
}

public class SpawnMetaPro5 : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float smoothTime = 1.0f; // Velocidad de interpolaci�n
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private string serverURL = "http://172.24.100.110:8080/data/metapro5";

    void Start()
    {
        StartCoroutine(UpdateObjectPosition());
    }

    IEnumerator UpdateObjectPosition()
    {
        while (true)
        {
            UnityWebRequest request = UnityWebRequest.Get(serverURL);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                QuestProCameraData data = JsonUtility.FromJson<QuestProCameraData>(json);

                float xPos = data.x;
                float yPos = data.y;
                float zPos = data.z;
                float rotx = data.rotx;
                float roty = data.roty;
                float rotz = data.rotz;

                if (xPos == 1000f && yPos == 1000f && zPos == 1000f)
                {
                    // La posici�n es (1000, 1000, 1000), desactiva el objeto
                    objectToSpawn.SetActive(false);
                }
                else
                {
                    // La posici�n no es (1000, 1000, 1000), activa el objeto y establece la posici�n y rotaci�n
                    objectToSpawn.SetActive(true);

                    // Establece la posici�n y rotaci�n de destino

                    targetPosition = new Vector3(xPos, yPos, zPos);
                    targetRotation = Quaternion.Euler(rotx, roty , rotz);

                }
            }
            else
            {
                Debug.LogError("Error fetching camera data: " + request.error);
            }

            // Espera x segundos antes de la pr�xima actualizaci�n
            yield return new WaitForSeconds(2f);
        }
    }

    void Update()
    {
        // Interpola suavemente la posici�n y rotaci�n del objeto hacia la posici�n y rotaci�n de destino
        objectToSpawn.transform.position = Vector3.Lerp(objectToSpawn.transform.position, targetPosition, smoothTime * Time.deltaTime);
        objectToSpawn.transform.rotation = Quaternion.Slerp(objectToSpawn.transform.rotation, targetRotation, smoothTime * Time.deltaTime);
    }
}

