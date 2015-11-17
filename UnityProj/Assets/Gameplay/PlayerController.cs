using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public GUIText debugText;

	public float speed;
	public float hRotSpeed;
	public float vRotSpeed;
	public float upAngle;
	public float downAngle;
	public GameObject dragonHead;

	//Inputs
	private Vector3 savedVelocity;
	private Vector3 referenceRotation;
	private bool refRotSet;
	private float xSmooth, xDir, xTimer;
	private float ySmooth, yDir, yTimer;
	private float prevX = .0f, prevY = .0f;
	private float xCursor;
	private float xCursorTimer;
	private float yCursor;
	private float yCursorTimer;

	//Stream
	public bool isInStream;
	public float streamModifier;
	private float streamCursor;
	private float inStreamTimer;

	//Adjust xRotation (out of stream an bounce)
	private bool adjustXRot;
	private float adjustXRotCursor;
	private float adjustXRotTimer;

	//Speed up
	public float speedUpCursor;
	private float goingDownTimer;

	public float boostedSpeed;
	public float normalSpeed;
	public float speedBoostCursor;
	private float speedBoostTimer;
	//public float speedAmount = 1.0f;
	//private bool speedBoostLock = false;
	public SpeedBoostGauge gauges;

	//Avoid body collisions
	public bool collisionToAvoid = false;
	public bool avoidMade = true;
	public Vector3 bodyPartPos;
	public Vector3 bodyPartForward;
	public float avoidingRadius;
	private Vector3 avoidPoint = Vector3.zero;
	private bool avoidPointReach;
	private Vector3 nextPoint = Vector3.zero;
	private float oldForwardSmoothCursor;
	private Vector3 smoothVelocity;
	private Vector3 oldForward;

	//Projectiles
	public GameObject projectile;
	public Transform projectileSpawner;
	public float shootTimer;
	public float shootForce;
	private float nextShotTimer;

	void Start () {
#if UNITY_ANDROID
    		Input.gyro.enabled = true;
	    	refRotSet = false;
#endif

		xCursor = .0f;
		xCursorTimer = .0f;
		yCursor = .0f;
		yCursorTimer = .0f;

		streamCursor = .0f;
		inStreamTimer = .0f;
		adjustXRot = false;
		adjustXRotCursor = .0f;
		adjustXRotTimer = .0f;

		speedUpCursor = 1.0f;
		goingDownTimer = .0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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

		//------------------------------------------------------------------------------------------------------------------------------------------------------------
		//Smooth inputs
		//------------------------------------------------------------------------------------------------------------------------------------------------------------
#if UNITY_ANDROID
#if UNITY_EDITOR
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");
#else
		//Gyroscope
		//if (!refRotSet && (ConvertRotation(Input.gyro.attitude) * GetRotFix()).eulerAngles != Vector3.zero)
		//{
		//	referenceRotation = (ConvertRotation(Input.gyro.attitude) * GetRotFix()).eulerAngles;
		//	referenceRotation = new Vector3(referenceRotation.x, correctPitch(referenceRotation.y), correctRoll(referenceRotation.z));
		//	refRotSet = true;
		//}

		//Vector3 deviceRotation = (ConvertRotation(Input.gyro.attitude) * GetRotFix()).eulerAngles;
		//deviceRotation = new Vector3(deviceRotation.x, correctPitch(deviceRotation.y), correctRoll(deviceRotation.z));
		//Vector3 rotFromRef = deviceRotation - referenceRotation;

		//float roll = rotFromRef.z;
		//float pitch = rotFromRef.y;

		//float x = roll / 45.0f;
		//float y = pitch / 90.0f;
		//debugText.text = "refRot = " + referenceRotation + "\ncurrentRot = " + deviceRotation + "\ndifRot = " + rotFromRef + " roll = " + x + " pitch = " + y;

		//Accelerometre
		if (!refRotSet)
		{
			referenceRotation = Input.acceleration;
			refRotSet = true;
		}

		float x = Input.acceleration.x - referenceRotation.x;
		float y = Input.acceleration.z - referenceRotation.z;

		//X Smooth
		if (x >= 0.005f || x <= 0.005f)
		{
			float currentXDir = .0f;
			if (x > .0f)
				currentXDir = 1.0f;
			else if (x < .0f)
				currentXDir = -1.0f;

			if (currentXDir != xDir)
			{
				xDir = currentXDir;
				xTimer = Time.fixedDeltaTime;
			}
			else
			{
				xTimer += Time.fixedDeltaTime;
			}

			xSmooth = AIUtils.QuadEaseOut(Mathf.Abs(x), .0f, 1.0f, 1.0f);
			if (xSmooth > 1.0f)
				xSmooth = 1.0f;
		}
		else
		{
			xDir = .0f;
			xTimer = .0f;
			xSmooth = .0f;
		}

		//Y Smooth
		if (y >= 0.005f || y <= 0.005f)
		{
			float currentYDir = .0f;
			if (y > .0f)
				currentYDir = 1.0f;
			else if (y < .0f)
				currentYDir = -1.0f;

			if (currentYDir != yDir)
			{
				yDir = currentYDir;
				yTimer = Time.fixedDeltaTime;
			}
			else
			{
				yTimer += Time.fixedDeltaTime;
			}

			ySmooth = AIUtils.QuadEaseOut(Mathf.Abs(y), .0f, 1.0f, 1.0f);
			if (ySmooth > 1.0f)
				ySmooth = 1.0f;
		}
		else
		{
			yDir = .0f;
			yTimer = .0f;
			ySmooth = .0f;
		}

		x *= xSmooth;
		y *= ySmooth;

		debugText.text = "refRot = " + referenceRotation + " roll = " + x + " pitch = " + y;
		
#endif
#else
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");
#endif

		if (PlayerData.PD.invertYAxis)
			y = -y;


		if (x != .0f)
		{
			if (x * prevX < .0f) //Not the same sign
			{
				xCursor = .0f;
				xCursorTimer = .0f;
			}

			xCursorTimer += Time.fixedDeltaTime;
			if(xCursorTimer > 1.0f)
				xCursorTimer = 1.0f;
			//xCursor = AIUtils.easingFunction(xCursor, xCursorTimer, 5.0f);
			xCursor = AIUtils.QuadEaseOut(xCursorTimer, .0f, x, 1.0f);
			if (xCursor > 1.0f)
				xCursor = 1.0f;
		}
		else
		{
			xCursor = .0f;
			xCursorTimer = .0f;
		}

		if (y != .0f)
		{
			if (y * prevY < .0f) //Not the same sign
			{
				yCursor = .0f;
				yCursorTimer = .0f;
			}

			yCursorTimer += Time.fixedDeltaTime;
			if(yCursorTimer > 1.0f)
				yCursorTimer = 1.0f;
			//yCursor = AIUtils.easingFunction(yCursor, yCursorTimer, 10.0f);
			yCursor = AIUtils.QuadEaseOut(yCursorTimer, .0f, y, 1.0f);
			if (yCursor > 1.0f)
				yCursor = 1.0f;
		}
		else
		{
			yCursor = .0f;
			yCursorTimer = .0f;
		}

		prevX = x;
		prevY = y;

		x *= Mathf.Abs(xCursor);
		y *= Mathf.Abs(yCursor);

		//------------------------------------------------------------------------------------------------------------------------------------------------------------
		//Rotate
		//------------------------------------------------------------------------------------------------------------------------------------------------------------

		bool turnOnX = true;

		//Stream handling
		if (isInStream)
		{
			if (adjustXRot)
				adjustXRot = false;

			inStreamTimer += Time.fixedDeltaTime;
			streamCursor = AIUtils.ExpoEaseOut(inStreamTimer, .0f, streamModifier, 10.0f);
			if (streamCursor > streamModifier)
				streamCursor = streamModifier;
		}
		else if (inStreamTimer != .0f)
		{
			inStreamTimer = .0f;
			streamCursor = .0f;

			adjustXRot = true;
			adjustXRotTimer = .0f;
			adjustXRotCursor = .0f;
		}

		//Debug.Log((upAngle+streamCursor));

		//On bloque la rotation en foction des paramètres
		if (transform.forward.y > 0.0f)
		{
			if (transform.eulerAngles.x > 0.001f && transform.eulerAngles.x <= (360 - (upAngle + streamCursor)) && y >= .0f)
			{
				turnOnX = false;
			}
		}
		else
		{
			if (transform.eulerAngles.x >= downAngle && y < .0f)
			{
				turnOnX = false;
			}
		}

		//Check if the dragon is about to hit himself
		if (collisionToAvoid)
		{
			//Avoid collisions with bodypart
			if (!avoidMade)
			{
				oldForward = transform.forward;
				float dist = Vector3.Distance(transform.position, bodyPartPos);
				Vector3 collisionPos = transform.position + dist * transform.forward.normalized;

				Vector3 avoidDir = Vector3.Cross(bodyPartPos - transform.position, bodyPartForward);
				avoidDir.Normalize();
				avoidPoint = collisionPos + avoidingRadius * avoidDir.normalized;

				Vector3 continueDir = collisionPos - transform.position;
				nextPoint = collisionPos + continueDir;
				oldForwardSmoothCursor = .0f;

				avoidMade = true;
				avoidPointReach = false;
				smoothVelocity = Vector3.zero;

				//Debug.DrawRay(collisionPos, avoidingRadius * avoidDir.normalized, Color.blue);
				//Debug.DrawRay(collisionPos, continueDir, Color.yellow);
				//UnityEditor.EditorApplication.isPaused = true;
			}

			Vector3 target = Vector3.zero;
			if (!avoidPointReach && Vector3.Distance(transform.position, avoidPoint) > 0.2f)
			{
				target = avoidPoint - transform.position;
				target.Normalize();
			}
			else if (avoidPointReach || Vector3.Distance(transform.position, avoidPoint) <= 0.2f)
			{
				if (!avoidPointReach)
					avoidPointReach = true;

				target = (nextPoint + oldForwardSmoothCursor * oldForward) - transform.position;
				target.Normalize();
				oldForwardSmoothCursor += .1f;
			}

			Vector3 newForward = Vector3.SmoothDamp(transform.forward.normalized, target.normalized, ref smoothVelocity, 0.1f);
			newForward.Normalize();

			Vector3 vNextPoint = nextPoint - transform.position;
			if (Vector3.Dot(target, vNextPoint) <= 0)
			{
				transform.forward = oldForward;
				collisionToAvoid = false;
			}
			else
			{
				transform.forward = newForward.normalized;
			}
		}
		else
		{
			if (GameMaster.GM.levelTimer > .0f)
			{
				if (turnOnX)
				{
					if (adjustXRot)
						adjustXRot = false;

					transform.Rotate(-y * vRotSpeed, .0f, .0f, Space.Self);
				}
				else
				{
					if (adjustXRot && transform.forward.y > .0f)
					{
						adjustXRotTimer += Time.fixedDeltaTime;
						adjustXRotCursor = AIUtils.QuadEaseIn(adjustXRotTimer, .0f, 1.0f, 4.0f);
						if (adjustXRotCursor > 1.0f)
							adjustXRotCursor = 1.0f;

						transform.Rotate(adjustXRotCursor * vRotSpeed, .0f, .0f, Space.Self);
					}
				}

				transform.Rotate(.0f, x * hRotSpeed, .0f, Space.World);
			}
		}
		

		//------------------------------------------------------------------------------------------------------------------------------------------------------------
		//Move
		//------------------------------------------------------------------------------------------------------------------------------------------------------------
		//Debug.Log(transform.forward.y);

		//Speed up boost
		if (Input.GetButton("Jump") == true && gauges.CanUseBoost())
		{
			if (speedBoostTimer < 1.0f) speedBoostTimer += Time.fixedDeltaTime;
			speedBoostCursor = AIUtils.QuadEaseOut(speedBoostTimer, 0.0f, 1.0f, 1.0f);

			gauges.UseBoost();
		}
		else
		{
			if (speedBoostTimer > 0.0f) speedBoostTimer -= Time.fixedDeltaTime;
			speedBoostCursor = AIUtils.QuadEaseIn(speedBoostTimer, 0.0f, 1.0f, 1.0f);

			gauges.ReloadGauges();
		}
		
		speed = normalSpeed + boostedSpeed * speedBoostCursor;

		//Speed up when going down
		//if (transform.forward.y <= -0.4)
		//{
		//	if (speedUpCursor <= 3.0f) goingDownTimer += Time.fixedDeltaTime;
		//	speedUpCursor = AIUtils.QuadEaseIn(goingDownTimer, 1.0f, 3.0f, 3.0f);
		//}
		//else if (speedUpCursor - 1.0f > .01f)
		//{
		//	goingDownTimer -= Time.fixedDeltaTime;
		//	speedUpCursor = AIUtils.QuadEaseIn(goingDownTimer, 1.0f, 3.0f, 3.0f);
		//}
		//else
		//{
		//	speedUpCursor = 1.0f;
		//	goingDownTimer = .0f;
		//}
		//Debug.Log(speedUpCursor);

		//Teleport back when close to the world limit
		float distance = Vector3.Distance(Vector3.zero, transform.position);
		if (distance > GameMaster.GM.maxDistDragon)
		{
			Vector3 modifier = transform.forward * 50.0f;
			Vector3 newPos = transform.position - modifier;

			GetComponent<SnakeAI>().updateBodipartPos(modifier);
			Camera.main.transform.parent = transform;
			transform.position = newPos;
			Camera.main.transform.parent = null;
		}

		if (GameMaster.GM.levelTimer > .0f || GameMaster.GM.spiritCount == 0)
		{
			GetComponent<SnakeAI>().setHeadPrevPos(transform.position, transform.forward);
			//transform.Translate(transform.forward * speed * speedUpCursor, Space.World);
			GetComponent<Rigidbody>().velocity = transform.forward * speed * speedUpCursor;
			GetComponent<SnakeAI>().placeBodyPart(speed);
		}
		else
		{
			GetComponent<Rigidbody>().velocity = transform.forward * .0f;
		}

		//float speedMult = 1.0f / Time.deltaTime * 60 * 60;
		//Debug.Log("Speed : " + Mathf.RoundToInt((speed * speedUpCursor * speedMult) / 1000.0f) +"km/h");

		dragonHead.transform.position = transform.position;
		dragonHead.transform.rotation = transform.rotation;
		dragonHead.transform.Rotate(new Vector3(.0f, 0.0f, -90.0f));
		dragonHead.transform.Rotate(new Vector3(.0f, -90.0f, 0.0f));

        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Shoot
        //------------------------------------------------------------------------------------------------------------------------------------------------------------
        if (Input.GetButton("Fire1") && Time.time > nextShotTimer)
		{
			nextShotTimer = Time.time + shootTimer;
			GameObject clone = Instantiate(projectile, projectileSpawner.position, projectileSpawner.rotation) as GameObject;
			clone.GetComponent<Rigidbody>().AddForce(clone.transform.forward * shootForce);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			if (!collision.collider.CompareTag("SmallObject"))
			{
				//Debug.DrawRay(contact.point, contact.normal*10, Color.green, 1000.0f);
			
				GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

				Vector3 projectionOntheNormal = contact.normal * Vector3.Dot(transform.forward, contact.normal);
				Vector3 newForward = transform.forward - projectionOntheNormal;
				transform.forward = newForward;

				//Debug.DrawRay(contact.point, contact.normal * 10, Color.green);
				//Debug.DrawRay(contact.point, projectionOntheNormal * 10, Color.blue);
				//Debug.DrawRay(contact.point, newForward * 10, Color.red);
				//UnityEditor.EditorApplication.isPaused = true;
			}


			if (transform.forward.y > .0f)
			{
				adjustXRot = true;
				adjustXRotTimer = .0f;
				adjustXRotCursor = .0f;
			}
		}
	}

	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);
	}

	private Quaternion GetRotFix()
	{
		if (Screen.orientation == ScreenOrientation.Portrait)
			return Quaternion.identity;
		if (Screen.orientation == ScreenOrientation.LandscapeLeft
		|| Screen.orientation == ScreenOrientation.Landscape)
			return Quaternion.Euler(0, 0, -90);
		if (Screen.orientation == ScreenOrientation.LandscapeRight)
			return Quaternion.Euler(0, 0, 90);
		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
			return Quaternion.Euler(0, 0, 180);
		return Quaternion.identity;
	}

	private float correctRoll(float _roll)
	{
		if (_roll > .0f && _roll <= 180.0f)
		{
			return -_roll;
		}
		else
		{
			return 360.0f - _roll;
		}
	}

	private float correctPitch(float _pitch)
	{
		if (_pitch > .0f && _pitch <= 180.0f)
		{
			return -_pitch;
		}
		else
		{
			return 360.0f - _pitch;
		}
	}
}
