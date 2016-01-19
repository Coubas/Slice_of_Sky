using UnityEngine;
using System.Collections;

[System.Serializable]
public class SubGauge
{
	public bool active;
    public bool willReload;
	public float totalAmount;
	public float currentAmount;
    public bool gotBonus;

	public SubGauge()
	{
		active = false;
        willReload = false;
		totalAmount = 2.0f;
		currentAmount = .0f;
        gotBonus = false;
	}
}

public class SpeedBoostGauge : MonoBehaviour {
	public SubGauge[] gauges;
	public int nbActive = 1;
    public int minNbActive;
	float boostCost;
	bool speedBoostLock = false;

	// Use this for initialization
	void Start () {
		boostCost = Time.fixedDeltaTime;

        if (PlayerData.PD.gaugesLvl.Length > 0 && PlayerData.PD.gaugesLvl[1] > 0)
        {
            int lvl = PlayerData.PD.gaugesLvl[1];
            minNbActive = 1 + lvl / 3;
            nbActive = 1 + lvl / 2;

            if (minNbActive > 6)
                minNbActive = 6;
            if (nbActive > 6)
                nbActive = 6;
        }

        for (int i = 0; i < gauges.Length; ++i)
        {
            gauges[i].willReload = i < minNbActive;
            gauges[i].active = i < nbActive;

            if (i < minNbActive || i < nbActive)
                gauges[i].currentAmount = gauges[i].totalAmount;
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

	public bool CanUseBoost()
	{
		if (speedBoostLock)
			return false;
		else if (nbActive > 1)
			return true;
		else if (nbActive == 1 && gauges[nbActive-1].currentAmount > boostCost)
			return true;

		return false;
	}

	public void UseBoost()
	{
		if (gauges[nbActive - 1].currentAmount >= boostCost)
		{
			gauges[nbActive - 1].currentAmount -= boostCost;
			if (nbActive > 1 && gauges[nbActive - 1].currentAmount == .0f)
			{
                if(nbActive > minNbActive)
    				gauges[nbActive - 1].active = false;
				nbActive--;
			}
		}
		else if (gauges[nbActive - 1].currentAmount < boostCost)
		{
			if (nbActive > 1)
			{
				float rest = boostCost - gauges[nbActive - 1].currentAmount;
                if (nbActive > minNbActive)
                    gauges[nbActive - 1].active = false;
				nbActive--;
				
				gauges[nbActive - 1].currentAmount -= rest;
			}
		}
	}

	public void ReloadGauges()
	{
		if (!speedBoostLock && gauges[0].currentAmount < boostCost)
			speedBoostLock = true;
		else if (speedBoostLock && gauges[0].currentAmount >= gauges[0].totalAmount*.5f)
			speedBoostLock = false;

		if (gauges[nbActive - 1].currentAmount < gauges[nbActive-1].totalAmount)
			gauges[nbActive - 1].currentAmount += Time.fixedDeltaTime * .5f;
        else if(nbActive < minNbActive)
        {
            nbActive++;
            gauges[nbActive - 1].active = true;
            gauges[nbActive - 1].currentAmount += Time.fixedDeltaTime * .5f;
        }
	}

	public void addGauge()
	{
		float current = gauges[nbActive - 1].currentAmount;

		gauges[nbActive - 1].currentAmount = gauges[nbActive - 1].totalAmount;
		nbActive++;
		gauges[nbActive - 1].active = true;
		gauges[nbActive - 1].currentAmount = current;
        gauges[nbActive - 1].gotBonus = true;
	}
}
