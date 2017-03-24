/*	Author: Kostas Sfikas
	Date: March 2017
	Language: C#
	Platform: Unity 5.5.0 f3 (personal edition) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	/* This class takes care of the camera's movement in the scene.
	More specifically: The camera is always looking towards a point that is fixed in its place.
	This point is represented by the lookPoint Vector3 variable.
	The camera itself is rotating around that point's vertical axis at a steady distance 
	which is equal to the cameraMovementRadius variable, and at a fixed height which is 
	equal to the cameraHeight variable. */

	//variable declaration
	Vector3 lookPoint;				// the point towards which the camera is looking
	float cameraMovementRadius;		// the radius of the camera's movement
	float cameraHeight;				// the camera's height (fixed)
	float myTime;					// the camera's local time variable (is manually updated)
	float rotationSpeed;			// the camera's rotation speed (in cycles per second) - can also be negative

	// Use this for initialization
	void Awake () {

		//initialize variables
		lookPoint = new Vector3 (0.0f, -500.0f, 0.0f);	// set the point where the camera is looking at
		cameraMovementRadius = 4000.0f;							//	set the radius of the camera's movement
		cameraHeight = 2800.0f;
		myTime = 0.0f;
		rotationSpeed = 0.01f;	// 0.01 cycles per second -> 1 cycle per 100 seconds
	}

	// Update is called once per frame
	void Update () {
		myTime += Time.deltaTime;// (float)Time.time - startTime;
		float timeCycle = 2.0f*Mathf.PI*myTime*rotationSpeed;
		float x = cameraMovementRadius * Mathf.Sin (timeCycle);
		float y = cameraHeight;
		float z = cameraMovementRadius * Mathf.Cos (timeCycle);
		Vector3 p = new Vector3 (x, y, z);
		transform.position = p;
		transform.LookAt (lookPoint);
	}
}
