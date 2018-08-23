
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Hayden Reeve
	File:			PlayerController.cs
	Description: 	This script controls the player object.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
	

	// --- GAMEPLAY VARIABLES

	[Header("Entity Settings")]
	[Tooltip("Player One is Integer 1. Player Two is Integer 2.")]
	[SerializeField] private int _itPlayerNum = 1;

	[Header("Gameplay Settings")]
	[SerializeField] private float _flShipSpeed = 3000f;
	[SerializeField] private float _flFireRate = 0.25f;
	[SerializeField] private float _flBulletSpeed = 1000f;
	[SerializeField] private float _flNormalTurn = 10f;
	[SerializeField] public float _flScore = 0f;

	[Header("Hull Settings")]
	[SerializeField] private int _itHullPoints = 6;
	[SerializeField] private float _flHullInvuln = 0.25f;

	[Header("Ultimate Settings")]
	[SerializeField] private float _flUltimateDur = 1f;
	[SerializeField] private float _flUltimateTurn = 2f;

	[Header("Charge Settings")]
	[SerializeField] private float _flChargeMax = 500f;
	[SerializeField] private float _flChargeCur = 0f;

	[Header("Rumble Settings")]
	[SerializeField] private float _flRumbleOnHit = 0.2f;
	[SerializeField] private float _flRumbleControllerOnHit = 0.2f;


	// --- CONTROLLERS VARIABLES

	// Input Variables
	[Header("Controller Settings")]
	[SerializeField] private int _itController = 0; // 0 is Keyboard, 1 is Gamepad #1. 2 is Gamepad #2. How convenient!

	private string _stPlayerMode = "PlayerMode";
	private bool _isChangingControls = false;

	// Gameplay Controllers

	[HideInInspector] public bool _isDead = false;
	[HideInInspector] public bool _areShieldsDown = false;

	[HideInInspector] private bool _isInvuln = false;
	[HideInInspector] private bool _canFire = true;
	[HideInInspector] public bool _isBeaming = false;

	// External Rumble Variables

	[HideInInspector] public bool _needsToRumble = false;
	[HideInInspector] private float _flNextRumbleDur;
	[HideInInspector] private float _flNextControllerShake;


	// --- COMPONENTS

	// External Components
	[Header("External Components")]

	// Shield Variables
	[SerializeField] private Text _txShield;
	[SerializeField] private Slider _slShieldBar;

	// Charge Variables
	[SerializeField] private Text _txCharge;
	[SerializeField] private Slider _slChargeBar;

	// Health Variables

	[SerializeField] private Text _txHealth;
	[SerializeField] private Slider _slHealthBar;

	// Score Variables
	[SerializeField] private Text _txScore;


	[SerializeField] private CameraGameplay _scMain;

	// Player Components
	[Header("Player Components")]
	[SerializeField] private Rigidbody _rbProjectile;
	[SerializeField] private Transform _trFirePoint;
	[SerializeField] private Transform _trBulletContainer;
	[SerializeField] private GameObject _gmLazor;
	[SerializeField] private ShieldController _scShield;

	// Internal Components
	private PlayerIndex _piPLayerNum;
	private GamePadState _psState;
	private Rigidbody _rb;

	[Header("Audio Components")]
	[SerializeField] AudioClip laser;
	[SerializeField] AudioClip ultLaser;
	[SerializeField] AudioSource audio;

	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */


	void Start () {

		audio = GetComponent<AudioSource>();

		// Assign Components
		_rb = GetComponent<Rigidbody> ();

		// Configure the Game Area depending on the number of players.
		if (PlayerPrefs.GetInt (_stPlayerMode) != 2) {

			if (_itPlayerNum == 1)
				transform.position = transform.parent.position;
			else
				Destroy (gameObject);

		}
		
		// Collect Configuration Data from Settings and apply it to PlayerNum.
		SetController ();

		// Initialise the Player's Score.
		AddCharge (0f);
		_slChargeBar.maxValue = _flChargeMax;
		_slHealthBar.maxValue = _itHullPoints;
		_slShieldBar.maxValue = _itHullPoints;
		_txHealth.text = _itHullPoints.ToString ();

		// Initialise the Player's Weaponry.
		_gmLazor.SetActive(false);

	}

	void Update () {

		// --- DIRTY CHEATS (Only in Dev-Mode)

		#if UNITY_EDITOR
		if (Input.GetKey(KeyCode.X)) {
			_flChargeCur = _flChargeMax;
			_slChargeBar.value = _flChargeCur;
			_txCharge.text = (_flChargeCur / _flChargeMax * 100f) + "%";
		}
		#endif

		// --- CHECK CONTROLS

		if (Time.timeScale == 0f && _isChangingControls == false) {
			_isChangingControls = true;
			SetController ();
		} else {
			_isChangingControls = false;
		}

		// --- PLAYER CONTROLS

		if (_itController >= 1) {

			// Create the Reference
			_psState = XInputDotNetPure.GamePad.GetState (_piPLayerNum);

			// Gamepad Controls -- Fire
			if (_psState.Triggers.Right == 1f && _canFire) {
				StartCoroutine (FireProjectile ());
			}

			// Gamepad Controls -- Ultimate
			if (_psState.Buttons.A == ButtonState.Pressed && _flChargeCur >= _flChargeMax) {
				StartCoroutine (FireUltimate ());
			}

		} else {

			// Keyboard Controls -- Fire
			if (Input.GetAxis("Fire1") != 0 && _canFire) {
				StartCoroutine (FireProjectile ());
			}

			// Keyboard Controls -- Ultimate
			if (Input.GetAxis("Fire2") != 0 && _flChargeCur >= _flChargeMax) {
				StartCoroutine (FireUltimate ());
			}

		}

		// --- OTHER SCRIPT CALLS

		if (_needsToRumble) {
			StartCoroutine (Rumble(_flNextRumbleDur, _flNextControllerShake));
			_needsToRumble = false;
		}

	}

	void FixedUpdate () { 

		// GAMEPAD
		if (_itController >= 1) {

			// Find the Movement Thumbstick's Direction and move them based on it.
			Vector3 v3AimedDir = new Vector3 (_psState.ThumbSticks.Left.X, _psState.ThumbSticks.Left.Y, 0f);
			_rb.AddForce (v3AimedDir * _flShipSpeed * Time.deltaTime, ForceMode.Acceleration);

			// Change the player's rotation to face towards the angle the user has pressed the thumbstick in.

			if (_psState.ThumbSticks.Right.Y != 0 && _psState.ThumbSticks.Right.X != 0) {
				transform.rotation = Quaternion.Lerp (
					transform.rotation, 
					Quaternion.AngleAxis (Mathf.Atan2 (_psState.ThumbSticks.Right.Y, _psState.ThumbSticks.Right.X) * Mathf.Rad2Deg - 90, Vector3.forward), 
					Time.deltaTime * (_isBeaming ? _flUltimateTurn : _flNormalTurn)
				);
			}

		// KEYBOARD
		} else {

			// Monitor the player's Key Input and change the desired direction based on it.
			Vector3 v3AimedDir = new Vector3 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
			_rb.AddForce (v3AimedDir * _flShipSpeed * Time.deltaTime, ForceMode.Acceleration);

			// Rotate the player towards the current mouse location.
			Vector3 v3MouseDir = Input.mousePosition; //- CameraParent.main.WorldToScreenPoint (transform.position);

			transform.rotation = Quaternion.Lerp (
				transform.rotation, 
				Quaternion.AngleAxis (Mathf.Atan2 (v3MouseDir.y, v3MouseDir.x) * Mathf.Rad2Deg - 90, Vector3.forward), 
				Time.deltaTime * (_isBeaming ? _flUltimateTurn : _flNormalTurn)
			);

		}

	}

	// On Collision with Ship...
	void OnCollisionEnter (Collision cl) {

		if (cl.gameObject.tag == "Asteroid" && _areShieldsDown && !_isInvuln) {

			// Make sure we aren't accidentally hit by the same asteroid twice before we start removing hull points.
			StartCoroutine (HullInvuln ());
			_itHullPoints -= 1;
			_slHealthBar.value = _itHullPoints;
			_txHealth.text = _itHullPoints.ToString();

			_slShieldBar.value = _itHullPoints;
			_txShield.text = _itHullPoints.ToString();

			// Add some more Game Juice
			StartCoroutine (Rumble (_flRumbleOnHit,_flRumbleControllerOnHit));

			// Stop previous recharging coroutines and start a new one.
			StopCoroutine ("RestartShields");
			StartCoroutine (RestartShields ());

			// If our shield is depleted, clean the script and then shut evertyhing down. The script is re-enabled in the PlayerController.cs.
			if (_itHullPoints == 0) {
				StopAllCoroutines ();
				_isDead = true;
				Destroy (gameObject);
			}

		}

	}

	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

	// Called when an entity needs to increment the player's charge.
	public void AddCharge (float flChargeAdd) {

		// Make sure the new score adheres to the parameters.
		_flChargeCur += flChargeAdd;

		if (_flChargeCur > _flChargeMax) {
			_flChargeCur = _flChargeMax;
		}

		// Present the final information to the player.
		_slChargeBar.value = _flChargeCur;

		if (_flChargeCur != 0) {
			_txCharge.text = (_flChargeCur / _flChargeMax * 100f) + "%";
		} else {
			_txCharge.text = _flChargeCur + "%";
		}

		if (flChargeAdd > 0) 
			AddScore (flChargeAdd);

	}

	// Called when an entity needs to increement the player's score.
	public void AddScore (float flScoreAdd) {

		// Increment the score, and pass the variables to the game
		_flScore += flScoreAdd;
		_txScore.text = _flScore.ToString();

	}

	// Called when we need to figure out what controller the player is using
	public void SetController () {

		// Collect Configuration Data from Settings and apply it to PlayerNum.
		_itController = PlayerPrefs.GetInt ("OptionController" + _itPlayerNum);

		if (_itController == 1)
			_piPLayerNum = PlayerIndex.One;
		else if (_itController == 2)
			_piPLayerNum = PlayerIndex.Two;

	}

	// Call the Rumble Event. If we call the Coroutine directly from another script and that script disables, the effect is stopped.
	public void CallRumble (float flRumbleDur, float flControllerRumble) {

		_flNextRumbleDur = flRumbleDur;
		_flNextControllerShake = flControllerRumble;

		if ( !_isBeaming)
			_needsToRumble = true;

	}

	// Called when the Player activates a basic fire command.
	IEnumerator FireProjectile () {

		audio.PlayOneShot(laser, 0.5F);

		// Disable Firing temporarily.
		_canFire = false;

		// Instantiate the projectile.
		Rigidbody rb;

		rb = Instantiate (_rbProjectile, _trFirePoint.position, _trFirePoint.rotation) as Rigidbody;
		rb.AddForce (_trFirePoint.up * _flBulletSpeed);
		rb.transform.parent = _trBulletContainer;
		rb.GetComponent<BulletController> ()._pcOrigin = this;

		// Enable Firing at the next FireRate Tick.
		yield return new WaitForSeconds(_flFireRate);

		// Only enable if the player hasn't activated the beam beam in the downtime.
		if (_isBeaming == false)
			_canFire  = true;

	}

	// Called when the player has enough Ultimate Charge and chooses to subsequently unleash their win button.
	IEnumerator FireUltimate () {
		
		audio.PlayOneShot(ultLaser, 0.5F);
		// Expend Charge
		AddCharge(-_flChargeMax);

		// Create Ultimate Beam Environment
		_canFire = false;
		_isBeaming = true;

		StopCoroutine ("Rumble");
		StartCoroutine (Rumble(_flUltimateDur, 0.5f));

		// Activate the Beam for the UltimateDuration timeframe.
		_gmLazor.SetActive (true);
		yield return new WaitForSeconds (_flUltimateDur);
		_gmLazor.SetActive (false);

		// Cleanup Ultimate Beam Environment
		_isBeaming = false;
		_canFire  = true;

	}

	// Called when RUMBLE is needed.
	public IEnumerator Rumble (float flRumbleDur, float flControllerRumble) {

		_scMain._isRumbling = true;

		if (_itController != 0)
			GamePad.SetVibration(_piPLayerNum,flControllerRumble,flControllerRumble);

		yield return new WaitForSeconds (flRumbleDur);

		if (_itController != 0)
			GamePad.SetVibration(_piPLayerNum,0f,0f);
		
		_scMain._isRumbling = false;

	}

	// Toggle Immunity for the Health Immunity Duration. This should generally be a really low number and just used as a safeguard.
	IEnumerator HullInvuln () {

		_isInvuln = true;
		yield return new WaitForSeconds(_flHullInvuln);
		_isInvuln = false;

	}

	// Set the Shield to Recharge after not being hit for a number of seconds...
	IEnumerator RestartShields () {

		yield return new WaitForSeconds (_scShield._flRechargeWait);
		_scShield.gameObject.SetActive (true);
		_areShieldsDown = false;

	}

}
