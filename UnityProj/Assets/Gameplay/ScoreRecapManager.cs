using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreRecapManager : MonoBehaviour {
	public Text scoreNumberText;
	public Text highScoreText;

	public Slider[] gauges;
    public Text[] gaugesLvlText;
    public int[] gaugesLvl;

	PlayerData.ScoreData scoreData;

	// Use this for initialization
	void Start () {
        gaugesLvl = new int[5];
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

        highScoreText.enabled = PlayerData.PD.isHighScore;

		for (int i = 0; i < gauges.Length; ++i)
		{
            int totalAmoutOfSpirits = PlayerData.PD.gaugesStocks[i];
            gaugesLvl[i] = 0;
            while(totalAmoutOfSpirits >= (5+gaugesLvl[i]*5))
            {
                totalAmoutOfSpirits -= 5 + gaugesLvl[i] * 5;
                gaugesLvl[i]++;
            }

			gauges[i].value = (float)totalAmoutOfSpirits / (float)(5 + gaugesLvl[i] * 5);
            gaugesLvlText[i].text = gaugesLvl[i].ToString();
        }
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
