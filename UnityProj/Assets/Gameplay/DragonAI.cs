using UnityEngine;
using System.Collections;

public class DragonAI : MonoBehaviour {
	public bool isSinging = false;
	public ParticleSystem singEffect;

	// Use this for initialization
	void Start () {
        ParticleSystem.EmissionModule em = singEffect.emission;
        em.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GM.gamePaused)
			return;

		if (isSinging && !singEffect.emission.enabled)
        {
            ParticleSystem.EmissionModule em = singEffect.emission;
            em.enabled = true;
        }
		else if (!isSinging && singEffect.emission.enabled)
        {
            ParticleSystem.EmissionModule em = singEffect.emission;
            em.enabled = false;
        }
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
