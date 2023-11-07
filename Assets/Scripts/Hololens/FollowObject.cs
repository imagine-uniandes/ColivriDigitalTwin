using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform targetObject;
    public Vector3 offset;

    void Update()
    {
        if (targetObject != null)
        {
            transform.position = targetObject.position + offset;
        }
    }
}
