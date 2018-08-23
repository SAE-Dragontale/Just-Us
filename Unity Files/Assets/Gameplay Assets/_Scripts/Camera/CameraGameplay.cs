
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Hayden Reeve
	File:			CameraController.cs
	Description: 	This script controls the camera's position in relation to the player objects. This script must be applied to the camera object.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

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

		// Get Post Processing initial settings, then replace the instance with a modifiable duplicate.
		var ppOriginal = GetComponent<PostProcessingBehaviour> ();

		_pp = Instantiate (ppOriginal.profile);
		ppOriginal.profile = _pp;

		// Fade into the game.
		StartCoroutine (FadeToGame());

	}

	// Update
	void Update () {

		if (_isEnding) {
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


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

	// Fade the camera OUT.
	public IEnumerator FadeFromGame () {

		Time.timeScale = 1f;

		bool isTransition = true;

		while (isTransition) {

			var ppBloom = _pp.bloom.settings;
			ppBloom.bloom.intensity = Mathf.Lerp (ppBloom.bloom.intensity, 50f, Time.deltaTime * _flTransition);
			ppBloom.bloom.threshold = Mathf.Lerp (ppBloom.bloom.threshold, 0f, Time.deltaTime * _flTransition);
			ppBloom.bloom.radius = Mathf.Lerp (ppBloom.bloom.radius, 7f, Time.deltaTime * _flTransition);
			_pp.bloom.settings = ppBloom;

			if (ppBloom.bloom.threshold < 0.1f && ppBloom.bloom.radius > 6.9f && ppBloom.bloom.intensity > 49f) {
				isTransition = false;
			}

			yield return new WaitForEndOfFrame ();

		}

		SceneManager.LoadScene (_itNewScene);

	}

	// Fade the camera IN.
	public IEnumerator FadeToGame () {

		Time.timeScale = 1f;

		bool isTransition = true;

		// Set up our modification variable...
		var ppBloom = _pp.bloom.settings;

		// Store it's original values...
		float flBloomIntensity = _pp.bloom.settings.bloom.intensity;
		float flBloomThreshhold = _pp.bloom.settings.bloom.threshold;
		float flBloomRadius = _pp.bloom.settings.bloom.radius;

		// Set the transition-in beginning values...
		ppBloom.bloom.intensity = 50f;
		ppBloom.bloom.threshold = 0f;
		ppBloom.bloom.radius = 7f;

		// And then until it's finished lerping to it's original values, continue lerping!
		while (isTransition) {
			
			ppBloom.bloom.intensity = Mathf.Lerp (ppBloom.bloom.intensity, flBloomIntensity, Time.deltaTime * _flTransition);
			ppBloom.bloom.threshold = Mathf.Lerp (ppBloom.bloom.threshold, flBloomThreshhold, Time.deltaTime * _flTransition * 0.1f);
			ppBloom.bloom.radius = Mathf.Lerp (ppBloom.bloom.radius, flBloomRadius, Time.deltaTime * _flTransition * 0.1f);
			_pp.bloom.settings = ppBloom;

			if (ppBloom.bloom.threshold == flBloomThreshhold && ppBloom.bloom.radius == flBloomRadius && ppBloom.bloom.intensity == flBloomIntensity)
				isTransition = false;

			yield return new WaitForEndOfFrame();

		}
		
	}

}