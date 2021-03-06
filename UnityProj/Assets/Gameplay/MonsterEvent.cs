﻿using UnityEngine;
using System.Collections;

public class MonsterEvent : GameEvent {
	public GameObject monster;
	public float monsterRadius;
	public ObjectivePointer pointer;

    public Vector3 maxDistFromDragon;

    private GameObject currentMonster;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Begin()
	{
		//Instantiate the monster
        Vector3 dragonPos = GameMaster.GM.dragon.transform.position;
		Vector3 pos;
		Quaternion rot = Quaternion.Euler(.0f, .0f, .0f);

		do
		{
			pos = new Vector3(Random.Range(dragonPos.x - maxDistFromDragon.x, dragonPos.x + maxDistFromDragon.x), Random.Range(dragonPos.y - maxDistFromDragon.y, dragonPos.y + maxDistFromDragon.y), Random.Range(dragonPos.z - maxDistFromDragon.z, dragonPos.z + maxDistFromDragon.z));
		} while (Physics.CheckSphere(pos, monsterRadius) || (pos.y < dragonPos.y - 5.0f) || (new Vector2(pos.x - dragonPos.x, pos.z - dragonPos.z).magnitude < 5.0f) || (Vector3.Distance(Vector3.zero, pos) >= GameMaster.GM.maxDistDragon - 2.0f));

		currentMonster = Instantiate(monster, pos, rot) as GameObject;
		((Monster)currentMonster.GetComponent<Monster>()).master = this;

        //Start tracking it
        pointer.SetPointer(ObjectivePointer.Pointers.Monster_Pointer);
        pointer.target = currentMonster;
	}

	public override void End(float _minTimeUntilNext = .0f, float _maxTimeUntilNext = .0f)
	{
		pointer.target = null;
		Destroy(currentMonster);
		manager.eventEnded(_minTimeUntilNext, _maxTimeUntilNext);
	}

}
