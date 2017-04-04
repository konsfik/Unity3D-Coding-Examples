using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudedObjectShapeController : MonoBehaviour {
	public ProceduralAudioController audioController;
	public GameObject body;
	public GameObject upPart;
	public GameObject downPart;
	public GameObject leftPart;
	public GameObject rightPart;
	public GameObject frontPart;
	public GameObject backPart;

	// Use this for initialization
	void Start () {
		
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
	}



	float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
		/* This function maps (converts) a value from one range to another */
		return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}
}
