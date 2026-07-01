/*	Author: Kostas Sfikas
	Date: April 2017
	Language: c#
	Platform: Unity 5.5.0 f3 (personal edition) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SinusWave{
	
	double currentSignalTime;			// the current dsp time
	double currentSignalFrequency;		// the current frequency of the signal
	double currentSignalPhaseOffset;	// the current phase offset of the signal

	double currentSignaOutlValue;		//the current signal output value		

	public SinusWave(){
		currentSignalFrequency = 400.0;
		currentSignalPhaseOffset = 0.0;
	}

	public double calculateSignalValue(double newSignalTime, double newSignalFrequency){

		if (currentSignalFrequency != newSignalFrequency) {
			/*This part takes care of what should happen when the signal's frequency changes
			(when the incoming frequency is different from the current frequency)
			 This is VERY IMPORTANT, because if you do not handle this matter then you will hear
			 LOTS of CLICKS when the frequency changes.

			Description: When the frequency changes, it is driven either by a slider, or by an
			external signal. Either way, the change happens in (very small) increments. When such
			a change takes place, then the produced wave suddenly changes phase.
			To make this more clear, suppose that there is a change from 
			Sin(2 * PI * 0.75) = -1 
			to Sin(2 * PI * 0.8) = -0.951056516295
			This sudden change from -1 to  -0.951056516295 causes a discontinuity in the sinusoidal function's graph, and this can be VERY bad.
			The way I handled this is:
			Every time the frequency changes, the phase of the wave is shifted in such a way that the current value of the sinusoidal 
			function remains the same between the two steps. This way, no matter how quickly the frequency changes, no clicks are ever 
			heard (because of that, at least). 
			This is kind of complicated, and very low-level audio stuff, so if you do not understand it, you may just use it. */

			// calculate the signal's current period: period = 1 / frequency
			double currentSignalPeriod = 1.0 / currentSignalFrequency;
			// calculate the current number of cycles:
			// the number of cycles is the number of times that a complete period of the wave has occured.
			double currentNumberOfSignalCycles = (currentSignalTime / currentSignalPeriod) + (currentSignalPhaseOffset / (2.0 * Math.PI));
			double currentSignalCyclePosition = currentNumberOfSignalCycles % 1.0;	//current cycle position

			double newSignalPeriod = 1.0 / newSignalFrequency;
			double newNumberOfSignalCycles = currentSignalTime / newSignalPeriod;
			double newSignalCyclePosition = newNumberOfSignalCycles % 1.0;			//new cycle position

			double cycleDifference = currentSignalCyclePosition - newSignalCyclePosition;
			double phaseDifference = cycleDifference * Math.PI * 2.0;

			currentSignalPhaseOffset = phaseDifference;

			currentSignalFrequency = newSignalFrequency;
			currentSignalTime = newSignalTime;

			currentSignaOutlValue = Math.Sin (currentSignalTime * 2.0 * Math.PI * currentSignalFrequency + currentSignalPhaseOffset);
			return currentSignaOutlValue;
		} else {
			currentSignalFrequency = newSignalFrequency;
			currentSignalTime = newSignalTime;
			currentSignaOutlValue = Math.Sin (currentSignalTime * 2.0 * Math.PI * currentSignalFrequency + currentSignalPhaseOffset);
			return currentSignaOutlValue;
		}
	}

}