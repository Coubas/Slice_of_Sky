using UnityEngine;
using System.Collections;

public class SpeedGaugeBonus : Bonus
{
	protected override void applyBonus(GameObject _player)
	{
		_player.GetComponent<PlayerController>().gauges.addGauge();
	}
}
