using UnityEngine;
using System.Collections;

public class MockSaveController : MonoBehaviour {
	
	static int bestScore 		= 0;					//The best score the player has reached
	
	//If there is no save data, create it, else, load the existing
	public static void CreateAndLoadData() 
	{
		//If found the coin ammount data, load the datas
		if (PlayerPrefs.HasKey("Best Score"))
			LoadData();
		//Else, create the data
		else
			CreateData();
	}

	//Creates a blank save
	public static void CreateData()
	{
				PlayerPrefs.SetInt ("Best Score", 0);
	}

	//Loads the save
	static void LoadData()
	{
		bestScore 	= PlayerPrefs.GetInt("Best Score");
	}

	//Return data
	public static int GetBestScore()
	{
		return bestScore;
	}

	//Modifies and saves
	public static void SetBestScore(int score)
	{
		bestScore = score;
		PlayerPrefs.SetInt("Best Score", bestScore);
		PlayerPrefs.Save();
	}
}
