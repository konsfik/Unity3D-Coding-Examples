/*	Author: Kostas Sfikas
	Date: March 2017
	Language: C#
	Platform: Unity 5.5.0 f3 (personal edition) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnvironmentController : MonoBehaviour {
	/* This class is of minor importance. It takes the bottomColor from the ProceduralLandscapeController
	and applies it to the color of another material. This is used in the scene to make the color of some 
	decorative objects the same as the landscape's bottom color, clearly for "aesthetic" reasons. */

	public Material lineMaterial;
	public ProceduralLandscapeController landscapeController;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		lineMaterial.color = landscapeController.bottomColor;
	}
}
