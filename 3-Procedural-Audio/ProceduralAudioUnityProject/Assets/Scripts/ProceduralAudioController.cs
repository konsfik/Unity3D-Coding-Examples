/*	Author: Kostas Sfikas
	Date: April 2017
	Language: c#
	Platform: Unity 5.5.0 f3 (personal edition) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof (AudioSource))]
public class ProceduralAudioController : MonoBehaviour {
	/* This class is the main audio engine, 
	- It uses the OnAudioFilterRead() function to create sound by applying mathematical functions
	on each separate audio sample.
	- It uses the SawWave, SinusWave and SquareWave classes to produce the audio waves, 
	as well as the Frequency and Amplitude Modulations. 
	- This class (as well as the related classes) has not been optimized for performance. Therefore 
	it is not recommended to use multiple instance of this class, because you may have performance issues.*/

	SawWave sawAudioWave;
	SquareWave squareAudioWave;
	SinusWave sinusAudioWave;

	SinusWave amplitudeModulationOscillator;
	SinusWave frequencyModulationOscillator;

	public bool autoPlay;

	[Header("Volume / Frequency")]
	[Range(0.0f,1.0f)]
	public float masterVolume = 0.5f;
	[Range(100,2000)]
	public double mainFrequency = 500;
	[Space(10)]

	[Header("Tone Adjustment")]
	public bool useSinusAudioWave;
	[Range(0.0f,1.0f)]
	public float sinusAudioWaveIntensity = 0.25f;
	[Space(5)]
	public bool useSquareAudioWave;
	[Range(0.0f,1.0f)]
	public float squareAudioWaveIntensity = 0.25f;
	[Space(5)]
	public bool useSawAudioWave;
	[Range(0.0f,1.0f)]
	public float sawAudioWaveIntensity = 0.25f;

	[Space(10)]

	[Header("Amplitude Modulation")]
	public bool useAmplitudeModulation;
	[Range(0.2f,30.0f)]
	public float amplitudeModulationOscillatorFrequency = 1.0f;
	[Header("Frequency Modulation")]
	public bool useFrequencyModulation;
	[Range(0.2f,30.0f)]
	public float frequencyModulationOscillatorFrequency = 1.0f;
	[Range(1.0f,100.0f)]
	public float frequencyModulationOscillatorIntensity = 10.0f;

	[Header("Out Values")]
	[Range(0.0f,1.0f)]
	public float amplitudeModulationRangeOut;
	[Range(0.0f,1.0f)]
	public float frequencyModulationRangeOut;


	float mainFrequencyPreviousValue;
	private System.Random RandomNumber = new System.Random();

	private double sampleRate;	// samples per second
	//private double myDspTime;	// dsp time
	private double dataLen;		// the data length of each channel
	double chunkTime;			
	double dspTimeStep;
	double currentDspTime;

	void Awake(){
		sawAudioWave = new SawWave ();
		squareAudioWave = new SquareWave ();
		sinusAudioWave = new SinusWave ();

		amplitudeModulationOscillator = new SinusWave ();
		frequencyModulationOscillator = new SinusWave ();

		sampleRate = AudioSettings.outputSampleRate;
	}

	void Update(){
		if (autoPlay) {
			if(!useSinusAudioWave){
				useSinusAudioWave = true;
			}
			if(!useSquareAudioWave){
				useSquareAudioWave = true;
			}
			if(!useSawAudioWave){
				useSawAudioWave = true;
			}
			if(!useAmplitudeModulation){
				useAmplitudeModulation = true;
			}
			if(!useFrequencyModulation){
				useFrequencyModulation = true;
			}
			mainFrequency = Mathf.PingPong (Time.time *200.0f, 1900.0f) + 100.0f;
			sinusAudioWaveIntensity = Mathf.PingPong (Time.time * 0.5f, 1.0f);
			squareAudioWaveIntensity = Mathf.PingPong (Time.time * 0.6f, 1.0f);
			sawAudioWaveIntensity = Mathf.PingPong (Time.time * 0.7f, 1.0f);
			amplitudeModulationOscillatorFrequency = Mathf.PingPong (Time.time * 3.0f, 30.0f);
			frequencyModulationOscillatorFrequency = Mathf.PingPong (Time.time * 4.0f, 30.0f);
			frequencyModulationOscillatorIntensity = Mathf.PingPong (Time.time * 10.0f, 100.0f);
		}
	}

	void OnAudioFilterRead(float[] data, int channels){
		/* This is called by the system
		suppose: sampleRate = 48000
		suppose: data.Length = 2048
		suppose: channels = 2
		then:
		dataLen = 2048/2 = 1024
		chunkTime = 1024 / 48000 = 0.0213333... so the chunk time is around 21.3 milliseconds.
		dspTimeStep = 0.0213333 / 1024 = 2.083333.. * 10^(-5) = 0.00002083333..sec = 0.02083 milliseconds
			keep note that 1 / dspTimeStep = 48000 ok!		
		*/

		currentDspTime = AudioSettings.dspTime;
		dataLen = data.Length / channels;	// the actual data length for each channel
		chunkTime = dataLen / sampleRate;	// the time that each chunk of data lasts
		dspTimeStep = chunkTime / dataLen;	// the time of each dsp step. (the time that each individual audio sample (actually a float value) lasts)

		double preciseDspTime;
		for (int i = 0; i < dataLen; i++)	{ // go through data chunk
			preciseDspTime = currentDspTime +  i * dspTimeStep;
			double signalValue = 0.0;
			double currentFreq = mainFrequency;
			if (useFrequencyModulation) {
				double freqOffset = (frequencyModulationOscillatorIntensity * mainFrequency * 0.75) / 100.0;
				currentFreq += mapValueD (frequencyModulationOscillator.calculateSignalValue (preciseDspTime, frequencyModulationOscillatorFrequency), -1.0, 1.0, -freqOffset, freqOffset);
				frequencyModulationRangeOut = (float)frequencyModulationOscillator.calculateSignalValue (preciseDspTime, frequencyModulationOscillatorFrequency) * 0.5f + 0.5f;
			} else {
				frequencyModulationRangeOut = 0.0f;
			}

			if (useSinusAudioWave) {
				signalValue += sinusAudioWaveIntensity * sinusAudioWave.calculateSignalValue (preciseDspTime, currentFreq);
			}
			if (useSawAudioWave) {
				signalValue += sawAudioWaveIntensity * sawAudioWave.calculateSignalValue (preciseDspTime, currentFreq);
			}
			if (useSquareAudioWave) {
				signalValue += squareAudioWaveIntensity * squareAudioWave.calculateSignalValue (preciseDspTime, currentFreq);
			}

			if (useAmplitudeModulation) {
				signalValue *= mapValueD (amplitudeModulationOscillator.calculateSignalValue (preciseDspTime, amplitudeModulationOscillatorFrequency), -1.0, 1.0, 0.0, 1.0);
				amplitudeModulationRangeOut = (float)amplitudeModulationOscillator.calculateSignalValue (preciseDspTime, amplitudeModulationOscillatorFrequency) * 0.5f + 0.5f;
			} else {
				amplitudeModulationRangeOut = 0.0f;
			}

			float x = masterVolume * 0.5f * (float)signalValue;

			for (int j = 0; j < channels; j++) {
				data[i * channels + j] = x;
			}
		}

	}

	float mapValue(float referenceValue, float fromMin, float fromMax, float toMin, float toMax) {
		/* This function maps (converts) a Float value from one range to another */
		return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}

	double mapValueD(double referenceValue, double fromMin, double fromMax, double toMin, double toMax) {
		/* This function maps (converts) a Double value from one range to another */
		return toMin + (referenceValue - fromMin) * (toMax - toMin) / (fromMax - fromMin);
	}
}
