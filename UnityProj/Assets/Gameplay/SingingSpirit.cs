using UnityEngine;
using System.Collections;

public class SingingSpirit : MonoBehaviour {
	private bool singing = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GM.gamePaused)
			return;

		if (!singing && ((Spirit)GetComponent<Spirit>()).collected)
		{
			((DragonAI)GameMaster.GM.dragon.GetComponent<DragonAI>()).startSinging();
			singing = true;
		}
	}
}
