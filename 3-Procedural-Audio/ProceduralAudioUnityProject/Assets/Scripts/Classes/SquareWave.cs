using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SquareWave{
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
