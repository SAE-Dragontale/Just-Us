
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
  	Author: 		Eric Cox
	File:			AsteroidStats.cs
	Description: 	AsteroidStats gives the asteroid health, take damage, and controls 'splitting'.

	Contributor:	Hayden Reeve
	Description:	Changed the Score-Linking to a new script and streamlined the checking plus a small amount of code streamlining.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidStats : MonoBehaviour {

	public float health = 2f;
	public float charge = 5f;

	public PlayerController playerCredit;

	public void TakeDamage(float damage)
	{		

		playerCredit.AddCharge (1);

		health -= damage;
		gameObject.GetComponent<Renderer> ().material.color = new Color32(165,165,165,250);

		if (health <= 0) 
		{		
			playerCredit.AddCharge (charge);
			Instantiate ();
			Destroy (gameObject);
		}
	}

	public virtual void Instantiate()
	{
		
	}

}
