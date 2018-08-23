
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Hayden Reeve
	File:			Camera.cs
	Description: 	Initial hierarchy for camera inheritance. Contains all the basic universal camera functions.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraParent : MonoBehaviour {

	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// DECLARATION

	// --- CAMERA RUMBLE
	[Header("Pirlean Rumble")]



	// --- POST PROCESSING
	[Header("Post Processing")]
	[SerializeField] protected float _flTransition = 10f;
	[SerializeField] protected float _flRumble = 1f;


	// --- TRANSITIONAL
	[Header("Transitional")]

	[HideInInspector] public int _itNewScene = 0;



	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// INTEGRATED

	// Use this for initialization
	protected virtual void StartAdditional () {



	}


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// RUMBLE

	protected Vector3 AddPirleanToVector(Vector3 v3) {
		v3 += new Vector3 (0.35f-Mathf.PerlinNoise (Random.insideUnitCircle.x,Random.insideUnitCircle.y), 0.35f-Mathf.PerlinNoise (Random.insideUnitCircle.x,Random.insideUnitCircle.y), 0f) * _flRumble;
		return v3;
	}


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */
	// TRANSITIONS

	// Fade the camera OUT.
	public IEnumerator FadeFromGame () {

		Time.timeScale = 1f;

		//while (true) {

		//	var ppBloom = _pp.bloom.settings;
		//	ppBloom.bloom.threshold = Mathf.Lerp (ppBloom.bloom.threshold, 0.1f, Time.deltaTime * _flTransition);
		//	ppBloom.bloom.radius = Mathf.Lerp (ppBloom.bloom.radius, 7f, Time.deltaTime * _flTransition);
		//	_pp.bloom.settings = ppBloom;

		//	if (ppBloom.bloom.threshold < 0.1f && ppBloom.bloom.radius > 6.9f)
		//		SceneManager.LoadScene (1);

		//	yield return null;

		//}

		SceneManager.LoadScene(_itNewScene);

		yield return null;

	}

	// Fade the camera IN.
	public IEnumerator FadeToGame () {

		Time.timeScale = 1f;

		//	bool isTransition = true;

		//	// Set up our modification variable...
		//	var ppBloom = _pp.bloom.settings;

		//	// Store it's original values...
		//	float flBloomIntensity = _pp.bloom.settings.bloom.intensity;
		//	float flBloomThreshhold = _pp.bloom.settings.bloom.threshold;
		//	float flBloomRadius = _pp.bloom.settings.bloom.radius;

		//	// Set the transition-in beginning values...
		//	ppBloom.bloom.intensity = 50f;
		//	ppBloom.bloom.threshold = 0.1f;
		//	ppBloom.bloom.radius = 7f;

		//	// And then until it's finished lerping to it's original values, continue lerping!
		//	while (isTransition) {

		//		ppBloom.bloom.intensity = Mathf.Lerp (ppBloom.bloom.intensity, flBloomIntensity, Time.deltaTime * _flTransition);
		//		ppBloom.bloom.threshold = Mathf.Lerp (ppBloom.bloom.threshold, flBloomThreshhold, Time.deltaTime * _flTransition * 0.1f);
		//		ppBloom.bloom.radius = Mathf.Lerp (ppBloom.bloom.radius, flBloomRadius, Time.deltaTime * _flTransition * 0.1f);
		//		_pp.bloom.settings = ppBloom;

		//		if (ppBloom.bloom.threshold == flBloomThreshhold && ppBloom.bloom.radius == flBloomRadius && ppBloom.bloom.intensity == flBloomIntensity)
		//			isTransition = false;

		//		yield return null;

		//	}

		yield return null;

	}

}