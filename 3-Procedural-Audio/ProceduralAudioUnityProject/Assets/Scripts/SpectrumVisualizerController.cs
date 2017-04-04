/*	Author: Kostas Sfikas
	Date: April 2017
	Language: c#
	Platform: Unity 5.5.0 f3 (personal edition) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumVisualizerController : MonoBehaviour {
	public GameObject spectrumCubePrefab;
	public AudioSource audioSource;
	List<GameObject> cubes;
	// Use this for initialization
	void Start () {
		cubes = new List<GameObject> ();
		for (int i = 0; i < 180; i++) {
			float angle = (2.0f * i / 360.0f) * 2.0f * Mathf.PI;
			float radius = 2.0f;
			float x = Mathf.Sin (angle) * radius;
			float z = Mathf.Cos (angle) * radius;
			float y = 0;
			Vector3 cubePos = new Vector3 (x, y, z);
			//Quaternion cubeRotation = new Quaternion(0.0f,i*5.0f,0.0f,0.0f);
			GameObject instance = Instantiate (spectrumCubePrefab, cubePos, Quaternion.identity);
			Quaternion rot = Quaternion.Euler(0.0f, i * 2.0f, 0.0f);
			instance.transform.rotation = rot;
			instance.transform.parent = this.transform;
			cubes.Add (instance);
		}
	}
	
	// Update is called once per frame
	void Update () {
		float[] spectrum = audioSource.GetSpectrumData (2048, 0, FFTWindow.Hanning);
		for (int i = 0; i <cubes.Count; i++) {
			Vector3 prevScale = cubes [i].transform.localScale;
			prevScale.y = spectrum [i] * 16.0f + 0.01f;
			cubes [i].transform.localScale = prevScale;
			Vector3 prevPos = cubes [i].transform.position;
			prevPos.y = prevScale.y / 2.0f;
			cubes [i].transform.position = prevPos;
		}
	}
}
