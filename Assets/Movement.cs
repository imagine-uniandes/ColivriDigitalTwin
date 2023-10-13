using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update

    public UDPReceive udpReceive; 

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 rotationAmount;

    private float startingX;
    private float startingY;
    private float startingR;

    private float prevX = 0;
    private float prevY = 0;
    private float prevR = 0;

    private bool b = true;

    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        if(b){
            string data = udpReceive.data;
            data = data.Remove(0, 1);
            data = data.Remove(data.Length-1, 1);
            string[] coords = data.Split(',');

            startingX = float.Parse(coords[0])/1000;
            startingY = float.Parse(coords[1])/1000;
            startingR = float.Parse(coords[2]);
            b = false;

            startingPosition = gameObject.transform.position;
            startingRotation = gameObject.transform.rotation;
        } else {

            string data = udpReceive.data;
            data = data.Remove(0, 1);
            data = data.Remove(data.Length-1, 1);
            string[] coords = data.Split(',');

            float currentX = float.Parse(coords[0])/1000;
            float currentY = float.Parse(coords[1])/1000;
            float currentR = float.Parse(coords[2]);

            float x = startingX - currentX;
            float y = startingY - currentY;
            float r = startingR - currentR;





            if(Math.Abs(x-prevX) < 0.001 || Math.Abs(y-prevY) < 0.001){
                prevX = x;
                prevY = y;
            } else {
                gameObject.transform.localPosition = new Vector3(startingPosition.x - x, startingPosition.y, startingPosition.z + y);
                Vector3 rotationAmount = new Vector3(0, startingRotation.y - r, 0);
                transform.eulerAngles = rotationAmount;
                Debug.Log(currentR);
                prevX = x;
                prevY = y;
                prevR = r;

            }
        }
    }
}
