using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	Vector3 lookPoint;
	Vector3 camPos;
	// Use this for initialization
	void Start () {
		lookPoint = new Vector3 (0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		float xx = Mathf.Sin (Time.time/10.0f) * 2.5f;
		float zz = Mathf.Cos (Time.time/10.0f) * 2.5f;
		float yy = Mathf.Sin (Time.time/4.0f) + 1.0f;
		camPos = new Vector3 (xx, yy, zz);
		transform.position = camPos;
		transform.LookAt (lookPoint);
	}
}
