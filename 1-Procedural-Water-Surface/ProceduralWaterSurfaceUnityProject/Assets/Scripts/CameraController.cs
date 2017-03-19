using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Source Code written by Kostas Sfikas, March 2017
Tested with Unity 5.5.0 f3 Pesonal edition*/

public class CameraController : MonoBehaviour {
	/* This class takes care of the camera's movement in the scene. */
	Vector3 camPos;	// vector that will set the camera's position
	float localTime;
	float localTimeScale;
	float routeRadius;
	float cameraHeight;
	// Use this for initialization
	void Start () {
		localTime = 0.0f;
		localTimeScale = 0.15f;
		routeRadius = 10.0f;
		cameraHeight = 9.0f;
	}
	
	// Update is called once per frame
	void Update () {
		localTime += Time.deltaTime * localTimeScale;
		float x = Mathf.Sin (localTime) * routeRadius;
		float z = Mathf.Cos (localTime) * routeRadius;
		float y = cameraHeight;
		camPos = new Vector3 (x, y, z);
		transform.position = camPos;
		transform.LookAt (new Vector3(0.0f,0.0f,0.0f));
	}
}
