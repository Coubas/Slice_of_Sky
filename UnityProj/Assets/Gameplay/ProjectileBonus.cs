using UnityEngine;
using System.Collections;

public class ProjectileBonus : Bonus
{
    public int nbAmmoRecharge;

	protected override void applyBonus(GameObject _player)
	{
        PlayerData.PD.ammoCount += nbAmmoRecharge;
        GameMaster.GM.uiMgr.PickedUpProjectileBonus();
    }
}
