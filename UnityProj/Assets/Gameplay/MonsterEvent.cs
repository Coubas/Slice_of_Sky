using UnityEngine;
using System.Collections;

public class MonsterEvent : GameEvent {
	public GameObject monster;
	public float monsterRadius;
	public ObjectivePointer pointer;

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
		float maxDist = GameMaster.GM.maxDistDecor;
		Vector3 pos;
		Quaternion rot = Quaternion.Euler(.0f, .0f, .0f);

		do
		{
			pos = new Vector3(Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist));
		} while (Physics.CheckSphere(pos, monsterRadius));

		currentMonster = Instantiate(monster, pos, rot) as GameObject;
		((Monster)currentMonster.GetComponent<Monster>()).master = this;

		//Start tracking it
		pointer.target = currentMonster;
	}

	public override void End()
	{
		pointer.target = null;
		Destroy(currentMonster);
		manager.eventEnded();
	}

}
