using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
[ExecuteAlways]
public class CableProceduralMultipoint : MonoBehaviour
{

	LineRenderer line;

	// The Start of the cable will be the transform of the GameObject that has this component.
	// The Transforms that define the cable. This needs to be assigned in the inspector.
	[SerializeField, Tooltip("Series of points in cable.  If this transform is the desired start, add it to this array.")] CableSection[] cableSections;

	// Number of points per meter
	[SerializeField, Tooltip("Number of points per unit length, using the straight line from the start to the end transform.")] float pointDensity = 3;
	// These are used later for calculations
	Vector3 sagDirection;

	// cable section
	[Serializable] class CableSection
	{
		public Transform start, end;
		public float sag;
	}



	void Start () 
	{
		line = GetComponent<LineRenderer>();

		line.positionCount = 0;

		if (cableSections.Length == 0)
		{
			Debug.LogError("No sections assigned to Cable_Procedural component attached to " + gameObject.name);
			return;
		}
		
		// The Direction of SAG is the direction of gravity
		sagDirection = Physics.gravity.normalized;

		// Draw each section 
		foreach (CableSection section in cableSections)
		{
			Draw(section);
		}
	}




	void Draw(CableSection section)
	{
		// Get direction Vector.
		Vector3 vectorFromStartToEnd = section.end.position - section.start.position;
		// Setting the Start object to look at the end will be used for making the wind be perpendicular to the cable later.
		section.start.forward = vectorFromStartToEnd.normalized;
		// Get number of points in the cable using the distance from the start to end, and the point density
		int pointsCount = Mathf.FloorToInt(pointDensity * vectorFromStartToEnd.magnitude);
		
		// What point is being calculated
		int i = 0;
			   
		while(i < pointsCount)
		{
			// This is the fraction of where we are in the cable and it accounts for arrays starting at zero.
			float pointForCalcs = (float)i / ((pointsCount - 1) > 0 ? pointsCount - 1 : 1 );
			// This is what gives the cable a curve and makes the wind move the center the most.
			float effectAtPointMultiplier = Mathf.Sin(pointForCalcs * Mathf.PI);

			// Calculate the position of the current point i
			Vector3 pointPosition = vectorFromStartToEnd * pointForCalcs;
			// Calculate the sag vector for the current point i
			Vector3 sagAtPoint = sagDirection * section.sag;
			// Calculate the sway vector for the current point i

			// Calculate the postion with Sag.
			Vector3 currentPointsPosition = 
				section.start.position + 
				pointPosition +
				sagAtPoint * effectAtPointMultiplier;

			// Add a point to the line renderer
			line.positionCount += 1;

			// Set point
			line.SetPosition(line.positionCount - 1, currentPointsPosition);
			i++;
		}
	}
}
