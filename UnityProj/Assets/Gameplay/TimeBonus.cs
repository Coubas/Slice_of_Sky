using UnityEngine;
using System.Collections;

public class TimeBonus : Bonus {
	public float timeToAdd;

	protected override void applyBonus(GameObject _player)
	{
		GameMaster.GM.levelTimer += timeToAdd;
        GameMaster.GM.uiMgr.PickedUpTimerBonus(timeToAdd);
    }
}
