
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
  	Author: 		Eric Cox, Hayden Reeve
	File:			BulletController.cs
	Description: 	This script controls the player's bullets. Including collision and Self Destruction.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour {

	[Header("Bullet Variables")]

	[Tooltip("The damage applied to the collision target")]
	[SerializeField] private float _flDamage = 1f;

	public PlayerController _pcOrigin;


	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

	// Use this for initialisation.
	void Start () 
	{
		StartCoroutine (Destroy ());
	}

	// Use this to recieve collision specific functions.
	public void OnTriggerEnter (Collider cd) 
	{
		if (cd.gameObject.tag == "Asteroid") 
		{
			cd.gameObject.GetComponent<AsteroidStats> ().playerCredit = _pcOrigin;
			cd.gameObject.GetComponent<AsteroidStats> ().TakeDamage (_flDamage);
			Destroy (gameObject);
		}

		if (cd.gameObject.tag == "Enemy") 
		{
			//cd.gameObject.GetComponent<EnemyStats> ().TakeDamage (_flDamage);
		}
		//(PlayerHealth)target.GetComponent("PlayerHealth")

	}

	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

	// After a delay and having hit nothing, destroy the bullet.
	IEnumerator Destroy() 
	{
		yield return new WaitForSeconds (1.5f);
		Destroy(gameObject);
	}
}
