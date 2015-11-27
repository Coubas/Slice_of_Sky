using UnityEngine;
using System.Collections;

public class FleeingSpirit : MonoBehaviour {
	public float speed;
	public float distanceOfFlee;
    
    private float wasFleeingTimer;
	private Vector3 initialPos;
	// Use this for initialization
	void Start () {
		initialPos = transform.position;
		((Spirit)GetComponent<Spirit>()).setFalling(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GM.gamePaused)
			return;

		if (((Spirit)GetComponent<Spirit>()).afraid)
			return;

		if (!((Spirit)GetComponent<Spirit>()).collected && !((DragonAI) GameMaster.GM.dragon.GetComponent<DragonAI>()).isSinging)
		{
			float distance = Vector3.Distance(transform.position, GameMaster.GM.dragon.transform.position);

			if (distance < distanceOfFlee)
			{
				Flee();
                wasFleeingTimer = 1.0f;
			}
            else if(wasFleeingTimer > .0f)
            {
                wasFleeingTimer -= Time.deltaTime;
                Flee();
            }
			//else
			//{
			//	backToInitialPos();
			//}
		}
	}

	private void Flee()
	{
		Vector3 dir;
		Vector3 realDir;
		//Pick Up a direction
		dir = GameMaster.GM.dragon.transform.forward;
		realDir = transform.position - GameMaster.GM.dragon.transform.position;
		realDir.y = .0f;

		if (Vector3.Dot(realDir, dir) > 0.0f)
		{
			//Don't let it go to far away
			float distFromMid = Vector3.Distance(transform.position, Vector3.zero);
			float maxDist = (GameMaster.GM.maxDistDecor + GameMaster.GM.maxDistDragon) / 2.0f;
			float multiplier = distFromMid / maxDist;
			Vector3 dirToMid = Vector3.zero - transform.position;
			dirToMid.y = .0f;

            //if (multiplier >= 0.75f)
            {
                realDir = realDir + dirToMid * (multiplier + 0.1f);
                realDir.Normalize();
            }

            transform.LookAt(transform.position + realDir);

            Quaternion xRot = Quaternion.AngleAxis(25.0f, Vector3.Cross(realDir, Vector3.up));
            Quaternion roundRot = Quaternion.AngleAxis(Mathf.RoundToInt(Time.time * 272.0f) % 360, realDir);
            realDir = xRot * realDir;
            realDir = roundRot * realDir;
            realDir.Normalize();
			transform.Translate(realDir * speed * Time.timeScale, Space.World);
		}
	}

	private void backToInitialPos()
	{
		float distance = Vector3.Distance(transform.position, initialPos);

		if (distance > 1.0f)
		{
			Vector3 dir = initialPos - transform.position;
			dir.Normalize();
			transform.Translate(dir * speed * 0.5f * Time.timeScale, Space.World);
		}
	}
}
