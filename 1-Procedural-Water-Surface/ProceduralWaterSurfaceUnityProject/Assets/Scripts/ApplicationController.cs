using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Source Code written by Kostas Sfikas, March 2017
Tested with Unity 5.5.0 f3 Pesonal edition*/

public class ApplicationController : MonoBehaviour {
	/* This class exists only to end the application when the escape key is pressed.
	Will not work in the editor mode (you can hit the play button to start and
	stop the application while in editor mode), but it will work if you make a build
	of the application on your windows / mac / linux computer. */

	void Start () {
		
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){ //check if the Esc key has been pressed
			Application.Quit();	//if the Esc key has been pressed, quit the application
		}
	}
}
