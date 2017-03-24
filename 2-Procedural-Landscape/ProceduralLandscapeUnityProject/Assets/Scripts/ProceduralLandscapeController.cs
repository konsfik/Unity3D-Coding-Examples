/*	Author: Kostas Sfikas
	Date: March 2017
	Language: C#
	Platform: Unity 5.5.0 f3 (personal edition) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralLandscapeController : MonoBehaviour {
	/*This class creates a procedural landscape formation. It uses the perlin noise
	technique to do that.
	This class creates a 3d-landscape formation by using three combined layer of perlin noise functions.
	Uses */
	//main variables
	float myTime;					// time
	float myTimeScale;				// timescale: scales the flow of time
	int landscapeWidthSegments;		// number of segments along the X axis
	int landscapeLengthSegments;	// number of segments along the Z axis
	float landscapeWidth;			// landscape width: actual size along the X axis
	float landscapeLength;			// landscape length: actual size along the Z axis
	//landscape formation variables
	//float seed;

	public bool animate;
	// update form variables
	[Header ("updateable variables")]

	[Space(8)]
	// control of layer 1
	[Range(700.0f,2000.0f)]
	public float layer1Scale = 200.0f;
	float layer1ScalePreviousValue = 0;
	[Range(1.0f,500.0f)]
	public float layer1Height = 250.0f;
	float layer1HeightPreviousValue = 0;
	[Range(1000.0f,1010.0f)]
	public float layer1OffsetX = 1000.0f;
	float layer1OffsetXPreviousValue;
	[Range(1000.0f,1010.0f)]
	public float layer1OffsetZ = 1000.0f;
	float layer1OffsetZPreviousValue;

	[Space(8)]
	// control of layer 2
	[Range(200.0f,500.0f)]
	public float layer2Scale = 50.0f;
	float layer2ScalePreviousValue = 0;
	[Range(1.0f,250.0f)]
	public float layer2Height = 20.0f;
	float layer2HeightPreviousValue = 0;
	[Range(1000.0f,1010.0f)]
	public float layer2OffsetX = 1000.0f;
	float layer2OffsetXPreviousValue;
	[Range(1000.0f,1010.0f)]
	public float layer2OffsetZ = 1000.0f;
	float layer2OffsetZPreviousValue;

	[Space(8)]
	// control of layer 3
	[Range(10.0f,100.0f)]
	public float layer3Scale = 8.0f;
	float layer3ScalePreviousValue = 0;
	[Range(1.0f,100.0f)]
	public float layer3Height = 1.0f;
	float layer3HeightPreviousValue = 0;
	[Range(1000.0f,1010.0f)]
	public float layer3OffsetX = 1000.0f;
	float layer3OffsetXPreviousValue;
	[Range(1000.0f,1010.0f)]
	public float layer3OffsetZ = 1000.0f;
	float layer3OffsetZPreviousValue;

	[Space(8)]
	//texture
	public Color peakColor;
	Color peakColorPreviousValue;
	public Color bottomColor;
	Color bottomColorPreviousValue;
	[Range(0.0f,1.0f)]
	public float materialSmoothness;
	float materialSmoothnessPreviousValue;
	[Range(0.0f,1.0f)]
	public float materialMetallicness;
	float materialMetallicnessPreviousValue;


	Mesh myMesh;										// the mesh (is created in this script)
	Material landScapeMaterial;							// the material that will be applied on the mesh (is created in this script)
	Texture2D landscapeMaterialTexture;					// the matrial's texture (is created in this script)
	//mesh creation lists
	List<Vector3> verticeList = new List<Vector3>();	//list of vertices
	List<Vector2> uvList = new List<Vector2>();			//list of uvs
	List<int> triList = new List<int>();				//list of triangles
	// Use this for initialization
	void Awake () {
		//animate = true;
		//initialize main variables
		landscapeWidthSegments = 200;		// the 
		landscapeLengthSegments = 200;
		landscapeWidth = 4000;
		landscapeLength = 4000;

		//initialize landscape formation variables
		myTime = 0.0f;
		myTimeScale = 0.3f;
		CreateLandscapeGeometry ();	// 1. make the landscape object (geometry)
		CreateLandscapeTexture();	// 2. make the 2d texture
		CreateLandscapeMaterial();	// 3. create the landscape material
		// 4. assign the landscape material to the landscape object
		GetComponent<Renderer> ().material = landScapeMaterial;
	}

	// Update is called once per frame
	void Update () {
		if (animate == true) {
			/*if the "animate" variable is set to true, then the variables that produce the landscape are animated.
			otherwise they are steady and can be changed by the user, using the sliders.*/
			myTime += Time.deltaTime * myTimeScale;
			layer1Scale = map (Mathf.Sin (myTime), -1.0f, 1.0f, 300.0f, 500.0f);
			layer1Height = map (Mathf.Sin (myTime * 1.1f), -1.0f, 1.0f, 100.0f, 500.0f);
			layer2Scale = map (Mathf.Sin (myTime * 1.2f), -1.0f, 1.0f, 100.0f, 150.0f);
			layer2Height = map (Mathf.Sin (myTime * 1.3f), -1.0f, 1.0f, 50.0f, 150.0f);
			layer3Scale = map (Mathf.Sin (myTime * 1.4f), -1.0f, 1.0f, 10.0f, 40.0f);
			layer3Height = map (Mathf.Sin (myTime * 1.5f), -1.0f, 1.0f, 10.0f, 40.0f);
			/*bottomColor.r += Random.Range (-0.05f, 0.05f);
			bottomColor.g += Random.Range (-0.05f, 0.05f);
			bottomColor.b += Random.Range (-0.05f, 0.05f);
			peakColor.r += Random.Range (-0.05f, 0.05f);
			peakColor.g += Random.Range (-0.05f, 0.05f);
			peakColor.b += Random.Range (-0.05f, 0.05f);*/
		}
		if (UpdateVariableValueChange ()) {
			UpdateLandscapeGeometry ();
		}
	}

	private void UpdateLandscapeGeometry(){
		/*This function updates the mesh geometry by repositioning the mesh's existing vertices
		and then recalculating the normals. The mesh's inner structure does not change.*/
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

		CreateLandscapeTexture ();
		CreateLandscapeMaterial();
		GetComponent<Renderer> ().material = landScapeMaterial;
	}

	private void CreateLandscapeGeometry(){
		/*This function creates the Grid Mesh upon which the Landscape will be built
		*/
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
				triList.Add(landscapeWidthSegments * i +j); //Top right
				triList.Add(landscapeWidthSegments * i + j - 1); //Bottom right
				triList.Add(landscapeWidthSegments * (i - 1) + j - 1); //Bottom left - First triangle
				triList.Add(landscapeWidthSegments * (i - 1) + j - 1); //Bottom left 
				triList.Add(landscapeWidthSegments * (i- 1) + j); //Top left
				triList.Add(landscapeWidthSegments * i + j); //Top right - Second triangle
			}
		}
		myMesh.vertices = verticeList.ToArray();
		myMesh.uv = uvList.ToArray();
		myMesh.triangles = triList.ToArray();
		myMesh.RecalculateNormals();
		myMesh.name = "LandscapeMesh";
		GetComponent<MeshFilter>().mesh = myMesh;
	}

	private void CreateLandscapeTexture(){
		//creates the texture that will then be applied to the landscape's material.
		landscapeMaterialTexture = new Texture2D (landscapeWidthSegments, landscapeLengthSegments, TextureFormat.RGB24, true);
		landscapeMaterialTexture.name = "LandscapeMaterialTexture";
		landscapeMaterialTexture.wrapMode = TextureWrapMode.Clamp;
		float landscapeMinHeight = minimumLandscapeHeight ();
		float landscapeMaxHeight = maximumLandscapeHeight ();
		for(int i = 0; i < landscapeWidthSegments; i++){
			for(int j = 0; j < landscapeLengthSegments; j++){
				//cycle through all of the landscape's vertices
				Vector3 p = LandscapePoint (i, j);	// get the landscape point
				//calculate the landscape point's relative height: where it lies between the current minimum ang maximum height of the landscape
				float landscapeRelativeHeight = map (p.y, landscapeMinHeight, landscapeMaxHeight, 0.0f, 1.0f);	// will receive a value 0.0f to 1.0f (relative to minimum to maximum)
				//use the 
				float cr = map (landscapeRelativeHeight, 0.0f, 1.0f, bottomColor.r, peakColor.r);	// set the color's red value accordingly
				float cg = map (landscapeRelativeHeight, 0.0f, 1.0f, bottomColor.g, peakColor.g);	// set the color's green value accordingly
				float cb = map (landscapeRelativeHeight, 0.0f, 1.0f, bottomColor.b, peakColor.b);	// set the color's blue value accordingly
				Color cc = new Color(cr,cg,cb,1.0f);
				landscapeMaterialTexture.SetPixel (i, j, cc);
			}
		}
		landscapeMaterialTexture.Apply ();
	}

	private void CreateLandscapeMaterial(){
		/*
		This function creates the 
		*/
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
		float x = map ((float)i, 0.0f, (float)landscapeLengthSegments, minimumX, maximumX);
		//calculate Z
		float minimumZ = -(landscapeWidth / 2.0f);
		float maximumZ = landscapeWidth / 2.0f;
		float z = map ((float)j, 0.0f, (float)landscapeWidthSegments, minimumZ, maximumZ );
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


	float map(float fromValue, float fromMin, float fromMax, float toMin, float toMax)
	{
		return toMin + (fromValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	float minimumLandscapeHeight(){
		float h = verticeList [0].y;
		for (int i = 0; i < verticeList.Count; i++) {
			if (verticeList [i].y < h)
				h = verticeList [i].y;
		}
		return h;
	}

	float maximumLandscapeHeight(){
		float h = verticeList [0].y;
		for (int i = 0; i < verticeList.Count; i++) {
			if (verticeList [i].y > h)
				h = verticeList [i].y;
		}
		return h;
	}


	bool UpdateVariableValueChange(){
		/*This function checks whether any of those variables has been changed, since the last frame.
		If ANY of them has been changed, then it returns TRUE. Otherwise it returns false.
		This Function is used within the Update
		How this is used: There are some things that must happen ONLY when one of these variables has changed.
		So with this function it is possible to check for value chabges.*/
		if(
			//check whether anything has changed...
			(layer1OffsetXPreviousValue != layer1OffsetX)		// 
			||(layer1OffsetZPreviousValue != layer1OffsetZ)		
			||(layer2OffsetXPreviousValue != layer2OffsetX)		
			||(layer2OffsetZPreviousValue != layer2OffsetZ)		
			||(layer3OffsetXPreviousValue != layer3OffsetX)		
			||(layer3OffsetZPreviousValue != layer3OffsetZ)		
			||(layer1ScalePreviousValue != layer1Scale)			// or layer1Scale has changed
			||(layer2ScalePreviousValue != layer2Scale)			// or layer2Scale has changed
			||(layer3ScalePreviousValue != layer3Scale)			// or layer3Scale has changed
			||(layer1HeightPreviousValue != layer1Height)		// or layer1Height has changed
			||(layer2HeightPreviousValue != layer2Height)		// or layer2Height has changed
			||(layer3HeightPreviousValue != layer3Height)		// or layer3Height has changed
			||(materialSmoothnessPreviousValue != materialSmoothness)
			||(materialMetallicnessPreviousValue != materialMetallicness)
			||(peakColorPreviousValue != peakColor)			
			||(bottomColorPreviousValue != bottomColor)
		){
			//if anything has changed, then set previous values to current values
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
		else return false;	//else return false...
	}
}
