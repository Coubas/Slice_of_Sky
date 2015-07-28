using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreRecapManager : MonoBehaviour {
	public Text scoreNumberText;
	public Text highScoreText;

	public Slider[] gauges;
	public int[] valuesToFillGauges;

	PlayerData.ScoreData scoreData;

	// Use this for initialization
	void Start () {
		//Score
		//int[] spCol = new int[5];
		//spCol[0] = 5;
		//spCol[1] = 2;
		//spCol[2] = 6;
		//spCol[3] = 4;
		//spCol[4] = 9;
		//PlayerData.ScoreData scoreTest = new PlayerData.ScoreData(1, 15896, spCol);
		scoreData = PlayerData.PD.getLastScore();
		scoreNumberText.text = scoreData.score.ToString();

		if (scoreData.score >= PlayerData.PD.highScore)
		{
			PlayerData.PD.highScore = scoreData.score;
			highScoreText.enabled = true;
		}
		else
		{
			highScoreText.enabled = false;
		}

		for (int i = 0; i < gauges.Length; ++i)
		{
			gauges[i].value = (float)scoreData.spriritsCollected[i] / (float)valuesToFillGauges[i];
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
