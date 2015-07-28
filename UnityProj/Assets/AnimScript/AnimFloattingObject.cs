using UnityEngine;
using System.Collections;

public class AnimFloattingObject : MonoBehaviour
{
	[System.Serializable]
	public class RandBoundary
	{
		public float range;

		public RandBoundary(float _r)
		{
			range = _r;
		}
	}

	[System.Serializable]
	public enum Randomization
	{
		never,
		once,
		everyLoop
	}

	public RandBoundary xTranslate;
	public RandBoundary yTranslate;
	public RandBoundary zTranslate;

	public RandBoundary xRotate;
	public RandBoundary yRotate;
	public RandBoundary zRotate;

	public Randomization randomize;
	public float loopDuration;

	private int step;
	private float stepTimer;
	private float stepDuration;
	private Vector3 minTranslateTarget;
	private Vector3 maxTranslateTarget;
	private Vector3 minRotateTarget;
	private Vector3 maxRotateTarget;
	private Vector3 initialPos;
	private Vector3 initialRot;

	// Use this for initialization
	void Start () {
		stepTimer = .0f;

		initialPos = transform.localPosition;
		initialRot = transform.localEulerAngles;

		defineTarget();
	}
	
	// Update is called once per frame
	void Update () {
		if (stepTimer < stepDuration)
		{
			stepTimer += Time.deltaTime;
			if (stepTimer > stepDuration)
				stepTimer = stepDuration;
		}
		else
		{
			stepTimer = .0f;

			if (step < 3)
			{
				step++;
			}
			else
			{
				step = 0;
				if(randomize == Randomization.everyLoop)
					defineTarget();
			}
		}

		//initial -> max -> initial -> min -> initial
		float t = stepTimer / stepDuration;
		float tBlind;

		if (step == 0)
		{
			tBlind = AIUtils.QuadEaseOut(t, .0f, 1.0f, 1.0f);

			//transform.localPosition = AIUtils.vectorEasing(initialPos, maxTranslateTarget, t, 1.0f, AIUtils.QuadEaseInOut);
			//transform.localEulerAngles = AIUtils.vectorEasing(initialRot, maxRotateTarget, t, 1.0f, AIUtils.QuadEaseInOut);
			transform.localPosition = Vector3.Lerp(initialPos, maxTranslateTarget, tBlind);
			transform.localEulerAngles = Vector3.Lerp(initialRot, maxRotateTarget, tBlind);
		}
		else if (step == 1)
		{
			tBlind = AIUtils.QuadEaseIn(t, .0f, 1.0f, 1.0f);

			//transform.localPosition = AIUtils.vectorEasing(maxTranslateTarget, initialPos, t, 1.0f, AIUtils.QuadEaseInOut);
			//transform.localEulerAngles = AIUtils.vectorEasing(maxRotateTarget, initialRot, t, 1.0f, AIUtils.QuadEaseInOut);
			transform.localPosition = Vector3.Lerp(maxTranslateTarget, initialPos, tBlind);
			transform.localEulerAngles = Vector3.Lerp(maxRotateTarget, initialRot, tBlind);
		}
		else if (step == 2)
		{
			tBlind = AIUtils.QuadEaseOut(t, .0f, 1.0f, 1.0f);

			//transform.localPosition = AIUtils.vectorEasing(initialPos, minTranslateTarget, t, 1.0f, AIUtils.QuadEaseInOut);
			//transform.localEulerAngles = AIUtils.vectorEasing(initialRot, minRotateTarget, t, 1.0f, AIUtils.QuadEaseInOut);
			transform.localPosition = Vector3.Lerp(initialPos, minTranslateTarget, tBlind);
			transform.localEulerAngles = Vector3.Lerp(initialRot, minRotateTarget, tBlind);
		}
		else
		{
			tBlind = AIUtils.QuadEaseIn(t, .0f, 1.0f, 1.0f);

			//transform.localPosition = AIUtils.vectorEasing(minTranslateTarget, initialPos, t, 1.0f, AIUtils.QuadEaseInOut);
			//transform.localEulerAngles = AIUtils.vectorEasing(minRotateTarget, initialRot, t, 1.0f, AIUtils.QuadEaseInOut);
			transform.localPosition = Vector3.Lerp(minTranslateTarget, initialPos, tBlind);
			transform.localEulerAngles = Vector3.Lerp(minRotateTarget, initialRot, tBlind);
		}
	}

	void defineTarget()
	{
		if (randomize != Randomization.never)
		{
			stepDuration = Random.Range(loopDuration * .75f, loopDuration * 1.25f) * .25f;

			float xT = Random.Range(.0f, xTranslate.range);
			float yT = Random.Range(.0f, yTranslate.range);
			float zT = Random.Range(.0f, zTranslate.range);
			float xR = Random.Range(.0f, xRotate.range);
			float yR = Random.Range(.0f, yRotate.range);
			float zR = Random.Range(.0f, zRotate.range);

			minTranslateTarget = new Vector3(initialPos.x - xT, initialPos.y - yT, initialPos.z - zT);
			maxTranslateTarget = new Vector3(initialPos.x + xT, initialPos.y + yT, initialPos.z + zT);

			minRotateTarget = new Vector3(initialRot.x - xR, initialRot.y - yR, initialRot.z - zR);
			maxRotateTarget = new Vector3(initialRot.x + xR, initialRot.y + yR, initialRot.z + zR);
		}
		else
		{
			stepDuration = loopDuration * .25f;

			minTranslateTarget = new Vector3(initialPos.x - xTranslate.range, initialPos.y - yTranslate.range, initialPos.z - zTranslate.range);
			maxTranslateTarget = new Vector3(initialPos.x + xTranslate.range, initialPos.y + yTranslate.range, initialPos.z + zTranslate.range);

			minRotateTarget = new Vector3(initialRot.x - xRotate.range, initialRot.y - yRotate.range, initialRot.z - zRotate.range);
			maxRotateTarget = new Vector3(initialRot.x + xRotate.range, initialRot.y + yRotate.range, initialRot.z + zRotate.range);
		}
		
	}
}
