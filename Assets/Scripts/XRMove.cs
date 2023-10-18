using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRMove : MonoBehaviour
{

    public GameObject myXRRig;

    public float y;

    public float z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLocation1()
    {
        y = -3.54F;
        
        z = 5.64F;

        myXRRig.transform.position = new Vector3(0,y,z);
    }

    public void GoToLocation2()
    {
        myXRRig.transform.position = new Vector3(0,0,7.93F);
    }
}
