/*	Author: Kostas Sfikas
	Date: April 2017
	Language: c#
	Platform: Unity 5.5.0 f3 (personal edition) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexObjectShapeController : MonoBehaviour {
	/* This class controls the behaviour of the Complex Object. The data it uses comes from the
	Procedural Audio Controller, so in a way this object is part of the Audio Controller's visualisation.
	It does not have an actual functional role in the project, it is just a visual aid to help the 
	user correlate som of the audio's behaviour to a visual analogue.*/

	public ProceduralAudioController audioController;
	public GameObject body;
	public GameObject upPart;
	public GameObject downPart;
	public GameObject leftPart;
	public GameObject rightPart;
	public GameObject frontPart;
	public GameObject backPart;

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
		float scale = mapValue (audioController.amplitudeModulationRangeOut, 0.0f, 1.0f, 1.5f, 2.5f);
		body.transform.localScale = new Vector3 (scale, scale, scale);

		float rotRang = audioController.frequencyModulationRangeOut * 360.0f * audioController.frequencyModulationOscillatorIntensity / 100.0f;
		upPart.transform.localRotation = Quaternion.Euler (0.0f, rotRang,0.0f);
		downPart.transform.localRotation = Quaternion.Euler (0.0f, -rotRang,0.0f);
		leftPart.transform.localRotation = Quaternion.Euler (rotRang, 0.0f,0.0f);
		rightPart.transform.localRotation = Quaternion.Euler (-rotRang, 0.0f,0.0f);
		frontPart.transform.localRotation = Quaternion.Euler (0.0f,0.0f,rotRang);
		backPart.transform.localRotation = Quaternion.Euler (0.0f,0.0f,-rotRang);

		float freqScaleXZ = mapValue ((float)audioController.mainFrequency, 100.0f, 2000.0f, 1.25f, 0.25f);
		float freqScaleY = mapValue ((float)audioController.mainFrequency, 100.0f, 2000.0f, 0.25f, 1.25f);
		upPart.transform.localScale = new Vector3 (freqScaleXZ,freqScaleY,freqScaleXZ);
		downPart.transform.localScale = new Vector3 (freqScaleXZ,freqScaleY,freqScaleXZ);
		leftPart.transform.localScale = new Vector3 (freqScaleY,freqScaleXZ,freqScaleXZ);
		rightPart.transform.localScale = new Vector3 (freqScaleY,freqScaleXZ,freqScaleXZ);
		frontPart.transform.localScale = new Vector3 (freqScaleXZ,freqScaleXZ,freqScaleY);
		backPart.transform.localScale = new Vector3 (freqScaleXZ,freqScaleXZ,freqScaleY);

		transform.Rotate(Vector3.right * Time.deltaTime * xRotFactor);
		transform.Rotate(Vector3.up * Time.deltaTime * yRotFactor);
		transform.Rotate(Vector3.forward * Time.deltaTime * zRotFactor);
	}



	float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
		/* This function maps (converts) a value from one range to another */
		return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}
}
