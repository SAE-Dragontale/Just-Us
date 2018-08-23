
/* --------------------------------------------------------------------------------------------------------------------------------------------------------- //
	Author: 		Eric Cox
	File:			PauseController.cs
	Description: 	Looks at if the players have died, and controls when the game ends based on isDead, and -flScore variables from PlayerController.
					Additionally Writes and reads highscores. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*	
	1. Read Score
	2. Read Death State
	3. If Player 1 is Dead and Player 2's Score is Higher--
	4. End the game, pause time, bring up final game menu.
	5. Declare Winner, let player write their name in. (Then remove panel)
	6. Allow the player to restart / exit to main menu
*/

public class GameController : MonoBehaviour {

	[Header("Player References")]
	[SerializeField] PlayerController player1;
	[SerializeField] PlayerController player2;
	[SerializeField] int numOfPlayers;
	[SerializeField] bool nameEntered;

	[Header("User Interface")]
	[SerializeField] public GameObject deathPanel;
	[SerializeField] Text winText;
	[SerializeField] GameObject player1Bars;
	[SerializeField] GameObject player2Bars;


	[Header("High Score Settings")]
	[SerializeField] string highScoreKey = "HighScore";
	[SerializeField] string highScoreNameKey = "HighScoreName";
	[SerializeField] int playersScore;
	[SerializeField] int highScore;
	[SerializeField] string highScoreName;
	[SerializeField] Text curNameTxtRef;
	[SerializeField] string strCurName;
	[SerializeField] InputField txtObjCurName;
	[SerializeField] bool hasGameEnded;

	[SerializeField] int[] highScores = new int[10];

	void Awake()
	{	
		hasGameEnded = false;
		Time.timeScale = 1f;

		if (!PlayerPrefs.HasKey ("PlayerMode")) 
		{
			PlayerPrefs.SetInt ("PlayerMode", 1);
		}

		numOfPlayers = PlayerPrefs.GetInt ("PlayerMode");

		if (numOfPlayers == 1) 
		{
			player2Bars.SetActive (false);
		}

		if (numOfPlayers == 2) 
		{
			player2Bars.SetActive (true);
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown ("p")) 
		{
			EndGame ();
		}

		if (numOfPlayers == 1) 
		{
			if (player1._isDead == true && hasGameEnded == false) 
			{
				EndGame ();
			}
		}

	// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

		if (numOfPlayers == 2) 
		{
			//if player 1 is dead
			if (player1._isDead == true && hasGameEnded == false) 
			{
				//if player 2 is alive
				if (player2._isDead == false && hasGameEnded == false) 
				{				
					//if player 2's score beats the dead player 1, win!
					if (player2._flScore > player1._flScore) 
					{
						EndGame ();
					}
				} 

				//if player 1 is dead and player 2 is dead.
				else 
				{
					EndGame ();
				}
			}

			//if player 2 is dead
			if (player2._isDead == true && hasGameEnded == false) 
			{
				//if player 1 is alive
				if (player1._isDead == false && hasGameEnded == false) 
				{				
					//if player 1's score beats the dead player 1, win!
					if (player1._flScore > player2._flScore) 
					{
						EndGame ();
					}
				}

				//if player 2 is dead and player 1 is dead.
				else 
				{
					EndGame ();
				}
			}
		}
	}

	public void WriteName()
	{
		txtObjCurName.enabled = false;
		strCurName = curNameTxtRef.text;

		nameEntered = true;
		EndGame ();
	}

	public void EndGame ()
	{	
		hasGameEnded = true;
		//Pauses game	
		Time.timeScale = 0f;

		//grabs text element with name Win Text on the death panel.
		winText = winText.GetComponent<Text> ();

		if (numOfPlayers == 2) 
		{
			if (player1._flScore > player2._flScore) 
			{
				winText.text = "Player 1 has JUST won!\nScore: " + player1._flScore.ToString ();
			}
			if (player2._flScore > player1._flScore) 
			{
				winText.text = "Player 2 has JUST won!\nScore: " + player2._flScore.ToString ();
			}
		}

		if (numOfPlayers == 1)
		{
			winText.text = "You JUST Died!\nScore: " + player1._flScore.ToString ();
		}

		deathPanel.SetActive (true);
		if (nameEntered == false) 
		{
			txtObjCurName.enabled = true;
		}

		//if there's only 1 player
		if (numOfPlayers == 1 && nameEntered == true) 
		{		
			//displayed text on Win Text
			winText.text = "You JUST Died!\nScore: " + player1._flScore.ToString ();

			//the winners score is set to the GameController variable.
			playersScore = (int)player1._flScore;

			Debug.Log ("start score loop");

			//loops 10 high scores
			for (int it = 0; it < highScores.Length; it++)
			{

				highScore = PlayerPrefs.GetInt(highScoreKey+it,0);
				highScoreName = PlayerPrefs.GetString(highScoreNameKey+it);

				//if score is greater, store previous highScore
				if(playersScore > highScore)
				{

					//Set new highScore
					//set score to previous highScore, and try again

					PlayerPrefs.SetInt (highScoreKey+it, playersScore);
					PlayerPrefs.SetString (highScoreNameKey+it, strCurName);

					playersScore = highScore;
					strCurName = highScoreName;

					//Once score is greater, it will always be for the
				}
			}

			PlayerPrefs.Save ();
		}

		// --------------------------------------------------------------------------------------------------------------------------------------------------------- */

		if (numOfPlayers == 2 && nameEntered == true) 
		{
			//if end game is called, check who has a higher score
			if (player1._flScore > player2._flScore) 
			{
				winText.text = "Player 1 has JUST won!\nScore: " + player1._flScore.ToString ();

				playersScore = (int)player1._flScore;

				Debug.Log ("start score loop");

				//loops 10 high scores
				for (int it = 0; it < highScores.Length; it++)
				{

					highScore = PlayerPrefs.GetInt(highScoreKey+it,0);
					highScoreName = PlayerPrefs.GetString(highScoreNameKey+it);

					//if score is greater, store previous highScore
					if(playersScore > highScore)
					{

						//Set new highScore
						//set score to previous highScore, and try again

						PlayerPrefs.SetInt (highScoreKey+it, playersScore);
						PlayerPrefs.SetString (highScoreNameKey+it, strCurName);

						playersScore = highScore;
						strCurName = highScoreName;

						//Once score is greater, it will always be for the
					}
				}
				PlayerPrefs.Save ();
			}

			if (player2._flScore > player1._flScore) 
			{
				winText.text = "Player 2 has JUST won!\nScore: " + player2._flScore.ToString ();

				playersScore = (int)player2._flScore;

				Debug.Log ("start score loop");

				//loops 10 high scores
				for (int it = 0; it < highScores.Length; it++)
				{

					highScore = PlayerPrefs.GetInt(highScoreKey+it,0);
					highScoreName = PlayerPrefs.GetString(highScoreNameKey+it);

					//if score is greater, store previous highScore
					if(playersScore > highScore)
					{

						//Set new highScore
						//set score to previous highScore, and try again

						PlayerPrefs.SetInt (highScoreKey+it, playersScore);
						PlayerPrefs.SetString (highScoreNameKey+it, strCurName);

						playersScore = highScore;
						strCurName = highScoreName;

						//Once score is greater, it will always be for the
					}
				}
				PlayerPrefs.Save ();

			}
		}	
	}
}
