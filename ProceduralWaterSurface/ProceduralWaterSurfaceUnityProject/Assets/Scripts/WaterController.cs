using System.Collections;
using System.Collections.Generic;
using System.Linq;	//required inorder to convert list to array
using UnityEngine;

/* Source Code written by Kostas Sfikas, March 2017
Tested with Unity 5.5.0 f3 Pesonal edition*/

[RequireComponent(typeof(MeshFilter))]		// making sure that the gameobject has a MeshFilter component
[RequireComponent(typeof(MeshRenderer))]	// making sure that the gameobject has a MeshRenderer component
public class WaterController : MonoBehaviour {
	/* This class creates a custom mesh that resembles a water (or other fluid) surface. The surface consists of 
	a grid of points that extends along the X and Z axes. Those points have a fixed X and Z position, but their 
	Y-value can be animated through time, using sinusoidal functions and thus create the effect of waves.
	The Waves are created in relation to the position of any scene object which is tagged as "wavesource".
	You can alter the number of wavesources and their position and see the effect that it has on the waves. */

	List<GameObject> waveSources;	//all of the wave sources (will be collected by tag)
	// declaring variables
	float waterLevelY;			// the water - surface's level (along the Y - axis)
	float surfaceActualWidth;	// surface - dimension along the X-axis
	float surfaceActualLength;	// surface - dimension along the Z-axis
	int surfaceWidthPoints;		// number of points on the X-axis
	int surfaceLengthPoints;	// number of points on the Z-axis
	float localTime;			// the current time (used for animating the waves)
	float localTimeScale;			// local time scale (makes the animation go faster or slower - accepts negative values as well)
	// Use this for initialization
	void Awake () {
		// initialize the values of variables
		waterLevelY = 0.0f;
		surfaceActualWidth = 10;
		surfaceActualLength = 10;
		surfaceWidthPoints = 100;
		surfaceLengthPoints = 100;
		localTime = 0.0f;
		localTimeScale = 2.0f;

		CreateMesh ();	// create the initial geometry of the water-mesh

		waveSources = new List<GameObject> ();										// initialize the wave sources list
		waveSources = GameObject.FindGameObjectsWithTag ("wavesource").ToList ();	//find all the wavesources in the scene and pass them in the list
	}

	// Update is called once per frame
	void Update () {
		localTime += Time.deltaTime * localTimeScale;	//advance local time...
		UpdateWaterMesh ();	//update the geometry of the created mesh (animation happens here)
	}

	private void UpdateWaterMesh(){
		/*This function updates the water mesh, by recalculating
		each point's Y - value using the CalculateWaterY() function*/
		Mesh waterMesh = GetComponent<MeshFilter> ().mesh; 	//get the current mesh from the MeshFilter
		Vector3[] verts = waterMesh.vertices;				//get the mesh's vertices
		for (int i = 0; i < verts.Length; i++) {	//cycle through all the vertices of the mesh
			float x = verts [i].x;					//value of X stays the same
			float y = RecalculatePointY(verts[i]);	//calculate the new Y-value for the mesh, using the calculateWaterY() function
			float z = verts [i].z;					//value of Z stays the same
			Vector3 p = new Vector3 (x,y,z);		//create a new point (with updated Y - value)
			verts [i] = p;							//replace the vertice
		}
		waterMesh.vertices = verts;			//pass the updated vertices array to the existing mesh
		waterMesh.RecalculateNormals();		//recalculate the normals of the surface inorder to have correct shading
	}

	private float RecalculatePointY(Vector3 point){
		/*This function recalculates the Y - value of each point of the water - surface
		by applying a sinusoidal function on the point, for each of the wave - sources
		that there are in the scene. */
		float y = 0.0f;	//initialize the y - value (set to zero)
		for (int i = 0; i < waveSources.Count; i++) {	//cycle through all of the wave sources
			Vector2 p1 = new Vector2 (point.x, point.z);	// 2D - version of the incoming 3D Point (Vector3)
			Vector2 p2 = new Vector2 (waveSources[i].transform.position.x, waveSources[i].transform.position.z);	// the wave-source's 2d-position
			float dist = Vector2.Distance (p1,p2);	//the distance between the water-point and the current wave source
			y += Mathf.Sin (dist * 12.0f - localTime) / (dist*20.0f+10.0f);	// apply the first wave
		}
		y += waterLevelY;	//add the water-level-Y value to the result of the calculation
		return y;
	}

	private void CreateMesh(){
		/* This function creates the mesh object - triangle by triangle - 
		 * and then applies it to the Mesh Filter's mesh. */
		Mesh newMesh = new Mesh();
		List<Vector3> verticeList = new List<Vector3>();	// list that will hold the mesh vertices
		List<Vector2> uvList = new List<Vector2>();			// list that will hold the mesh UVs
		List<int> triList = new List<int>();				// list that will hold the mesh triangles
		//mesh - data creation double loop
		for (int i = 0; i < surfaceWidthPoints; i++){		
			for (int j = 0; j < surfaceLengthPoints; j++){
				float x = MapValue (i, 0.0f, surfaceWidthPoints, -surfaceActualWidth/2.0f, surfaceActualWidth/2.0f);
				float z = MapValue (j, 0.0f, surfaceLengthPoints, -surfaceActualLength/2.0f, surfaceActualLength/2.0f);
				verticeList.Add(new Vector3(x, 0f, z));
				uvList.Add(new Vector2(x, z));
				//Skip if a new square on the plane hasn't been formed
				if (i == 0 || j == 0)
					continue;
				//Adds the index of the three vertices in order to make up each of the two tris
				triList.Add(surfaceLengthPoints * i +j); //Top right
				triList.Add(surfaceLengthPoints * i + j - 1); //Bottom right
				triList.Add(surfaceLengthPoints * (i - 1) + j - 1); //Bottom left - First triangle
				triList.Add(surfaceLengthPoints * (i - 1) + j - 1); //Bottom left 
				triList.Add(surfaceLengthPoints * (i- 1) + j); //Top left
				triList.Add(surfaceLengthPoints * i + j); //Top right - Second triangle
			}
		}
		//creating the mesh with the data generated above
		newMesh.vertices = verticeList.ToArray();	//pass vertices to mesh
		newMesh.uv = uvList.ToArray();				//pass uv list to mesh
		newMesh.triangles = triList.ToArray();		//pass triabgles to mesh
		newMesh.RecalculateNormals();				//recalculate mesh normals
		GetComponent<MeshFilter>().mesh = newMesh;	//pass the created mesh to the mesh filter
	}

	private float MapValue(float refValue, float refMin, float refMax, float targetMin, float targetMax){
		/* This function converts the value of a variable (reference value) from one range (reference range) to another (target range)
		in this example it is used to convert the x and z value to the correct range, while creating the mesh, in the CreateMesh() function*/
		return targetMin + (refValue - refMin) * (targetMax - targetMin) / (refMax - refMin);
	}
}
