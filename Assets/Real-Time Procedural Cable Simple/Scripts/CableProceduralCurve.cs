using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
[ExecuteAlways]
public class CableProceduralCurve : MonoBehaviour
{

	LineRenderer line;

	// The Start of the cable will be the transform of the GameObject that has this component.
	// The Transform of the Gameobject where the End of the cable is. This needs to be assigned in the inspector.
	[SerializeField] Transform endPointTransform;

	// Number of points per meter
	[SerializeField, Tooltip("Number of points per unit length, using the straight line from the start to the end transform.")] float pointDensity = 3;

	// How much the cable will sag by.
	[SerializeField, Tooltip("Positive keys are applied downward on cable.  Recommended use is to have keys at 0 at the start and end of the curve.")] AnimationCurve curve;

	// These are used later for calculations
	int pointsInLineRenderer;
	Vector3 vectorFromStartToEnd;
	Vector3 sagDirection;



	void Start () 
	{
		line = GetComponent<LineRenderer>();

		if (!endPointTransform)
		{
			Debug.LogError("No Endpoint Transform assigned to Cable_Procedural component attached to " + gameObject.name);
			return;
		}

		// Get direction Vector.
		vectorFromStartToEnd = endPointTransform.position - transform.position;
		// Setting the Start object to look at the end will be used for making the wind be perpendicular to the cable later.
		transform.forward = vectorFromStartToEnd.normalized;
		// Get number of points in the cable using the distance from the start to end, and the point density
		pointsInLineRenderer = Mathf.FloorToInt(pointDensity * vectorFromStartToEnd.magnitude);
		// Set number of points in line renderer
		line.positionCount = pointsInLineRenderer;

		// The Direction of SAG is the direction of gravity
		sagDirection = Physics.gravity.normalized;

		Draw();
	}




	void Draw()
	{
		// What point is being calculated
		int i = 0;
			   
		while(i < pointsInLineRenderer)
		{
			// This is the fraction of where we are in the cable and it accounts for arrays starting at zero.
			float pointForCalcs = (float)i / (pointsInLineRenderer - 1);

			// Calculate the position of the current point i
			Vector3 pointPosition = vectorFromStartToEnd * pointForCalcs;
			// Calculate the sag vector for the current point i
			Vector3 sagAtPoint = sagDirection * - curve.Evaluate(pointForCalcs);
			// Calculate the sway vector for the current point i

			// Calculate the postion with Sag.
			Vector3 currentPointsPosition = 
				transform.position + 
				pointPosition + 
				sagAtPoint;


			// Set point
			line.SetPosition(i, currentPointsPosition);
			i++;
		}
	}
}
