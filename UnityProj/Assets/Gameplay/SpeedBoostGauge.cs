using UnityEngine;
using System.Collections;

[System.Serializable]
public class SubGauge
{
	public bool active;
	public float totalAmount;
	public float currentAmount;

	public SubGauge()
	{
		active = false;
		totalAmount = 2.0f;
		currentAmount = .0f;
	}
}

public class SpeedBoostGauge : MonoBehaviour {
	public SubGauge[] gauges;
	public int nbActive = 1;
	float boostCost;
	bool speedBoostLock = false;

	// Use this for initialization
	void Start () {
		boostCost = Time.fixedDeltaTime;
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
				nbActive--;
				gauges[nbActive - 1].active = false;
			}
		}
		else if (gauges[nbActive - 1].currentAmount < boostCost)
		{
			if (nbActive > 1)
			{
				float rest = boostCost - gauges[nbActive - 1].currentAmount;
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
	}

	public void addGauge()
	{
		float current = gauges[nbActive - 1].currentAmount;

		gauges[nbActive - 1].currentAmount = gauges[nbActive - 1].totalAmount;
		nbActive++;
		gauges[nbActive - 1].active = true;
		gauges[nbActive - 1].currentAmount = current;
	}
}
