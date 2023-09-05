using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update

    public UDPReceive udpReceive; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        string data = udpReceive.data;
        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
        string[] coords = data.Split(',');

        float x = float.Parse(coords[0])/100;
        float y = float.Parse(coords[1])/100;
        float z = float.Parse(coords[2])/1000;

        gameObject.transform.localPosition = new Vector3(x, y, z);

    }
}
