using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MoveObjectFromEndpoint : MonoBehaviour
{
    public string endpointURL = "http://172.24.100.110:8080/data/metapro";
    public GameObject objectToMove;
    private Vector3 targetPosition;

    private void Start()
    {
        InvokeRepeating("GetDataAndUpdatePosition", 0f, 2f);
    }

    private void GetDataAndUpdatePosition()
    {
        StartCoroutine(FetchData());
    }

    private IEnumerator FetchData()
    {
        UnityWebRequest www = UnityWebRequest.Get(endpointURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            Data data = JsonUtility.FromJson<Data>(json);
            if (data != null)
            {
                targetPosition = new Vector3(data.x, data.y, data.z);
                MoveObjectToTarget();
            }
        }
    }

    private void MoveObjectToTarget()
    {
        if (objectToMove != null)
        {
            objectToMove.transform.position = targetPosition;
        }
    }

    [System.Serializable]
    public class Data
    {
        public float x;
        public float y;
        public float z;
    }
}
