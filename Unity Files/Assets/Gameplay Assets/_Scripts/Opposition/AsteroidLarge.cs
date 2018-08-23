
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Eric Cox
	File:			AsteroidLarge.cs
	Description: 	Overrides score to a unique value

	Contributor:	Hayden Reeve
	Description:	Changed the instantiated objects to sort under an organised folder in the hierachy.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidLarge : AsteroidStats {

	[SerializeField] Rigidbody smallAsteroidPrefab; 
	[SerializeField] float smallSpeed = 30f; 

	[SerializeField] private GameObject explosionVFX;

	//[SerializeField] private ParticleSystem explosionVFX;

	public void Start()
	{
		charge = 5f;
	}

	public override void Instantiate()
	{

		GameObject particalTime = Instantiate (explosionVFX,transform.position,transform.rotation);
		Destroy (particalTime, .5f);

		Vector3 largeSpeed = gameObject.GetComponent<Rigidbody> ().velocity;
		int randomAsteroidNum = Random.Range (2,4);
		for (int it = 0; it < randomAsteroidNum; it++)
		{

			Vector3 spawnSpeed = new Vector3 (Random.Range (-10, 10), Random.Range (-10, 10), 0);

			Rigidbody instance;
			instance = Instantiate(smallAsteroidPrefab,transform.position,transform.rotation) as Rigidbody;
			instance.GetComponent<Rigidbody> ().AddForce (spawnSpeed * smallSpeed * Time.deltaTime,ForceMode.Impulse);
			instance.GetComponent<Renderer>().material.color = new Color32(165,165,165,250);
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ we can do something here like transform.forward, mess with it to see what wrks

			instance.GetComponent<Rigidbody> ().AddTorque (largeSpeed / 6, ForceMode.Impulse);

			//Hierachy formatting
			instance.transform.parent = transform.parent;
		}

	}


}
