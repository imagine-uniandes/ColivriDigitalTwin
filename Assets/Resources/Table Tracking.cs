using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ObjectTracker : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    public GameObject unityObject; // The Unity object that will be updated

    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            // Object is detected, update the position and rotation of the Unity object
            unityObject.transform.position = transform.position;
            unityObject.transform.rotation = transform.rotation;
        }
    }
}

