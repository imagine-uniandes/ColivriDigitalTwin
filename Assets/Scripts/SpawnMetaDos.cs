using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class MetaDosCameraPositionGet
{
    public float x;
    public float y;
    public float z;
}

public class SpawnMetaDos : MonoBehaviour
{
    public GameObject metaPro;
    private string serverURLMetaPro = "http://18.188.1.225:8080/data/meta2";

    void Start()
    {
        StartCoroutine(UpdateMetaProPosition());
    }

    IEnumerator UpdateMetaProPosition()
    {
        while (true)
        {
            UnityWebRequest request = UnityWebRequest.Get(serverURLMetaPro);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                MetaProCameraPositionGet position = JsonUtility.FromJson<MetaProCameraPositionGet>(json);

                float xPos = position.x;
                float yPos = position.y;
                float zPos = position.z;

                if (xPos == 1000f && yPos == 1000f && zPos == 1000f)
                {
                    // La posici�n es (1000, 1000, 1000), desactiva el objeto
                    metaPro.SetActive(false);
                }
                else
                {
                    // La posici�n no es (1000, 1000, 1000), activa el objeto y establece su posici�n
                    metaPro.SetActive(true);
                    metaPro.transform.position = new Vector3(xPos, yPos, zPos);
                }
            }
            else
            {
                Debug.LogError("Error fetching camera position: " + request.error);
            }

            // Espera x segundos antes de la pr�xima actualizaci�n
            yield return new WaitForSeconds(3f);
        }
    }
}