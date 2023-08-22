Procedural Cable Multipoint v1.0
DrinkingWindGames



NOTE:  All you need from the downloaded asset package is "CableProceduralStatic"



How To Use:

1.	Attach "CableProceduralCurve" component to an empty, a lineRenderer will be automatically added.

2.  Set the CableSections array length to the number of sections you need.

3.	Assign each cable section's Start and End transforms, it is intended that the end of one section is the start of the next section.
	(using empties is recommended)

4.  Assign each cable section's Sag.

4.	You need to set up the lineRenderer as you desire, YOU DO NOT NEED TO DO ANYHING WITH THE POISITONS.

5.	Assign the CableProceduralMultipoint's value as desired:
		Point Density = how many points per unit length will define the cable (smaller density means better performance).


Tips:

1. For Chains, set the lineRenderer's "Texture Mode" to "Repeat Per Segment".