using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PositionSender : MonoBehaviour
{
    private string serverURL;
    public float updateInterval = 2f;
    private bool isApplicationPaused = false;

    private void OnEnable()
    {
        serverURL = "http://172.24.100.110:8080/data/control-center";
        InvokeRepeating("SendPositionData", 0f, updateInterval);
    }

    private void OnApplicationQuit()
    {
        ResetPositionData();
    }

    private void SendPositionData()
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        PositionData data = new PositionData(position.x, position.y, position.z, rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);

        string json = JsonUtility.ToJson(data);
        StartCoroutine(PostPosition(json));
    }

    private void ResetPositionData()
    {
        PositionData data = new PositionData(1000f, 1000f, 1000f, 0f, 0f, 0f);
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
        public float rotx;
        public float roty;
        public float rotz;

        public PositionData(float x, float y, float z, float rx, float ry, float rz)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.rotx = rx;
            this.roty = ry;
            this.rotz = rz;
        }
    }
}
