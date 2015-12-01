using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public float lifeTimeMax;
	private float lifeTime;
	private Vector3 savedVelocity;

    public GameObject HitEffect;

	// Use this for initialization
	void Start ()
	{
	}

	void Update()
	{
		if (GameMaster.GM.gamePaused)
		{
			if (GetComponent<Rigidbody>().velocity != Vector3.zero)
			{
				
                savedVelocity = GetComponent<Rigidbody>().velocity;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                //GetComponent<Rigidbody>().Sleep();
			}
			
			return;
		}
		else if (GetComponent<Rigidbody>().velocity == Vector3.zero)
		{
            if (GetComponent<Rigidbody>().velocity == Vector3.zero)
            {
                //GetComponent<Rigidbody>().WakeUp();
                GetComponent<Rigidbody>().velocity = savedVelocity;
                savedVelocity = Vector3.zero;
            }
        }

		lifeTime += Time.deltaTime;

		if (lifeTime > lifeTimeMax)
		{
			Destroy(gameObject);
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Island"))
        {
            DestroyWithEffect();
        }
    }

    void OnDestroy()
    {
    }

    public void DestroyWithEffect()
    {
        Instantiate(HitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
