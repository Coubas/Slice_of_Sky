using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreRecapManager : MonoBehaviour
{
    public Text scoreNumberText;
    public Text highScoreText;

    public Slider[] gauges;
    public Text[] gaugesLvlText;
    public int[] gaugesLvl;
    public int[] toAddDuringUpdate;
    public int[] currentNbInGauge;

    PlayerData.ScoreData scoreData;

    public float fAddTimerLenght = 1.0f / 3.0f;
    private float fTimerBetweenAdd;

    // Use this for initialization
    void Start()
    {
        fTimerBetweenAdd = .0f;

        gaugesLvl = new int[5];
        toAddDuringUpdate = new int[5];
        currentNbInGauge = new int[5];
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

        //Set the gauges depending to previous score and set the value to add during update
        for (int i = 0; i < gauges.Length; ++i)
        {
            int totalAmoutOfSpirits = PlayerData.PD.gaugesStocks[i] - scoreData.spriritsCollected[i];
            gaugesLvl[i] = 0;
            while (totalAmoutOfSpirits >= (5 + gaugesLvl[i] * 5))
            {
                totalAmoutOfSpirits -= 5 + gaugesLvl[i] * 5;
                gaugesLvl[i]++;
            }

            gauges[i].value = (float)totalAmoutOfSpirits / (float)(5 + gaugesLvl[i] * 5);
            gaugesLvlText[i].text = gaugesLvl[i].ToString();

            currentNbInGauge[i] = totalAmoutOfSpirits;
            toAddDuringUpdate[i] = scoreData.spriritsCollected[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        fTimerBetweenAdd += Time.deltaTime;

        if (fTimerBetweenAdd > fAddTimerLenght)
        {
            fTimerBetweenAdd = .0f;

            for (int i = 0; i < gauges.Length; ++i)
            {
                if (toAddDuringUpdate[i] > 0 && currentNbInGauge[i] < (5 + gaugesLvl[i] * 5))
                {
                    toAddDuringUpdate[i]--;
                    currentNbInGauge[i]++;
                }
            }
        }

        for (int i = 0; i < gauges.Length; ++i)
        {
            float val = (float)currentNbInGauge[i] / (float)(5 + gaugesLvl[i] * 5);
            gauges[i].value = Mathf.Lerp(gauges[i].value, val, fTimerBetweenAdd / fAddTimerLenght);

            if(gauges[i].value == 1.0f)
            {
                currentNbInGauge[i] = 0;
                gaugesLvl[i]++;

                gauges[i].value = .0f;
                gaugesLvlText[i].text = gaugesLvl[i].ToString();
            }

        }
    }
}
