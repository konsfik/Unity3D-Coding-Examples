/*	Author: Kostas Sfikas
	Date: March 2017
	Language: C#
	Platform: Unity 5.5.0 f3 (personal edition) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;	//required for conversion of Lists to Arrays

[ExecuteInEditMode]							//the script is also updated while in edit mode.
[RequireComponent(typeof(MeshFilter))]		// The gameObject that this script is attached to must have a MeshFilter component attached to it
[RequireComponent(typeof(MeshRenderer))]	// The gameObject that this script is attached to must have a MeshRenderer component attached to it
public class ProceduralLandscapeController : MonoBehaviour {
	/* This class procedurally creates a landscape formation. It uses the perlin noise function to create the 
	height map of the landscape. 
	(Perlin Noise Function Reference: "https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html")
	There are three levels of detail which are summed up to construct the final landscape's form: Layer1 (coarse detail), 
	Layer2 (medium detail) and Layer3 (fine detail). The summing of those layers happens in the LandscapePoint(int i, int j) 
	function, which returns a Vector3 (a 3D point) that is the position of e vertex.
	-----------------------------------------------------------------------------------------------------------
	The class also creates a custom, procedural material that is colored as a gradient height map that 
	ranges between the two colors: bottomColor, peakColor.
	-----------------------------------------------------------------------------------------------------------
	You can use the sliders to control the landscape's form in real - time, in Unity Editor's edit-mode
	or in play mode.
	-----------------------------------------------------------------------------------------------------------
	There is also another option, to enable the "animate" choice, which produces a random, ever-changing landscape.*/

	public bool animate;	//boolean public variable. If set to true, the landscape will "dance" in play mode.

	/* The variables that follow are public. They are exposed to the editor and are accompanied by sliders
	that work within a specific range, for easy modification of the Landscape-mesh while
	in edit mode. They also work while in Play Mode, but the changes that you make in Play Mode 
	will be lost once you spress the Stop button. */
	[Header ("updateable variables")]

	[Space(8)]		// leave a gap of 8 pixels between the header and the parameters in editor

	// control variables for Layer1
	[Range(700.0f,2500.0f)]					// layer1Scale - value slider ranges from 700 to 2500
	public float layer1Scale = 700.0f;		// the current scale of Layer 1
	float layer1ScalePreviousValue = 0;		// stores the layer1Scale's previous value, for change - detection

	[Range(1.0f,1000.0f)]					// layer1Height - value slider ranges from 1 to 1000
	public float layer1Height = 1.0f;		// the current height of Layer 1
	float layer1HeightPreviousValue = 0;	// stores the layer1Height's previous value, for change - detection

	[Range(1000.0f,1010.0f)]				// layer1OffsetX - value slider ranges from 1000 to 1010
	public float layer1OffsetX = 1000.0f;	// The current offset of Layer 1 along the X-axis
	float layer1OffsetXPreviousValue = 0;	// stores the layer1OffsetX's previous value, for change - detection

	[Range(1000.0f,1010.0f)]				// layer1OffsetZ - value slider ranges from 1000 to 1010
	public float layer1OffsetZ = 1000.0f;	// The current offset of Layer 1 along the Z-axis
	float layer1OffsetZPreviousValue = 0;	// stores the layer1OffsetZ's previous value, for change - detection

	[Space(8)]		// leave a gap of 8 pixels between the parameters in editor

	// control variables for Layer2
	[Range(200.0f,500.0f)]					// layer2Scale - value slider ranges from 200 to 500
	public float layer2Scale = 200.0f;		// the current scale of Layer 2
	float layer2ScalePreviousValue = 0;		// stores the layer2Scale's previous value, for change - detection

	[Range(1.0f,500.0f)]					// layer2Height - value slider ranges from 1 to 500
	public float layer2Height = 1.0f;		// the current height of Layer 2
	float layer2HeightPreviousValue = 0;	// stores the layer2Height's previous value, for change - detection

	[Range(1000.0f,1050.0f)]				// layer2OffsetX - value slider ranges from 1000 to 1050
	public float layer2OffsetX = 1000.0f;	// The current offset of Layer 2 along the X-axis
	float layer2OffsetXPreviousValue = 0;	// stores the layer2OffsetX's previous value, for change - detection

	[Range(1000.0f,1050.0f)]				// layer2OffsetZ - value slider ranges from 1000 to 1050
	public float layer2OffsetZ = 1000.0f;	// The current offset of Layer 2 along the Z-axis
	float layer2OffsetZPreviousValue = 0;	// stores the layer2OffsetZ's previous value, for change - detection

	[Space(8)]		// leave a gap of 8 pixels between the parameters in editor

	// control variables for Layer3
	[Range(20.0f,100.0f)]					// layer3Scale - value slider ranges from 20 to 100
	public float layer3Scale = 20.0f;		// the current scale of Layer 3
	float layer3ScalePreviousValue = 0;		// stores the layer3Scale's previous value, for change - detection

	[Range(1.0f,100.0f)]					// layer3Height - value slider ranges from 1 to 100
	public float layer3Height = 1.0f;		// the current height of Layer 3
	float layer3HeightPreviousValue = 0;	// stores the layer3Height's previous value, for change - detection

	[Range(1000.0f,1100.0f)]				// layer3OffsetX - value slider ranges from 1000 to 1100
	public float layer3OffsetX = 1000.0f;	// The current offset of Layer 3 along the X-axis
	float layer3OffsetXPreviousValue = 0;	// stores the layer3OffsetX's previous value, for change - detection

	[Range(1000.0f,1100.0f)]				// layer3OffsetZ - value slider ranges from 1000 to 1100
	public float layer3OffsetZ = 1000.0f;	// The current offset of Layer 3 along the Z-axis
	float layer3OffsetZPreviousValue = 0;	// stores the layer3OffsetZ's previous value, for change - detection

	[Space(8)]		// leave a gap of 8 pixels between the parameters in editor

	// material - specific parameters
	public Color peakColor;						// the color that will be applied at the peak of the hills
	Color peakColorPreviousValue;				//the previous value of peakColor, for change - detection

	public Color bottomColor;					// the color that will be used at the bottom of the hills
	Color bottomColorPreviousValue;				// the previous value of bottomColor, for change - detection

	[Range(0.0f,1.0f)]							// Material Smoothness - value slider ranges from 1000 to 1100
	public float materialSmoothness;			// The current Material Smoothness
	float materialSmoothnessPreviousValue;		// materialSmoothness's previous value, for change detection

	[Range(0.0f,1.0f)]							// Material Metallicness - value slider ranges from 1000 to 1100
	public float materialMetallicness;			// The current Material Metallicness
	float materialMetallicnessPreviousValue;	// materialMetallicness's previous value, for change detection


	//main variables
	float localTime;						// local time (used for the animation of the landscape)
	float localTimeScale;					// local timescale: scales the flow of time

	Mesh myMesh;							// the mesh (is created in this script)
	Material landScapeMaterial;				// the material that will be applied on the mesh (is created in this script)
	Texture2D landscapeMaterialTexture;		// the material's texture (is created in this script)

	int landscapeWidthSegments;				// number of segments along the X axis
	int landscapeLengthSegments;			// number of segments along the Z axis
	float landscapeWidth;					// landscape width: actual size along the X axis
	float landscapeLength;					// landscape length: actual size along the Z axis

	//mesh creation lists
	List<Vector3> verticeList = new List<Vector3>();	// list of vertices
	List<Vector2> uvList = new List<Vector2>();			// list of uvs
	List<int> triList = new List<int>();				// list of triangles

	// Use this for initialization
	void Awake () {
		animate = false;
		//initialize main landscape variables
		landscapeWidthSegments = 255;		// number of segments along the X axis
		landscapeLengthSegments = 255;		// number of segments along the Z axis
		landscapeWidth = 4000;				// landscape width: actual size along the X axis
		landscapeLength = 4000;				// landscape length: actual size along the Z axis

		// initialize time and time scale
		localTime = Random.Range( 0.0f, 1000.0f);
		localTimeScale = 0.3f;
		// create the actual landscape mesh and apply material
		CreateLandscapeGeometry ();								// make the landscape object (geometry)
		CreateLandscapeTexture();								// make the 2d texture
		CreateLandscapeMaterial();								// create the landscape material
		GetComponent<Renderer> ().material = landScapeMaterial;	// apply the material to the mesh renderer
	}

	// Update is called once per frame
	void Update () {
		if ((animate == true) && (Application.isPlaying)) {
			/*if the "animate" variable is set to true and the Application is Playing, 
			then the variables that produce the landscape are "animated".
			otherwise they are steady and can be changed by the user, using the sliders.*/
			localTime += Time.deltaTime * localTimeScale;	
			layer1Scale = mapValue (Mathf.Sin (localTime), -1.0f, 1.0f, 700.0f, 2500.0f);
			layer1Height = mapValue (Mathf.Sin (localTime * 1.1f), -1.0f, 1.0f, 1.0f, 700.0f);
			layer1OffsetX = mapValue (Mathf.Sin (localTime * 0.1f), -1.0f, 1.0f, 1000.0f, 1010.0f);
			layer1OffsetZ = mapValue (Mathf.Cos (localTime * 0.15f), -1.0f, 1.0f, 1000.0f, 1010.0f);
			layer2Scale = mapValue (Mathf.Sin (localTime * 1.2f), -1.0f, 1.0f, 200.0f, 500.0f);
			layer2Height = mapValue (Mathf.Sin (localTime * 1.3f), -1.0f, 1.0f, 10.0f, 500.0f);
			layer2OffsetX = mapValue (Mathf.Sin (localTime * 0.1f), -1.0f, 1.0f, 1000.0f, 1050.0f);
			layer2OffsetZ = mapValue (Mathf.Cos (localTime * 0.15f), -1.0f, 1.0f, 1000.0f, 1050.0f);
			layer3Scale = mapValue (Mathf.Sin (localTime * 1.4f), -1.0f, 1.0f, 20.0f, 100.0f);
			layer3Height = mapValue (Mathf.Sin (localTime * 1.5f), -1.0f, 1.0f, 1.0f, 100.0f);	
			layer3OffsetX = mapValue (Mathf.Sin (localTime * 0.1f), -1.0f, 1.0f, 1000.0f, 1100.0f);
			layer3OffsetZ = mapValue (Mathf.Cos (localTime * 0.15f), -1.0f, 1.0f, 1000.0f, 1100.0f);
			materialMetallicness = mapValue (Mathf.Sin (localTime * 2.0f), -1.0f, 1.0f, 0.0f, 1.0f);	
			materialSmoothness = mapValue (Mathf.Sin (localTime * 1.6f), -1.0f, 1.0f, 0.0f, 1.0f);	
			bottomColor.r = mapValue (Mathf.Sin (localTime * 0.5f), -1.0f, 1.0f, 0.0f, 1.0f);
			bottomColor.g = mapValue (Mathf.Sin (localTime * 0.6f), -1.0f, 1.0f, 0.0f, 1.0f);
			bottomColor.b = mapValue (Mathf.Sin (localTime * 0.7f), -1.0f, 1.0f, 0.0f, 1.0f);
			peakColor.r = mapValue (Mathf.Sin (localTime * 0.3f), -1.0f, 1.0f, 0.0f, 1.0f);
			peakColor.g = mapValue (Mathf.Sin (localTime * 0.4f), -1.0f, 1.0f, 0.0f, 1.0f);
			peakColor.b = mapValue (Mathf.Sin (localTime * 0.5f), -1.0f, 1.0f, 0.0f, 1.0f);
		}
		if (CheckForVariableValueChange ()) {
			/* If ANY of the Updateable Variables has changed, then the Landscape Geometry will be updated.
			This works in Edit Mode, as well as in Play Mode. */
			UpdateLandscapeGeometry ();								// update the landscape object (geometry)
			UpdateLandscapeTexture ();								// update the 2d texture
			UpdateLandscapeMaterial();								// update the landscape material
			GetComponent<Renderer> ().material = landScapeMaterial;	// apply the material to the mesh renderer
		}
	}

	private void UpdateLandscapeGeometry(){
		/* This function updates the mesh geometry by repositioning the mesh's existing vertices
		and then recalculating the normals. The mesh's inner structure does not change. */
		myMesh = GetComponent<MeshFilter> ().sharedMesh;
		int cnt = 0;
		for (int i = 0; i < landscapeLengthSegments; i++) {
			for (int j = 0; j < landscapeWidthSegments; j++) {
				verticeList [cnt] = LandscapePoint (i, j);
				cnt++;
			}
		}
		myMesh.vertices = verticeList.ToArray ();
		myMesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = myMesh;
	}

	private void CreateLandscapeGeometry(){
		/* This function creates the Grid Mesh upon which the Landscape will be built */
		myMesh = new Mesh();								//initialize the mesh
		// construct the mesh of the landscape
		for (int i = 0; i < landscapeLengthSegments; i++){
			for (int j = 0; j < landscapeWidthSegments; j++){
				Vector3 p = LandscapePoint (i, j);
				verticeList.Add(p);
				uvList.Add(new Vector2(p.x, p.z));
				//Skip if a new square on the plane hasn't been formed
				if (i == 0 || j == 0)
					continue;
				//Adds the index of the three vertices in order to make up each of the two tris
				triList.Add(landscapeWidthSegments * i +j); 			//Top right
				triList.Add(landscapeWidthSegments * i + j - 1); 		//Bottom right
				triList.Add(landscapeWidthSegments * (i - 1) + j - 1);	//Bottom left - First triangle
				triList.Add(landscapeWidthSegments * (i - 1) + j - 1);	//Bottom left 
				triList.Add(landscapeWidthSegments * (i- 1) + j);		//Top left
				triList.Add(landscapeWidthSegments * i + j);			//Top right - Second triangle
			}
		}
		myMesh.vertices = verticeList.ToArray();
		myMesh.uv = uvList.ToArray();
		myMesh.triangles = triList.ToArray();
		myMesh.RecalculateNormals();
		myMesh.name = "LandscapeMesh";
		GetComponent<MeshFilter>().mesh = myMesh;
	}

	private void UpdateLandscapeTexture(){
		/* This function updates the Landscape Texture that will then be applied to the landscape's material*/
		landscapeMaterialTexture = GetComponent<MeshRenderer> ().sharedMaterial.mainTexture as Texture2D;
		float landscapeMinHeight = GetMinimumLandscapeHeight ();	//calculete the landscape's current minimum height
		float landscapeMaxHeight = GetMaximumLandscapeHeight ();	//calculete the landscape's current maximum height
		for(int i = 0; i < landscapeWidthSegments; i++){
			for(int j = 0; j < landscapeLengthSegments; j++){
				//cycle through all of the landscape's vertices
				Vector3 p = LandscapePoint (i, j);	// get the landscape point
				//calculate the landscape point's relative height: where it lies between the current minimum ang maximum height of the landscape
				float landscapeRelativeHeight = mapValue (p.y, landscapeMinHeight, landscapeMaxHeight, 0.0f, 1.0f);	// will receive a value 0.0f to 1.0f (relative to minimum to maximum)
				//use the landscapeRelativeHeight to create a gradient between the two colors
				float cr = mapValue (landscapeRelativeHeight, 0.0f, 1.0f, bottomColor.r, peakColor.r);	// set the color's red value accordingly
				float cg = mapValue (landscapeRelativeHeight, 0.0f, 1.0f, bottomColor.g, peakColor.g);	// set the color's green value accordingly
				float cb = mapValue (landscapeRelativeHeight, 0.0f, 1.0f, bottomColor.b, peakColor.b);	// set the color's blue value accordingly
				Color cc = new Color(cr,cg,cb,1.0f);			//create the color, based on the r, g, b values (calculated above)
				landscapeMaterialTexture.SetPixel (i, j, cc);	// set the pixel's color
			}
		}
		landscapeMaterialTexture.Apply ();
	}

	private void CreateLandscapeTexture(){
		// This function creates the texture that will then be applied to the landscape's material.
		landscapeMaterialTexture = new Texture2D (landscapeWidthSegments, landscapeLengthSegments, TextureFormat.RGB24, true);
		landscapeMaterialTexture.name = "LandscapeMaterialTexture";
		landscapeMaterialTexture.wrapMode = TextureWrapMode.Clamp;
		float landscapeMinHeight = GetMinimumLandscapeHeight ();
		float landscapeMaxHeight = GetMaximumLandscapeHeight ();
		for(int i = 0; i < landscapeWidthSegments; i++){
			for(int j = 0; j < landscapeLengthSegments; j++){
				//cycle through all of the landscape's vertices
				Vector3 p = LandscapePoint (i, j);	// get the landscape point
				//calculate the landscape point's relative height: where it lies between the current minimum ang maximum height of the landscape
				float landscapeRelativeHeight = mapValue (p.y, landscapeMinHeight, landscapeMaxHeight, 0.0f, 1.0f);	// will receive a value 0.0f to 1.0f (relative to minimum to maximum)
				//use the 
				float cr = mapValue (landscapeRelativeHeight, 0.0f, 1.0f, bottomColor.r, peakColor.r);	// set the color's red value accordingly
				float cg = mapValue (landscapeRelativeHeight, 0.0f, 1.0f, bottomColor.g, peakColor.g);	// set the color's green value accordingly
				float cb = mapValue (landscapeRelativeHeight, 0.0f, 1.0f, bottomColor.b, peakColor.b);	// set the color's blue value accordingly
				Color cc = new Color(cr,cg,cb,1.0f);
				landscapeMaterialTexture.SetPixel (i, j, cc);
			}
		}
		landscapeMaterialTexture.Apply ();
	}

	private void UpdateLandscapeMaterial(){
		/* This function updates the Landscape Material	*/
		landScapeMaterial = GetComponent<MeshRenderer> ().sharedMaterial;
		landScapeMaterial.mainTexture = landscapeMaterialTexture;
		Vector2 matScale = new Vector2 (1.0f/landscapeWidth,1.0f/landscapeLength);
		landScapeMaterial.mainTextureScale = matScale;
		Vector2 matOffset = new Vector2 (0.5f, 0.5f);
		landScapeMaterial.mainTextureOffset = matOffset;
		landScapeMaterial.SetFloat ("_Metallic", materialMetallicness);
		landScapeMaterial.SetFloat ("_Glossiness", materialSmoothness);
	}

	private void CreateLandscapeMaterial(){
		/* This function creates the Landscape Material	*/
		landScapeMaterial = new Material (Shader.Find ("Standard"));
		landScapeMaterial.mainTexture = landscapeMaterialTexture;
		Vector2 matScale = new Vector2 (1.0f/landscapeWidth,1.0f/landscapeLength);
		landScapeMaterial.mainTextureScale = matScale;
		Vector2 matOffset = new Vector2 (0.5f, 0.5f);
		landScapeMaterial.mainTextureOffset = matOffset;
		landScapeMaterial.SetFloat ("_Metallic", materialMetallicness);
		landScapeMaterial.SetFloat ("_Glossiness", materialSmoothness);
	}

	Vector3 LandscapePoint(int i, int j){
		//calculate X
		float minimumX = -(landscapeLength / 2.0f);
		float maximumX = landscapeLength / 2.0f;
		float x = mapValue ((float)i, 0.0f, (float)landscapeLengthSegments, minimumX, maximumX);
		//calculate Z
		float minimumZ = -(landscapeWidth / 2.0f);
		float maximumZ = landscapeWidth / 2.0f;
		float z = mapValue ((float)j, 0.0f, (float)landscapeWidthSegments, minimumZ, maximumZ );
		//calculate Y
		float perlin1 = Mathf.PerlinNoise (-layer1OffsetX + x/layer1Scale, -layer1OffsetZ + z/layer1Scale);
		float perlin2 = Mathf.PerlinNoise (-layer2OffsetX + x/layer2Scale, -layer2OffsetZ + z/layer2Scale);
		float perlin3 = Mathf.PerlinNoise (-layer3OffsetX + x/layer3Scale, -layer3OffsetZ + z/layer3Scale);

		float y1 = perlin1 * layer1Height;
		float y2 = perlin2 * perlin1 * layer2Height;
		float y3 = perlin3 * perlin2 * perlin1 * layer3Height;
		float y = y1 + y2 + y3;
		//set the final point
		Vector3 vertPos = new Vector3 (x, y, z);
		return vertPos;
	}
		
	float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
		/* This function maps (converts) a value from one range to another */
		return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	float GetMinimumLandscapeHeight(){
		/* This function searches for the minimum height of the landscape by cycling through all of its
		vertices and comparing their Y-values. It returns the lowest Y-value as a float. */
		float h = verticeList [0].y;
		for (int i = 0; i < verticeList.Count; i++) {
			if (verticeList [i].y < h)
				h = verticeList [i].y;
		}
		return h;
	}

	float GetMaximumLandscapeHeight(){
		/* This function searches for the maximum height of the landscape by cycling through all of its
		vertices and comparing their Y-values.  It returns the highest Y-value as a float. */
		float h = verticeList [0].y;
		for (int i = 0; i < verticeList.Count; i++) {
			if (verticeList [i].y > h)
				h = verticeList [i].y;
		}
		return h;
	}


	bool CheckForVariableValueChange(){
		/* This function checks whether any of those variables (see below) has been changed since the last frame.
		If ANY of them has been changed, then it returns TRUE and updates their previous value to current value. 
		Otherwise it returns false. 
		It is used in the Update, so that the mesh's geometry and material are updated ONLY when there has been a
		change in the variables thet define them. This way the algorithm avoids unnecessary calls to geometry and
		material Updates. */

		if(	//check whether anything has changed...
			(layer1OffsetXPreviousValue != layer1OffsetX)					// if layer1OffestX has changed
			||(layer1OffsetZPreviousValue != layer1OffsetZ)					// or layer1OffsetZ has changed
			||(layer2OffsetXPreviousValue != layer2OffsetX)					// or layer2OffestX has changed
			||(layer2OffsetZPreviousValue != layer2OffsetZ)					// or layer2OffsetZ has changed
			||(layer3OffsetXPreviousValue != layer3OffsetX)					// or layer3OffestX has changed
			||(layer3OffsetZPreviousValue != layer3OffsetZ)					// or layer3OffsetZ has changed
			||(layer1ScalePreviousValue != layer1Scale)						// or layer1Scale has changed
			||(layer2ScalePreviousValue != layer2Scale)						// or layer2Scale has changed
			||(layer3ScalePreviousValue != layer3Scale)						// or layer3Scale has changed
			||(layer1HeightPreviousValue != layer1Height)					// or layer1Height has changed
			||(layer2HeightPreviousValue != layer2Height)					// or layer2Height has changed
			||(layer3HeightPreviousValue != layer3Height)					// or layer3Height has changed
			||(materialSmoothnessPreviousValue != materialSmoothness)		// or materialSmoothness has changed
			||(materialMetallicnessPreviousValue != materialMetallicness)	// or materialMetallicness has changed
			||(peakColorPreviousValue != peakColor)							// or peakColor has changed
			||(bottomColorPreviousValue != bottomColor)						// or bottomColor has changed
		){
			// then set previous values to current values for all of the change-able variables
			layer1OffsetXPreviousValue = layer1OffsetX;
			layer1OffsetZPreviousValue = layer1OffsetZ;
			layer2OffsetXPreviousValue = layer2OffsetX;
			layer2OffsetZPreviousValue = layer2OffsetZ;
			layer3OffsetXPreviousValue = layer3OffsetX;
			layer3OffsetZPreviousValue = layer3OffsetZ;
			layer1ScalePreviousValue = layer1Scale;
			layer2ScalePreviousValue = layer2Scale;
			layer3ScalePreviousValue = layer3Scale;
			layer1HeightPreviousValue = layer1Height;
			layer2HeightPreviousValue = layer2Height;
			layer3HeightPreviousValue = layer3Height;
			peakColorPreviousValue = peakColor;
			bottomColorPreviousValue = bottomColor;
			//send message to log
			Debug.Log ("value change");
			// and return true
			return true;	
		}
		else return false;	//else return false
	}
}
