using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnvironmentController : MonoBehaviour {
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
