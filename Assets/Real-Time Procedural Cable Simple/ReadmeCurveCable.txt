Procedural Cable Curve v1.0
DrinkingWindGames



NOTE:  All you need from the downloaded asset package is "CableProceduralStatic"



How To Use:

1.	Attach "CableProceduralCurve" component to an empty, a lineRenderer will be automatically added.

2.	Assign any other transform you desire into the endPointTransform in the inspector.
	(using another empty is recommended)

3.	You need to set up the lineRenderer as you desire, YOU DO NOT NEED TO DO ANYHING WITH THE POISITONS.

4.	Assign the CableProceduralCurve's values as desired:
		Point Density = how many points per unit length will define the cable (smaller density means better performance).
		Curve = the offset of the cable from the line between the start and end.  Negative values on curve keys pull the cable down.


Tips:

1. For Chains, set the lineRenderer's "Texture Mode" to "Repeat Per Segment".