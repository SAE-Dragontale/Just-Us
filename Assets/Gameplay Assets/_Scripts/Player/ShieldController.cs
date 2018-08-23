
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Hayden Reeve
	File:			ShieldController.cs
	Description: 	This script controls the player object.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class ShieldController : MonoBehaviour {

	// --- GAMEPLAY VARIABLES

	[Header("Entity Settings")]

	[Tooltip("The health of the player Shield.")]
	[SerializeField] private int _itShielding = 3;
	[SerializeField] private int _itShieldCur;

	[Tooltip("You cannot take damage for X seconds after being hit.")]
	[SerializeField] private float _flShieldInvuln = 0.25f;

	[Tooltip("The time it takes for the shield to begin recharging.")]
	[SerializeField] public float _flRechargeWait = 10f;

	[Tooltip("The time it takes to recharge a single point of shield AFTER the shield has begun recharging.")]
	[SerializeField] private float _flChargingWait = 1f;

	[Header("Rumble Settings")]
	[SerializeField] private float _flRumbleOnHit = 0.05f;
	[SerializeField] private float _flRumbleControllerOnHit = 0.1f;

	// --- CONTROLLER VARIABLES

	private bool _isInvuln;

	// --- COMPONENTS

	[Header("Components")]
	[SerializeField] private Transform _trHighlight;

	[Header("Shield Health UI")]
	[SerializeField] private Text _txShield;
	[SerializeField] private Slider _slShieldBar;


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */


	// When the shield is activated, refresh all current variables.
	void OnEnable () {

		_itShieldCur = _itShielding;
		_isInvuln = false;

		// Refresh the Coroutines and begin powering up the shield.
		StopAllCoroutines ();
		StartCoroutine (ShieldInvuln ());
		StartCoroutine (Recharge (0f));

	}

	// On Collision with Shield...
	void OnCollisionEnter (Collision cl) {

		if (cl.gameObject.tag == "Asteroid" && !_isInvuln) {

			// Freshly Squeezed Game Juice.
			StartCoroutine (ShieldHighlight ());
			transform.parent.GetComponent<PlayerController> ().CallRumble (_flRumbleOnHit, _flRumbleControllerOnHit);

			// Make sure we aren't accidentally hit by the same asteroid twice before we start removing shield health.
			StartCoroutine (ShieldInvuln ());
			_itShieldCur -= 1;

			// Stop previous recharging coroutines and start a new one.
			StopCoroutine ("Recharge");
			StartCoroutine (Recharge (_flRechargeWait));

			// If our shield is depleted, clean the script and then shut evertyhing down. The script is re-enabled in the PlayerController.cs.
			if (_itShieldCur == 0) {
				transform.parent.GetComponent<PlayerController> ().CallRumble (_flRumbleOnHit, _flRumbleControllerOnHit);
				transform.parent.GetComponent<PlayerController> ()._areShieldsDown = true;
				gameObject.SetActive (false);
			}
			
		}

	}


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

	// Toggle Immunity for the Shield Immunity Duration. This should generally be a really low number and just used as a safeguard.
	IEnumerator ShieldInvuln () {

		_isInvuln = true;
		yield return new WaitForSeconds(_flShieldInvuln);
		_isInvuln = false;

	}

	// Set the Shield to Recharge after not being hit for a number of seconds...
	IEnumerator Recharge (float flWait) {

		yield return new WaitForSeconds (flWait);

		for (int it = _itShieldCur; it < _itShielding; it++) {
			_itShieldCur++;
			yield return new WaitForSeconds (_flChargingWait);
		}

	}

	// Lerp the Shield Highlight from Opaque to Transparent whenever the Player is hit.
	IEnumerator ShieldHighlight () {

		yield return null;

	}

}
