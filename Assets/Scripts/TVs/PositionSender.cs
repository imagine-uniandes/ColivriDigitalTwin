using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PositionSender : MonoBehaviour
{
    private string serverURL;
    private string computerID;

    // TODO: instead do it on enable
    private void Start()
    {
        computerID = gameObject.name;
        serverURL = "http://18.188.1.225:8080/data/" + computerID;
        SendPositionData();
    }

    private void SendPositionData()
    {
        Vector3 position = transform.position;
        PositionData data = new PositionData(position.x, position.y, position.z);

        string json = JsonUtility.ToJson(data);
        StartCoroutine(PostPosition(json));
    }

    private IEnumerator PostPosition(string json)
    {
        UnityWebRequest www = UnityWebRequest.PostWwwForm(serverURL, json);
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Position data sent successfully!");
        }
    }

    [System.Serializable]
    private class PositionData
    {
        public float x;
        public float y;
        public float z;

        public PositionData(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
