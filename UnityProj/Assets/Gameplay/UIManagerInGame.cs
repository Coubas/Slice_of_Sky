﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManagerInGame : MonoBehaviour {
	public Text Score;
	public Text Timer;
	public Animator TimerAnimator;
    public Text TimerBonus;
    public Animator TimerBonusAnimator;
	public Text Combo;
	public Animator ComboAnimator;
	private int prevCombo = 0;
	public Image[] speedGaugeFill;
	public Image[] speedGaugeBorder;
    public Color willReloadColor;
    public Color activeColor;
    public Color inactiveColor;
    public Text Ammo;

    private PlayerController playerControler;

	void Start()
	{
		playerControler = GameMaster.GM.dragon.GetComponent<PlayerController>() as PlayerController;

        if ((PlayerData.PD.gaugesLvl.Length > 0 && PlayerData.PD.gaugesLvl[0] > 0) || PlayerData.PD.gaugesLvl.Length <= 0)
        {
            Ammo.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            Ammo.transform.parent.gameObject.SetActive(false);
        }
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

            //Ammo
            if ((PlayerData.PD.gaugesLvl.Length > 0 && PlayerData.PD.gaugesLvl[0] > 0) || PlayerData.PD.gaugesLvl.Length <= 0)
            {
                Ammo.text = PlayerData.PD.ammoCount.ToString();
            }
		}

		UpdateSpeedGauges();
	}

	public void UpdateSpeedGauges()
	{
		for (int i = 0; i < playerControler.gauges.gauges.Length; ++i)
		{
            if ((PlayerData.PD.gaugesLvl.Length > 0 && PlayerData.PD.gaugesLvl[1] > 0) || PlayerData.PD.gaugesLvl.Length <= 0)
            {
                SubGauge currentGauge = playerControler.gauges.gauges[i];
                if (currentGauge.willReload)
                {
                    speedGaugeBorder[i].color = willReloadColor;
                    speedGaugeFill[i].fillAmount = (currentGauge.currentAmount / currentGauge.totalAmount);
                }
                else if (currentGauge.active)
                {
                    speedGaugeBorder[i].color = activeColor;
                    speedGaugeFill[i].fillAmount = (currentGauge.currentAmount / currentGauge.totalAmount);
                }
                else
                {
                    speedGaugeBorder[i].color = inactiveColor;
                    speedGaugeFill[i].fillAmount = .0f;
                }
            }
            else
            {
                speedGaugeBorder[i].enabled = false;
                speedGaugeFill[i].enabled = false;
            }
		}
	}

    public void PickedUpTimerBonus(float _amount)
    {
        int timeBonus = Mathf.RoundToInt(_amount);
        if (timeBonus % 60 >= 10)
            TimerBonus.text = "+ " + timeBonus / 60 + " : " + timeBonus % 60;
        else
            TimerBonus.text = "+ " + timeBonus / 60 + " : 0" + timeBonus % 60;
        TimerBonusAnimator.SetTrigger("BonusPickedUp");
    }
}
