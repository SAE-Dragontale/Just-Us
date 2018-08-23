
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Hayden Reeve
	File:			CameraController.cs
	Description: 	This script controls the camera's position in relation to the player objects. This script must be applied to the camera object.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using UnityEngine;

public class CameraGameplay : CameraParent {

	// --- GAMEPLAY VARIABLES

	[Header("Camera Settings")]
	[SerializeField] public float _flCameraSpeed = 100f;

	private int _itPlayerNum;

	[Header("Noise Variables")]
	[SerializeField] public bool _isRumbling = false;
	[SerializeField] private float _flRumbleMax = 10f;
	[SerializeField] private float _flRumbleLerp = 1f;

	private float _flRumbling;

	[Header("Post Processing")]


	// Other Variables

	[HideInInspector] public bool _isEnding = false;
	//[HideInInspector] public int _itNewScene = 0;

	// --- COMPONENTS

	// External Components

	[Header("Player Objects")]

	[Tooltip("Link this to the grounding point of the arena.")]
	[SerializeField] public Transform _trFocalGround;

	[Tooltip("Link this to player one.")]
	[SerializeField] public Transform _trFocalAddOne;


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */


	// Use this for initialization
	void Start () {

		// Grounding Components.
		_trFocalGround = _trFocalGround.GetComponent<Transform> ();
		_trFocalAddOne = _trFocalAddOne.GetComponent<Transform> ();
		//_trFocalAddTwo = _trFocalAddTwo.GetComponent<Transform> ();

		// Fade into the game.
		StartCoroutine (FadeToGame());

	}

	// Update
	void Update () {

		if (_isEnding) {
			_isEnding = false;
			StartCoroutine (FadeFromGame ());
		}

	}

	// Update is called once per frame
	void LateUpdate () {

		Vector3 v3CameraPos = new Vector3 (0f,0f,0f);

		// Find the new position based on the tracked objects.

		if (_trFocalGround != null && _trFocalAddOne != null) {
			v3CameraPos.x = (((_trFocalGround.position.x * 2) + _trFocalAddOne.position.x) / 3f);
			v3CameraPos.y = (((_trFocalGround.position.y * 2) + _trFocalAddOne.position.y) / 3f);
		}

		// Append the Camera Shake effect if it's in use.

		// Lerp into, and out of, the rumble effect for added smoothness.
		if (_isRumbling) {
			_flRumbling = Mathf.Lerp (_flRumbling, _flRumbleMax, Time.deltaTime * _flRumbleLerp * 3f);
		} else if (_flRumbling > 0) {
			_flRumbling = Mathf.Lerp (_flRumbling, 0f, Time.deltaTime * _flRumbleLerp);
		}

		// If we're at a non-zero rumble, apply the shake to the camera's trajectory.
		if (_flRumbling != 0) {  v3CameraPos = AddPirleanToVector(v3CameraPos);  }

		// Lerp to the final position.
		transform.position = Vector3.Lerp (transform.position, new Vector3 (v3CameraPos.x, v3CameraPos.y, transform.position.z), Time.deltaTime * _flCameraSpeed);
	
	}

}