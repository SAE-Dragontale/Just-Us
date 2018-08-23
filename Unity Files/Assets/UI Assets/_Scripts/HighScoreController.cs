using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour {
	
	[SerializeField] Text[] highScoreList;
	[SerializeField] string readHighScore;
	[SerializeField] string highScoreKey = "HighScore";
	[SerializeField] string highScoreNameKey = "HighScoreName";
	GameController gameController1;

	[SerializeField] int[] highScores = new int[10];

	string firstPlay;

	public void Start()
	{


		//if statement to write "No Score", and "0" to highscore name and value. 
		//simply to populate values and add polish
		//checks playerprefs for an existig "FirstPlay" string
		if (PlayerPrefs.GetString("FirstPlay") != "No") 
		{
			//loops through the length of the highscores(10)
			for (int it = 0; it < highScores.Length; it++)
			{
				//retrieves the highscore at each position through a unique key
				highScoreKey = "HighScore"+(it);

				//sets each unique key for numbers, to 0
				PlayerPrefs.SetInt (highScoreKey, 0);
			}
			//loops through the length of the highscores(10)
			for (int it = 0; it < highScores.Length; it++)
			{
				//retrieves the highscore at each position through a unique key
				highScoreNameKey = "HighScoreName"+(it);

				//sets each unique key for names, to 0
				PlayerPrefs.SetString (highScoreNameKey,"No Score");
			}

			//sets the prefs to No, so this if check has been used
			PlayerPrefs.SetString("FirstPlay","No");
		}

		//loops through the length of the high scores 
		for (int it = 0; it < highScoreList.Length; it++) 
		{
			//reads the highscore balue of each unique key
			readHighScore = PlayerPrefs.GetInt (highScoreKey + it).ToString();

			//writes to an array Text value,the name and value of the players score at that position
			highScoreList [it].text = PlayerPrefs.GetString("HighScoreName" + it) + " : " + readHighScore;
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown ("=")) 
		{
			Debug.Log ("Scores reset");

			//loops through the length of the highscores(10)
			for (int it = 0; it < highScores.Length; it++)
			{
				//retrieves the highscore at each position through a unique key
				highScoreKey = "HighScore"+(it);

				//sets each unique key for numbers, to 0
				PlayerPrefs.SetInt (highScoreKey, 0);
			}
			//loops through the length of the highscores(10)
			for (int it = 0; it < highScores.Length; it++)
			{
				//retrieves the highscore at each position through a unique key
				highScoreNameKey = "HighScoreName"+(it);

				//sets each unique key for names, to 0
				PlayerPrefs.SetString (highScoreNameKey,"No Score");
			}

			//sets the prefs to No, so this if check has been used
			PlayerPrefs.SetString("FirstPlay","No");


			//loops through the length of the high scores 
			for (int it = 0; it < highScoreList.Length; it++) 
			{
				//reads the highscore balue of each unique key
				readHighScore = PlayerPrefs.GetInt (highScoreKey + it).ToString();

				//writes to an array Text value,the name and value of the players score at that position
				highScoreList [it].text = PlayerPrefs.GetString("HighScoreName" + it) + " : " + readHighScore;
			}
		}
	}
}

