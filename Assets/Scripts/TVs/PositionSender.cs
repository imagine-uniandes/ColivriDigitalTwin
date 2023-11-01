using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PositionSender : MonoBehaviour
{
    private string serverURL;
    private string computerID;
    public float updateInterval = 15f;
    private bool isApplicationPaused = false;

    private void OnEnable()
    {
        computerID = gameObject.name;
        serverURL = "http://172.24.100.110:8080/data/control-center-" + computerID;
        InvokeRepeating("SendPositionData", 0f, 15f);
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
        UnityWebRequest request = new UnityWebRequest(serverURL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error sending position data: " + request.error);
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
