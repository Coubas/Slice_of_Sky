using UnityEngine;
using System.Collections;

public class DragonCamera : MonoBehaviour
{

	public GameObject target;
	public float posDamping = 1.0f;
	public float yDamping = .5f;

	private Vector3 offset;
	private float yVelocity = .0f;

	public ParticleSystem speedEffect;

	private int activeCamera;

	void Start()
	{
		offset = target.transform.position - transform.position;

		activeCamera = 0;
		GameObject.Find("HeadCamera").GetComponent<Camera>().enabled = false;

        ParticleSystem.EmissionModule em = speedEffect.emission;
        em.enabled = false;
    }

	void FixedUpdate()
	{
		if (GameMaster.GM.gamePaused)
			return;

		float currentAngleY = transform.eulerAngles.y;
		float desiredAngleY = target.transform.eulerAngles.y;
		float angleY = Mathf.SmoothDampAngle(currentAngleY, desiredAngleY, ref yVelocity, yDamping);

		float angleX;
		if (target.transform.forward.y < .0f)
		{
			angleX = target.transform.eulerAngles.x;
			if (angleX > 55.0f)
				angleX = 55.0f;
		}
		else
		{
			angleX = .0f;
		}

		Quaternion rotationY = Quaternion.Euler(angleX, angleY, .0f);

		Vector3 desiredPosition = target.transform.position - (rotationY * offset);
		Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.fixedDeltaTime * posDamping);
		transform.position = position;

		transform.LookAt(target.transform);

		//Speed effect
		//if (target.GetComponent<PlayerController>().speedUpCursor > 1.8f && !speedEffect.enableEmission)
		//{
		//	speedEffect.enableEmission = true;
		//}
		//else if (target.GetComponent<PlayerController>().speedUpCursor < 2.5f && speedEffect.enableEmission)
		//{
		//	speedEffect.enableEmission = false;
		//}

		if (target.GetComponent<PlayerController>().speedBoostCursor > .3f && !speedEffect.emission.enabled)
		{
            ParticleSystem.EmissionModule em = speedEffect.emission;
            em.enabled = true;
        }
		else if (target.GetComponent<PlayerController>().speedBoostCursor < .3f && speedEffect.emission.enabled)
		{
            ParticleSystem.EmissionModule em = speedEffect.emission;
            em.enabled = false;
		}

		if (Input.GetKeyDown("tab"))
		{
			switch(activeCamera)
			{
				case 0:
					this.GetComponent<Camera>().enabled = false;
					GameObject.Find("HeadCamera").GetComponent<Camera>().enabled = true;
					activeCamera = 1;
					break;
				case 1:
					this.GetComponent<Camera>().enabled = true;
					GameObject.Find("HeadCamera").GetComponent<Camera>().enabled = false;
					activeCamera = 0;
					break;
			}
		}
	}
}