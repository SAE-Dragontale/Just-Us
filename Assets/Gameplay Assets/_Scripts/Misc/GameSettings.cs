
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Hayden Reeve
	File:			GameSettings.cs
	Description: 	This script determines the player's personal settings, such as controller input and music volumes.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

	// --- Public Variables

	// Labels
	private string _stOptionMusic = "OptionMusic";
	private string _stOptionSFX = "OptionController";
	private string _stOptionController = "OptionController";

	// Variables
	private float _flOptionMusicDefault = 100f;
	private float _flOptionSFXDefault = 100f;
	private int _itController = 0; // 0 is Keyboard, 1 is Gamepad.

	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// Use this to set the default variables for settings if none are present.

	void Awake () {

		if (!PlayerPrefs.HasKey (_stOptionMusic)) {
			PlayerPrefs.SetFloat (_stOptionMusic, _flOptionMusicDefault);
		}

		if (!PlayerPrefs.HasKey (_stOptionSFX)) {
			PlayerPrefs.SetFloat (_stOptionSFX, _flOptionSFXDefault);
		}

		if (!PlayerPrefs.HasKey (_stOptionController)) {
			PlayerPrefs.SetInt (_stOptionController, _itController);
		}

	}

	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// Use these functions to control the player's settings through the option screens.

	void Update () {

	}

}
