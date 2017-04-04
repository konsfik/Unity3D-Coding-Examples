/*	Author: Kostas Sfikas
	Date: April 2017
	Language: c#
	Platform: Unity 5.5.0 f3 (personal edition) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SquareWave{
	/* This class creates an approximation of a Square Wave, by using harmonics of a sinus wave.
	This way the actual shape of the wave is smoother than an absolute square shape, thus reducing
	some of the noise this would create.
	The levels of each harmonic are stored in the squareHarmonicLevels Array */
	double[] squareHarmonicLevels = new double[] 
	{
		1.0, 		// level of 1st harmonic
		0.0,		// level of 2nd harmonic
		0.33333333,	// level of 3rd harmonic
		0.0,		// level of 4th harmonic
		0.2,		// level of 5th harmonic
		0.0,		// level of 6th harmonic
		0.14285714,	// level of 7th harmonic
		0.0,		// level of 8th harmonic
		0.11111111,	// level of 9th harmonic
		0.0,		// level of 10th harmonic
		0.09090909,	// level of 11th harmonic
		0.0,		// level of 12th harmonic
		0.07692308,	// level of 13th harmonic
		0.0,		// level of 14th harmonic
		0.06666666,	// level of 15th harmonic
		0.0			// level of 16th harmonic
	};

	SinusWave[] subSignals;

	public SquareWave(){
		subSignals = new SinusWave[16];
		for (int i = 0; i < 16; i++) {
			subSignals [i] = new SinusWave ();
		}
	}

	public double calculateSignalValue(double newSignalTime, double newSignalFrequency){
		double signalSum = 0.0;
		for (int i = 0; i < subSignals.Length; i++) {
			signalSum += squareHarmonicLevels[i] * subSignals [i].calculateSignalValue (newSignalTime, newSignalFrequency * (i+1));
		}
		return signalSum;
	}
}
