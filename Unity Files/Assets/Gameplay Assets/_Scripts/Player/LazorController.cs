
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
  	Author: 		Hayden Reeve
	File:			LazorController.cs
	Description: 	This script controls the player's ultimate ability, the MEGA LAZOR SUPER DEATH BEAM THINGY.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazorController : MonoBehaviour {

	// --- GAMEPLAY VARIABLES
	[Header("The score gained for destroying objects with the lazor.")]
	[SerializeField] private float _flScore = 5f;

	// --- COMPONENTS
	private PlayerController _pc;

	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

	// Use this for initialisation.
	void Start () {
		_pc = transform.parent.parent.GetComponent<PlayerController> ();
	}

	// Use this to recieve collision specific functions.
	public void OnTriggerEnter (Collider cd) {
		
		if (cd.gameObject.tag == "Asteroid" || cd.gameObject.tag == "Enemy") 
		{
			_pc.AddCharge (_flScore);
			Destroy (cd.gameObject);
		}
	
	}

}
