using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

[System.Serializable]
public class TableTrackingData
{
    public float x;
    public float y;
    public float angle;
}

public class TableTracking : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject objectTracked;
    private Vector3 startingPosition;
    private Vector3 startingRotation;
    private Vector3 rotationAmount;

    private string serverURL = "http://172.24.100.110:8080/data/mesa";

    private float startingX;
    private float startingY;
    private float startingR;
    private float firstR;

    private float prevX = 0;
    private float prevY = 0;
    private float prevR = 0;

    private bool b = true;

    void Start()
    {
        StartCoroutine(UpdateTablePosition());
    }

    IEnumerator UpdateTablePosition(){
        while(true){

            UnityWebRequest request = UnityWebRequest.Get(serverURL);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if(b){
                    string json = request.downloadHandler.text;

                    TableTrackingData data = JsonUtility.FromJson<TableTrackingData>(json);

                    startingX = data.x/1000;
                    startingY = data.y/1000;
                    startingR = data.angle;

                    b = false;

                    startingPosition = objectTracked.transform.localPosition;
                    startingRotation = objectTracked.transform.eulerAngles;
                    firstR = startingRotation.y;
                    Debug.Log(startingRotation);
                } else {

                    string json = request.downloadHandler.text;

                    TableTrackingData data = JsonUtility.FromJson<TableTrackingData>(json);

                    float currentX = data.x/1000;
                    float currentY = data.y/1000;
                    float currentR = data.angle;

                    float x = startingX - currentX;
                    float y = startingY - currentY;
                    float r = Math.Abs(startingR - currentR);


                    if(Math.Abs(x-prevX) < 0.001 || Math.Abs(y-prevY) < 0.001){
                        prevX = x;
                        prevY = y;
                    } else {
                        objectTracked.transform.localPosition = new Vector3(startingPosition.x - x, startingPosition.y, startingPosition.z + y);
                        Vector3 rotationAmount = new Vector3(startingRotation.x, firstR + currentR, startingRotation.z);
                        objectTracked.transform.eulerAngles = rotationAmount;
                        firstR = firstR + currentR;
                        prevX = x;
                        prevY = y;
                        prevR = r;

                }
            }
        }
            else
            {
                Debug.LogError("Error fetching camera data: " + request.error);
            }

            // Espera x segundos antes de la pr�xima actualizaci�n
            //yield return new WaitForSeconds(2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}
