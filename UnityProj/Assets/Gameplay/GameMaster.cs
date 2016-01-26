using UnityEngine;
using System.Collections;
using beffio.OneMinuteGUI;

//The use of the singleton allow us to set public variable in the inspector and still use them anywhere we want in the code

public class GameMaster : MonoBehaviour {
	public static GameMaster GM;

	public float maxDistDragon;
	public float maxDistDecor;

	public int score = 0;
	public int[] spiritsCollected;
	public int[] comboValues;
	public int combo = 0;
	public float comboTimer = .0f;

	public int spiritCount;
	public ArrayList spirits = new ArrayList();

	public float waitBeforeNextLevel;
	//GUI
	public bool gamePaused = false;
    public bool gameOn;
	public float levelTimer;
	public GameObject facebookController;
    public MenuManager menuManager;
    public GameObject tutoPanel;

    public UIManagerInGame uiMgr;

    //For tests
    public GameObject dragon;
	private float timer = .0f;

    void Start()
    {
        gameOn = true;

        if (PlayerData.PD.gaugesLvl.Length > 0)
        {
            if (PlayerData.PD.gaugesLvl[0] == 0 || PlayerData.PD.gaugesLvl[1] == 0)
            {
                levelTimer = .0f;

                //Put on the tutoMenu and pause the game
                gamePaused = true;
                menuManager.GoToMenu(tutoPanel);
            }
            else
                levelTimer += PlayerData.PD.gaugesLvl[3] * 15;
        }
    }

	void Update()
	{
		if (GameMaster.GM.gamePaused)
			return;

#if UNITY_ANDROID
		if(Screen.sleepTimeout != SleepTimeout.NeverSleep)
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif

		timer += Time.deltaTime;

        if (PlayerData.PD.gaugesLvl.Length > 0 && PlayerData.PD.gaugesLvl[0] == 0) //First lvl, only blue spirits
        {
            if(spiritsCollected[0] >= 5)
            {
                endCurrentLevel();
            }
        }
        else if (PlayerData.PD.gaugesLvl.Length > 0 && PlayerData.PD.gaugesLvl[1] == 0) // Second lvl, blue and yellow spirits
        {
            if (spiritsCollected[1] >= 5)
            {
                endCurrentLevel();
            }
        }
        else
        {
            if (levelTimer <= .0f)
            {
                endCurrentLevel();
            }
            else
                levelTimerHandler();
        }

		comboTimerHandle();
	}

    void endCurrentLevel()
    {
        gameOn = false;

        if (waitBeforeNextLevel > .0f)
        {
            waitBeforeNextLevel -= Time.deltaTime;
        }
        else
        {
            levelTimer = .0f;
            PlayerData.PD.addScore(Application.loadedLevel, score, spiritsCollected);
            Application.LoadLevel("ScoreRecap");
        }
    }

	void levelTimerHandler()
	{
			levelTimer -= Time.deltaTime;
	}

	void comboTimerHandle()
	{
		if (combo > 0)
		{
			if (comboTimer > .0f)
				comboTimer -= Time.deltaTime;
			else
			{
				comboTimer = .0f;
				combo = 0;
			}
		}
	}

	public void  addScore(int _score, int _spiritType)
	{
		score += _score * comboValues[combo];
		if (combo < comboValues.Length - 2)
			combo++;

		comboTimer = 3.0f;

		spiritsCollected[_spiritType]++;
	}

	void Awake()
	{
		GM = this;

		score = 0;
		spiritsCollected = new int[5];
		combo = 0;
		comboTimer = .0f;
		timer = .0f;
	}
}