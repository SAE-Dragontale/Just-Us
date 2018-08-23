
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Eric Cox
	File:			PauseController.cs
	Description: 	functionality for pause. 

	Contributor:	Hayden Reeve
	Description:	Scene Switching.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseController : MonoBehaviour {

	Scene thisScene;
	[SerializeField] CameraGameplay ourGameCamera;

	[Header("Player References")]
	[SerializeField] int numOfPlayers;

	[Header("User Interface")]
	[SerializeField] GameObject pauseUI;
	[SerializeField] GameObject optionsPanel;
	[SerializeField] Dropdown dropDownP1;
	[SerializeField] Dropdown dropDownP2;
	[SerializeField] Slider sfxSlider;
	[SerializeField] Slider musicSlider;

	[SerializeField] bool gamePaused;

	[Header("Audio Settings")]
	[SerializeField] AudioMixer gameVolumeMixer;

	void Start()
	{

		thisScene = SceneManager.GetActiveScene();

		if (!PlayerPrefs.HasKey ("PlayerMode")) 
		{
			PlayerPrefs.SetInt ("PlayerMode", 1);
		}
		numOfPlayers = PlayerPrefs.GetInt ("PlayerMode");

		if (numOfPlayers == 1) 
		{			
			dropDownP2.interactable = false;
		}
		else 
		{
			dropDownP2.interactable = true;
		}


		//Controller config
		//player 1
		PlayerPrefs.GetInt ("OptionController" + 1);
		//player 2
		PlayerPrefs.GetInt ("OptionController" + 2);

		dropDownP1.value = PlayerPrefs.GetInt ("OptionController" + 1);
		dropDownP2.value = PlayerPrefs.GetInt ("OptionController" + 2);

		sfxSlider.value = PlayerPrefs.GetFloat ("SFXVolume");
		musicSlider.value = PlayerPrefs.GetFloat ("MusicVolume");
	}

	void Update()
	{
		if (Input.GetKeyDown ("escape") && thisScene.name == "Gameplay") 
		{
			TogglePause ();
		}
	}

	//toggle pause function, which detects the time scale and displays UI, and pauses audio appropriately.
	public void TogglePause()
	{
		gamePaused = !gamePaused;
		if (Time.timeScale == 0) {
			pauseUI.SetActive (false);
			optionsPanel.SetActive (false);	
			Time.timeScale = 1;
		} else {
			pauseUI.SetActive (true);
			Time.timeScale = 0;
		}
	}

	public void Menu()
	{
		ourGameCamera._isEnding = true;
		ourGameCamera._itNewScene = 0;
		CloseAllUI ();
	}

	public void Options()
	{
		pauseUI.SetActive (false);
		optionsPanel.SetActive (true);			
	}

	public void OptionsLeave()
	{
		optionsPanel.SetActive (false);
		pauseUI.SetActive (true);

		PlayerPrefs.Save ();
	}

	public void CloseAllUI()
	{
		optionsPanel.SetActive (false);
		pauseUI.SetActive (false);
		gameObject.GetComponent<GameController> ().deathPanel.SetActive (false);
	}

	public void Restart()
	{
		ourGameCamera._isEnding = true;
		ourGameCamera._itNewScene = SceneManager.GetActiveScene().buildIndex;
		CloseAllUI ();
	}

	public void SetSFXVolume(float sfxLvl)
	{
		sfxLvl = sfxSlider.value;
		gameVolumeMixer.SetFloat ("sfxVolume", sfxLvl);
		PlayerPrefs.SetFloat ("SFXVolume", sfxSlider.value);
	}

	public void SetMusicVolume(float musicLvl)
	{
		musicLvl = musicSlider.value;
		gameVolumeMixer.SetFloat ("musicVolume", musicLvl);
		PlayerPrefs.SetFloat ("MusicVolume", musicSlider.value);

	}

	public void ControllerSelectP1()
	{
		// 0 is Keyboard, 1 is Gamepad.
		//drop down if statements for Player 1
		if (dropDownP1.value == 0) 
		{
			//PP set int() of key name "OptionController1", to 0 
			PlayerPrefs.SetInt ("OptionController" + 1, 0);
			Debug.Log("Player 1 controls are now keyboard");
		}
		if (dropDownP1.value == 1) 
		{
			//PP set int of key name "OptionController1", to 1 
			PlayerPrefs.SetInt ("OptionController" + 1, 1);
			Debug.Log("Player 1 controls are now gamepad 1");
		}
		if (dropDownP1.value == 2) 
		{
			PlayerPrefs.SetInt ("OptionController" + 1, 2);
			Debug.Log("Player 1 controls are now gamepad 2");
		}
		PlayerPrefs.Save ();
	}

	public void ControllerSelectP2()
	{
		//drop down if statements for Player 2
		if (dropDownP2.value == 0) 
		{
			PlayerPrefs.SetInt ("OptionController" + 2, 0);
			Debug.Log ("Player 2 controls are now keyboard");
		}
		if (dropDownP2.value == 1) 
		{
			PlayerPrefs.SetInt ("OptionController" + 2, 1);
			Debug.Log ("Player 2 controls are now gamepad 1");
		}
		if (dropDownP1.value == 2) 
		{
			PlayerPrefs.SetInt ("OptionController" + 2, 2);
			Debug.Log("Player 1 controls are now gamepad 2");
		}
		PlayerPrefs.Save ();
	}

}
