using UnityEngine;
using System.Collections;

public class Spirit : MonoBehaviour {
	[System.Serializable]
	public enum SpiritType
	{
		blue = 0,
		yellow = 1,
		red = 2,
		green = 3,
		purple = 4
	}

	public SpiritType spiritType;
	public int value;
	public bool collected = false;
	public bool afraid = false;
	public bool freezed = false;

	private bool fallingSpirit = true;
	public float fallingSpeedMin;
	public float fallingSpeedMax;
	public float maxWaitingBeforeFall;
	private float fallingSpeed;
	private float waitBeforeFall;

    private float maxDist;
    private Vector3 newDestination;
    private Vector3 destination;
    private float turnTimer;
    public float speedMin;
    public float speedMax;
    private float speed;
    
	public float afraidDist;
	public float afraidSpeed;
	public Vector3 posBeforeAfraid;
	public Vector3 afraidTarget;

	public float freezeDuration;
	private float unfreezeTime;
    public Transform freezeEffect;
    private Vector3 initnialScale;

    public float collectedRadius;
    public float collectedRotSpeed;
    private float collectedTimer;

	void Start () {
		GameMaster.GM.spiritCount++;
		GameMaster.GM.spirits.Add(gameObject);

		//fallingSpeed = Random.Range(fallingSpeedMin, fallingSpeedMax);
		//waitBeforeFall = Random.Range(.0f, maxWaitingBeforeFall);

        maxDist = GameMaster.GM.maxDistDecor;
        speed = Random.Range(speedMin, speedMax);
        destination = getDestination();

        collectedTimer = .0f;

        initnialScale = freezeEffect.localScale;
    }
	
	void Update () 
	{
		if (GameMaster.GM.gamePaused)
			return;

		if (collected)
        {
            collectedTimer += Time.deltaTime;
            Vector3 pos = transform.parent.position;
            Vector3 parentUp = transform.parent.up;
            Vector3 up = Quaternion.AngleAxis(collectedRotSpeed * collectedTimer, transform.parent.right) * parentUp;
            transform.position = pos + up * collectedRadius;
			return;
        }

		if (freezed)
		{
			if (Time.time > unfreezeTime)
            {
				freezed = false;
                freezeEffect.gameObject.SetActive(false);
                freezeEffect.localScale = initnialScale;
            }
			else
            {
                //Scale down the freeze effect over time
                float leftDuration = unfreezeTime - Time.time;
                float scale = Mathf.Lerp(0.2f, 1.0f, leftDuration / freezeDuration);
                freezeEffect.localScale = initnialScale * scale;

                return;
            }
		}

		if (afraid)
		{
			makeAfraidMove();
			return;
		}

		if (fallingSpirit)
		{
            //         //Make the spirit fall
            //if (waitBeforeFall <= .0f)
            //	fall(fallingSpeed);
            //else
            //	waitBeforeFall -= Time.deltaTime;

            //if (transform.position.y < -GameMaster.GM.maxDistDecor)
            //{
            //	transform.Translate(.0f, GameMaster.GM.maxDistDecor * 2.0f, .0f, Space.World);
            //}

            Vector3 toDest = destination - transform.position;
            toDest.Normalize();

            float dist = Vector3.Distance(transform.position, destination);
            if (newDestination == Vector3.zero)
            {
                if (dist < 10.0f)
                {
                    do
                    {
                        newDestination = getDestination();
                    } while (Vector3.Distance(transform.position, newDestination) < 5.0f);

                    turnTimer = 2.0f;
                }

                transform.LookAt(transform.position + toDest * 2.0f);

                Quaternion xRot = Quaternion.AngleAxis(25.0f, Vector3.Cross(toDest, Vector3.up));
                Quaternion roundRot = Quaternion.AngleAxis(Mathf.RoundToInt(Time.time * 272.0f) % 360, toDest);
                toDest = xRot * toDest;
                toDest = roundRot * toDest;
                toDest.Normalize();
                transform.Translate(toDest * speed, Space.World);
            }
            else if(newDestination != Vector3.zero && turnTimer > .0f)
            {
                Vector3 toNewDest = newDestination - transform.position;
                toNewDest.Normalize();

                Vector3 smoothTurn = Vector3.Lerp(toDest, toNewDest, 1.0f - (turnTimer * 0.5f));
                smoothTurn.Normalize();

                transform.LookAt(transform.position + smoothTurn * 2.0f);

                Quaternion xRot = Quaternion.AngleAxis(25.0f, Vector3.Cross(smoothTurn, Vector3.up));
                Quaternion roundRot = Quaternion.AngleAxis(Mathf.RoundToInt(Time.time * 272.0f) % 360, smoothTurn);
                smoothTurn = xRot * smoothTurn;
                smoothTurn = roundRot * smoothTurn;
                smoothTurn.Normalize();
                transform.Translate(smoothTurn * speed, Space.World);

                turnTimer -= Time.deltaTime;
            }
            else
            {
                destination = newDestination;
                newDestination = Vector3.zero;
            }
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !collected && (!afraid || freezed))
		{
			other.GetComponent<SnakeAI>().addSpirit(gameObject);
		}
		else if (other.CompareTag("Projectile") && !collected)
		{
            if(!GetComponent<FleeingSpirit>())
            {
			    freezed = true;
			    unfreezeTime = Time.time + freezeDuration;
                freezeEffect.gameObject.SetActive(true);
            }

            Destroy(other.gameObject);
		}
        else if (other.CompareTag("Island"))
        {
            destination = getDestination();
        }
	}

	void fall(float _speed)
	{
		if (!collected)
			transform.Translate(.0f, -_speed * Time.timeScale, .0f, Space.World);
	}

	public void setFalling(bool _falling)
	{
		fallingSpirit = _falling;
	}

	public void setPosBeforeAfraid()
	{
		posBeforeAfraid = transform.position;
		getAfraidTarget();
	}

    Vector3 getDestination()
    {
        return new Vector3(Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist), Random.Range(-maxDist, maxDist));
    }

	void makeAfraidMove()
	{
		float dist = Vector3.Distance(transform.position, afraidTarget);
		if (dist > Random.Range(0.1f, afraidDist * .75f))
		{
			Vector3 dir = afraidTarget - transform.position;
            //Debug.DrawRay(transform.position, dir, Color.green);
            //dir.Normalize();
            transform.LookAt(afraidTarget);
			transform.Translate(dir * afraidSpeed, Space.World);
		}
		else
		{
			getAfraidTarget();		
		}
	}

	void getAfraidTarget()
	{
		afraidTarget = new Vector3(Random.Range(posBeforeAfraid.x - afraidDist, posBeforeAfraid.x + afraidDist), Random.Range(posBeforeAfraid.y - afraidDist, posBeforeAfraid.y + afraidDist), Random.Range(posBeforeAfraid.z - afraidDist, posBeforeAfraid.z + afraidDist));
	}
}
