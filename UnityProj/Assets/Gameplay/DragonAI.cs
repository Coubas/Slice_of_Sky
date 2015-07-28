using UnityEngine;
using System.Collections;

public class DragonAI : MonoBehaviour {
	public bool isSinging = false;
	public ParticleSystem singEffect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GM.gamePaused)
			return;

		if (isSinging && !singEffect.enableEmission)
			singEffect.enableEmission = true;
		else if (!isSinging && singEffect.enableEmission)
			singEffect.enableEmission = false;
	}

	public void startSinging()
	{
		isSinging = true;
	}

	public void stopSinging()
	{
		isSinging = false;
	}
}
