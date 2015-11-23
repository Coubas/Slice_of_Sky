using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManagerInGame : MonoBehaviour {
	public Text Score;
	public Text Timer;
	public Animator TimerAnimator;
	public Text Combo;
	public Animator ComboAnimator;
	private int prevCombo = 0;
	public Image[] speedGaugeFill;
	public Image[] speedGaugeBorder;

	private PlayerController playerControler;

	void Start()
	{
		playerControler = GameMaster.GM.dragon.GetComponent<PlayerController>() as PlayerController;
	}

	void Update()
	{
		if (GameMaster.GM.levelTimer > .0f)
		{
			//Score
			Score.text = GameMaster.GM.score.ToString();

			//Timer
			int timeLeft = Mathf.RoundToInt(GameMaster.GM.levelTimer);
			if (timeLeft % 60 >= 10)
				Timer.text = timeLeft / 60 + " : " + timeLeft % 60;
			else
				Timer.text = timeLeft / 60 + " : 0" + timeLeft % 60;
			TimerAnimator.SetInteger("TimeLeft", timeLeft);

			//Combo
			int combo = GameMaster.GM.combo;
			if (combo > 0 && combo > prevCombo)
			{
				Combo.text = "x" + (GameMaster.GM.comboValues[combo]);
				ComboAnimator.SetTrigger("ComboAdded");
				prevCombo = combo;
			}
			else if (combo == 0)
			{
				Combo.text = "";
                prevCombo = 0;
            }
		}

		UpdateSpeedGauges();
	}

	public void UpdateSpeedGauges()
	{
		for (int i = 0; i < playerControler.gauges.gauges.Length; ++i)
		{
			SubGauge currentGauge = playerControler.gauges.gauges[i];
			if (currentGauge.active)
			{
				speedGaugeBorder[i].color = Color.white;
				speedGaugeFill[i].fillAmount = (currentGauge.currentAmount / currentGauge.totalAmount);
			}
			else
			{
				speedGaugeBorder[i].color = Color.grey;
				speedGaugeFill[i].fillAmount = .0f;
			}
		}
	}
}
