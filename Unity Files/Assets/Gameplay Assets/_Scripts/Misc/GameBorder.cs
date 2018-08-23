
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Eric Cox
	File:			GameBorder.cs
	Description: 	Destroys anything that leaves the fixed play space.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBorder : MonoBehaviour {

	void OnTriggerExit (Collider col)
	{
		Destroy (col.gameObject);
	}
}
