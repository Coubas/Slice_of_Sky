﻿using UnityEngine;
using System.Collections;

public class SpiritGate : MonoBehaviour {
	private Transform allTheGatePart;
	// Use this for initialization
	void Start () {
		allTheGatePart = transform.parent.parent;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GM.gamePaused)
			return;

		float distance = Vector3.Distance(allTheGatePart.position, GameMaster.GM.dragon.transform.position);
		if(distance > 25.0f)
			allTheGatePart.LookAt(GameMaster.GM.dragon.transform);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Body") && other.GetComponentInChildren<Spirit>())
		{
			GameObject spirit = other.GetComponentInChildren<Spirit>().gameObject;
			if(spirit.GetComponent<SingingSpirit>())
				((DragonAI)GameMaster.GM.dragon.GetComponent<DragonAI>()).stopSinging();
			freeSpririt(spirit.GetComponent<Spirit>().value, (int)spirit.GetComponent<Spirit>().spiritType);
			GameMaster.GM.spirits.Remove(spirit);
			GameMaster.GM.spiritCount--;
			Destroy(spirit);
		}
	}

	void freeSpririt(int _points, int _spiritType)
	{
		GameMaster.GM.addScore(_points, _spiritType);
	}
}
