using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
	public static PlayerData PD;

	public ArrayList scores;
	public int highScore;
    public bool isHighScore;
	public bool invertYAxis = false;

    public int[] gaugesStocks;
    public int[] gaugesLvl;

    public int ammoCount;

    public class ScoreData
	{
		public int level;
		public int score;
		public int[] spriritsCollected;

		public ScoreData(int _lvl, int _score, int[] _spriritsCollected)
		{
			level = _lvl;
			score = _score;
			spriritsCollected = _spriritsCollected;
		}
	}

	// Use this for initialization
	void Awake () {

		//Singletton Code
		if (PD != null)
		{
			Destroy(gameObject);
			return;
		}
		else
			PD = this;

		DontDestroyOnLoad(this);
		//----------------

		scores = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Load()
    {
        SaveLoadManager.Load();
        SaveLoadManager.GetSavedData(ref invertYAxis, ref highScore, ref gaugesStocks, ref gaugesLvl);
    }

	public void addScore(int _level, int _score, int[] _spiritsCollected)
	{
		scores.Add(new ScoreData(_level, _score, _spiritsCollected));

        //HighScore management
        if(_score >= highScore)
        {
            highScore = _score;
            isHighScore = true;
        }
        else
        {
            isHighScore = false;
        }

        //Update collected spirits count
        for(int i = 0; i < _spiritsCollected.Length; ++i)
        {
            gaugesStocks[i] += _spiritsCollected[i];

            int totalAmoutOfSpirits = gaugesStocks[i];
            gaugesLvl[i] = 0;
            while (totalAmoutOfSpirits >= (5 + gaugesLvl[i] * 5))
            {
                totalAmoutOfSpirits -= 5 + gaugesLvl[i] * 5;
                gaugesLvl[i]++;
            }
        }

        //Save the game
        SaveLoadManager.SetSavedData(invertYAxis, highScore, gaugesStocks, gaugesLvl);
        SaveLoadManager.Save();
	}

	public ScoreData getLastScore()
	{
		return (ScoreData)scores[scores.Count - 1];
	}

    public void ComputeAmmoAmount()
    {
        if(gaugesLvl.Length > 0)
        {
            ammoCount = gaugesLvl[0] * 5 + 5;
        }
    }
}
