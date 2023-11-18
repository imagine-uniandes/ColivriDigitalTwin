using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Quest28CameraData
{
    public float x;
    public float y;
    public float z;
    public float rotx;
    public float roty;
    public float rotz;
}

public class SpawnQuest28 : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float smoothTime = 1.0f; // Velocidad de interpolación
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private string serverURL = "http://172.24.100.110:8080/data/meta28";

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

                Quest28CameraData data = JsonUtility.FromJson<Quest28CameraData>(json);

                float xPos = data.x;
                float yPos = data.y;
                float zPos = data.z;
                float rotx = data.rotx;
                float roty = data.roty;
                float rotz = data.rotz;

                if (xPos == 1000f && yPos == 1000f && zPos == 1000f)
                {
                    // La posición es (1000, 1000, 1000), desactiva el objeto
                    objectToSpawn.SetActive(false);
                }
                else
                {
                    // La posición no es (1000, 1000, 1000), activa el objeto y establece la posición y rotación
                    objectToSpawn.SetActive(true);

                    // Establece la posición y rotación de destino
                    targetPosition = new Vector3(xPos, yPos, zPos);
                    targetRotation = Quaternion.Euler(rotx, roty, rotz);
                }
            }
            else
            {
                Debug.LogError("Error fetching camera data: " + request.error);
            }

            // Espera x segundos antes de la próxima actualización
            yield return new WaitForSeconds(2f);
        }
    }

    void Update()
    {
        // Interpola suavemente la posición y rotación del objeto hacia la posición y rotación de destino
        objectToSpawn.transform.position = Vector3.Lerp(objectToSpawn.transform.position, targetPosition, smoothTime * Time.deltaTime);
        objectToSpawn.transform.rotation = Quaternion.Slerp(objectToSpawn.transform.rotation, targetRotation, smoothTime * Time.deltaTime);
    }
}
