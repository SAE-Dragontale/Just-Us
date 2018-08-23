
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
  	Author: 		Hayden Reeve, Aeisha Aziz-Wilkinson
	File:			MainMenuController.cs
	Description: 	This script controls all the neccissary functions for the Main Menu.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

public class CameraMainMenu : CameraParent {


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// DECLARATION

	// --- MOVEMENT
	[Header("Movement Settings")]

	[Tooltip("This list contains all possible movement locations. Order is important. Use Empty Game Objects to place checkpoints around the scene.")]
	[SerializeField] Transform[] _atrCheckpoints;

	[SerializeField] private float _flCameraSpeed = 10f;
	[SerializeField] private float _flAcceleration = 5f;

	[SerializeField] private float _flCameraSpeedCurrent;
	[SerializeField] private float _flCameraSpeedModifier;

	// --- DESTINATION
	[Header("Destination Settings")]

	[SerializeField] private int _itStartingLocale = 2;
	[SerializeField] private float _flDistanceMax;
	[SerializeField] private Vector3 _v3Destination;

	// --- GAME SETTINGS
	[Header("Game Settings")]

	[SerializeField] private string _stPlayerMode = "PlayerMode";
	[HideInInspector] private int _itPlayerOne = 1;

	// --- MENU
	[Header("Menu Specific")]

	[SerializeField] private Image _imPlayerOne;
	[SerializeField] private Image _imPlayerTwo;


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// INTEGRATED

	// Initialisation.
	void Start () {

		// Get Components
		transform.position = _atrCheckpoints [_itStartingLocale-1].position;
		CameraToLocation (_itStartingLocale);

	}

	// On Frame.
	void Update () {

		// Make only the closest menu to the camera active. We don't want to accidentally have the player click elsewhere.
		bool hasFound = false;

		foreach (Transform tr in _atrCheckpoints) {

			if ( (tr.position.z + 100f) > transform.position.z && !hasFound) {
				tr.parent.gameObject.SetActive (true);
				hasFound = true;
			} else {
				tr.parent.gameObject.SetActive (false);
			}

		}

		// If we're at a specific position, we should run the appropriate functions.
		PosCredits ();

	}

	// For Camera Movement.
	void LateUpdate () {

		// Transform the camera to the current destination.
		Vector3 v3CameraPos = _v3Destination;
		float flDistanceCurrent = Vector3.Distance (transform.position, _v3Destination);

		if (flDistanceCurrent != 0 && _flDistanceMax != 0)
			_flCameraSpeedModifier = Mathf.Clamp (flDistanceCurrent / _flDistanceMax, -50f, 50f);
		else
			_flCameraSpeedModifier = 0f;

		_flCameraSpeedCurrent = Mathf.Max(Mathf.Lerp (_flCameraSpeedCurrent, _flCameraSpeed * _flCameraSpeedModifier, Time.deltaTime * _flAcceleration), 0f);

		// Apply the shake to the camera's trajectory.
		v3CameraPos = AddPirleanToVector(v3CameraPos);

		// Lerp to the final position.
		transform.position = Vector3.Lerp (transform.position, v3CameraPos, Time.deltaTime * _flCameraSpeedCurrent);

	}


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// MENU POSITION

	/// <summary>
	/// Find the new destination of the camera.
	/// </summary>
	/// <param name="itLocale">The destination location's number in the checkpoints array.</param>

	public void CameraToLocation (int itLocale) {

		if (Vector3.Distance (_v3Destination, _atrCheckpoints [itLocale].position) != 0)
			_flDistanceMax = Vector3.Distance (_v3Destination, _atrCheckpoints [itLocale].position);

		_v3Destination = _atrCheckpoints [itLocale].position;

	}

	/// <summary>
	/// Monitors the player's input while they're on the credits screen, so that they can exit on any button input.
	/// </summary>

	void PosCredits () {

		if (Input.anyKeyDown && _atrCheckpoints[0].parent.gameObject.activeInHierarchy) {
			_v3Destination = _atrCheckpoints [_itStartingLocale].position;
		}

	}


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// MENU BUTTONS

	/// <summary>
	/// Swap the Character Images on the Screen.
	/// </summary>

	public void SwapCharacters () {

		// Swap the sprites of the images
		Sprite imTemp = _imPlayerOne.sprite;
		_imPlayerOne.sprite = _imPlayerTwo.sprite;
		_imPlayerTwo.sprite = imTemp;
		
	}

	/// <summary> 
	/// Set the player mode and start the game! 
	/// </summary>

	public void StartGame (int itPlayerMode) {
		PlayerPrefs.SetInt (_stPlayerMode, itPlayerMode);
		_itNewScene = 1;
		StartCoroutine (FadeFromGame());
	}

	/// <summary> 
	/// Exit the game. 
	/// </summary>

	public void ExitGame () {

		Application.Quit ();

		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#endif

	}

}
