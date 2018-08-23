
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Eric Cox
	File:			AsteroidSmall.cs
	Description: 	Overrides score to a unique value
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSmall : AsteroidStats {

	[SerializeField] private GameObject explosionVFX;

	public void Start()
	{
		charge = 15f;
	}

	public override void Instantiate()
	{
		GameObject particalTime = Instantiate (explosionVFX,transform.position,transform.rotation);
		Destroy (particalTime, .5f);
	}
}
