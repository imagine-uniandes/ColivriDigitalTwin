Procedural Cable Simple v1.1
DrinkingWindGames



NOTE:  All you need from the downloaded asset package is "CableProceduralSimple"



How To Use:

1.	Attach "CableProceduralSimple" component to an empty, a lineRenderer will be automatically added.

2.	Assign any other transform you desire into the endPointTransform in the inspector.
	(using another empty is recommended)

3.	You need to set up the lineRenderer as you desire, YOU DO NOT NEED TO DO ANYHING WITH THE POISITONS.

4.	Assign the CableProceduralSimple's values as desired:
		Point Density = how many points per unit length will define the cable (smaller density means better performance).
		Sag Amplitude = how far the cable will sag in the middle in Unity units (meters).
		Sway Multiplier = how far the cable will sway side to side and up and down in Unity units.
		Sway X Multiplier = this will change how much the cable will sway in the local X direction.
		Sway Y Multiplier = this will change how much the cable will sway in the local Y direction.
		Sway Frequency = how many times the cable will cycle per second (Hertz).


Tips:

1. For Chains, set the lineRenderer's "Texture Mode" to "Repeat Per Segment".