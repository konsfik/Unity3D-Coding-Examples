using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour {
	float xRotFactor;
	float yRotFactor;
	float zRotFactor;
	// Use this for initialization
	void Start () {
		xRotFactor = Random.Range (-50.0f, 50.0f);
		yRotFactor = Random.Range (-50.0f, 50.0f);
		zRotFactor = Random.Range (-50.0f, 50.0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.right * Time.deltaTime * xRotFactor);
		transform.Rotate(Vector3.up * Time.deltaTime * yRotFactor);
		transform.Rotate(Vector3.forward * Time.deltaTime * zRotFactor);
	}
}
