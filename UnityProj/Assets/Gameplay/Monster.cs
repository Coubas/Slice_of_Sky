﻿using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {
	public MonsterEvent master;
	public float fearDist;

	// Use this for initialization
	void Start () {
		afraidSpirits(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GM.gamePaused)
			return;

		float distFromPlayer = Vector3.Distance(transform.position, GameMaster.GM.dragon.transform.position);
		if (distFromPlayer < fearDist)
		{
			afraidSpirits(false);
			master.End();
		}

	}

	void afraidSpirits( bool _afraid)
	{
		for (int i = 0; i < GameMaster.GM.spiritCount; ++i)
		{
			Spirit spirit = ((GameObject) GameMaster.GM.spirits[i]).GetComponent<Spirit>() as Spirit;
			
			if (spirit.collected)
				continue;

			if (_afraid)
				spirit.setPosBeforeAfraid();
			spirit.afraid = _afraid;
		}
	}
}