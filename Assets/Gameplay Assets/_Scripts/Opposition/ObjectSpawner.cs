
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Eric Cox
	File:			ObstacleSpawner.cs
	Description: 	Dictates what spawns and where in fixed areas around the play space.

	Contributor:	Hayden Reeve
	Description:	Changed the instantiated objects to sort under an organised folder in the hierachy.
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

	[Header("Organisation")]

	//Array of gameobjects, intended for Asteroid and Enemy Ship prefabs.
	[SerializeField] Rigidbody[] objectsToSpawn;

	//Spawn locations from which objects are instantiated.
	[SerializeField] Transform[] spawnLocations;

	[SerializeField] Transform objectList;

	[Header("Spawner Settings")]
	[SerializeField] float spawnNowTimer = 2f;
	[SerializeField] float objectSpeed = 240f;
	[SerializeField] float randSpeedValue;
	[SerializeField] float objectDiscourse = 30f;
	[SerializeField] Transform[] tempPlayerPos;

	[Header("Player References")]
	[SerializeField] Transform player1;
	[SerializeField] Transform player2;
	[SerializeField] int numOfPlayers;

	void Start () 
	{ 
		
		if (!PlayerPrefs.HasKey ("PlayerMode")) 
		{
			PlayerPrefs.SetInt ("PlayerMode", 1);
		}
		numOfPlayers = PlayerPrefs.GetInt ("PlayerMode");
		Debug.Log (numOfPlayers);
		if (numOfPlayers == 2) 
		{
			// reduces spawn timer limit, reducing the time it takes for things to spawn.
			spawnNowTimer = spawnNowTimer / 1.5f;
		}

		StartCoroutine (Spawn (spawnNowTimer));	
	}

	void Update ()
	{		
	}

	private IEnumerator Spawn(float spawnRate)
	{		

		while (true) 
		{			
			yield return new WaitForSeconds (spawnRate);

			// Gets each of the children (spawn locations) at random, rotates it to look at player
			Transform tempTransform = transform.GetChild(Random.Range(0,spawnLocations.Length));

			//if there's one player in the game scene
			if (numOfPlayers == 1) 
			{
				//if that player exists
				if (player1 != null) 
				{
					//add the player's transform reference to a transform array.
					tempPlayerPos [0] = player1;
				}

				//look at array transform 0 and spawn asteroids in that direction
				tempTransform.LookAt (tempPlayerPos[0]);	
			}

			//if there's two players in the game scene
			if (numOfPlayers == 2) 
			{	
				//if payer 1 exists
				if (player1 != null) 
				{
					//add the player's transform reference to a transform array.
					tempPlayerPos [0] = player1;
				}

				//if payer 2 exists
				if (player2 != null) 
				{
					//add the player's transform reference to a transform array.
					tempPlayerPos [1] = player2;
				}

				//look at array transform 0 and spawn asteroids in that direction
				tempTransform.LookAt (tempPlayerPos [Random.Range (0, tempPlayerPos.Length)]);

			}

			//gets a random value of 
			randSpeedValue = Random.Range (objectSpeed * .8f, objectSpeed * 1.2f);
			//Creates an instance, instantiates a random game object. Then adds a random foce between 80 % and 120% of the inspectors 'object speed'.
			// this is thown towards the player, with minor discourse.
			Rigidbody instance;
			instance = Instantiate (objectsToSpawn [Random.Range (0, objectsToSpawn.Length)], tempTransform.position, tempTransform.rotation) as Rigidbody;
			instance.GetComponent<Rigidbody> ().AddForce (tempTransform.forward * randSpeedValue * Time.deltaTime,ForceMode.Impulse);
			instance.GetComponent<Rigidbody> ().AddForce (tempTransform.up * Random.Range(-objectDiscourse,objectDiscourse) * Time.deltaTime,ForceMode.Impulse);

			//gets a random value of 0 or 1, multiplies that by 2 and takes 1. so 0 becomes -1, and 1 becomes 2 then back to 1
			float torgueValue = Random.Range (0, 2)*2-1;
			//uses random value to to add torgue in up, forward, and right directions
			instance.GetComponent<Rigidbody> ().AddTorque (transform.up * torgueValue / 6, ForceMode.Impulse);
			instance.GetComponent<Rigidbody> ().AddTorque (transform.forward * torgueValue / 6, ForceMode.Impulse);
			instance.GetComponent<Rigidbody> ().AddTorque (transform.right * torgueValue / 6, ForceMode.Impulse);

			//childs the instantiated objects to an object list in the game editor.
			instance.transform.parent = objectList;
		}
	}
}
