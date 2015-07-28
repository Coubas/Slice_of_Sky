using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float lifeTimeMax;
	private float lifeTime;
	private Vector3 savedVelocity;

	// Use this for initialization
	void Start ()
	{
	}

	void Update()
	{
		if (GameMaster.GM.gamePaused)
		{
			if (!GetComponent<Rigidbody>().IsSleeping())
			{
				savedVelocity = GetComponent<Rigidbody>().velocity;
				GetComponent<Rigidbody>().Sleep();
			}
			
			return;
		}
		else if (GetComponent<Rigidbody>().IsSleeping())
		{
			GetComponent<Rigidbody>().WakeUp();
			GetComponent<Rigidbody>().velocity = savedVelocity;
		}

		lifeTime += Time.deltaTime;

		if (lifeTime > lifeTimeMax)
		{
			Destroy(gameObject);
		}
	}
}
