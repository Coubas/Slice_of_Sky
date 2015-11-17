using UnityEngine;
using System.Collections;

public class SnakeAI : MonoBehaviour
{
	//Body composition
	public GameObject prefabBodyPart;
	public int initialLenght;
	public float bodyMargin;
	public int spiritSpacing = 1;

	private int currentLenght;
	//private ArrayList body;
	public GameObject[] body;
	private ArrayList positions;
	private ArrayList forwards;
	private int maxLenght;

	void Start()
	{
		//body = new ArrayList();
		positions = new ArrayList();
		positions.Add(transform.position);

		forwards = new ArrayList();
		forwards.Add(transform.forward);

		//for (int i = 0; i < initialLenght; ++i)
		//{
		//	//GameObject bodyPart = (GameObject)Instantiate(prefabBodyPart);
		//	GameObject bodyPart = (GameObject)Instantiate(body[i]);
		//	bodyPart.name = "bodyPart " + i;
		//	//body.Add(bodyPart);
		//	body[i] = bodyPart;
		//}

		currentLenght = initialLenght;

		float mult = 1.0f;
		if (bodyMargin > 1.0f)
			mult = bodyMargin;

		maxLenght = Mathf.FloorToInt((currentLenght + 2) * mult);
	}

	void Update()
	{
		
	}

	public void setHeadPrevPos( Vector3 _prevPos, Vector3 _forward)
	{
		float dist = Vector3.Distance((Vector3)positions[0], _prevPos);

		if (dist == 1)
		{
			positions.Insert(0, _prevPos);
			forwards.Insert(0, _forward);

			int count = positions.Count;
			if (count > maxLenght)
			{
				positions.RemoveAt(count - 1);
				forwards.RemoveAt(count - 1);
			}
		}
		else if (dist > 1)
		{
			Vector3 v = _prevPos - (Vector3)positions[0];
			float dot = Vector3.Dot(v, v.normalized);

			positions.Insert(0, (Vector3)positions[0] + v.normalized);

			Vector3 forward = Vector3.Lerp((Vector3)forwards[0], _forward, 1 / dot);
			forwards.Insert(0, forward);

			int count = positions.Count;
			if (count > maxLenght)
			{
				positions.RemoveAt(count - 1);
				forwards.RemoveAt(count - 1);
			}
			setHeadPrevPos(_prevPos, _forward);
		}
	}

	//Scipt called by the head after her move to update the snake parts
	public void placeBodyPart( float _speed)
	{
		for (int i = 0; i < currentLenght; ++i)
		{
			GameObject bodyPart = (GameObject)body[i];
			float dist = ((i+1) * bodyMargin) - Vector3.Distance(transform.position, (Vector3)positions[0]);

			float index = dist;// *_speed;

			if(index < 0)
			{
				float t = Mathf.Abs(index);

				//Position
				Vector3 pointBefore = transform.position;
				Vector3 pointAfter = (Vector3)positions[0];

                float distBeforeAfter = Vector3.Distance(pointBefore, pointAfter);
                t /= distBeforeAfter;
				
				Vector3 targetPos = Vector3.Lerp(pointAfter, pointBefore, t);
				
				//Forward
				Vector3 forwardBefore = transform.forward;
				Vector3 forwardAfter = (Vector3)forwards[0];
				
				Vector3 targetForw = Vector3.Lerp(forwardAfter, forwardBefore, t);
				
				bodyPart.transform.forward = targetForw;
				bodyPart.transform.position = targetPos;
				bodyPart.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
				bodyPart.transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f));
			}
			else
			{
				if (positions.Count > Mathf.FloorToInt(index) + 2)
				{
					float t = index - Mathf.FloorToInt(index);

					//Position
					Vector3 pointBefore = (Vector3)positions[Mathf.FloorToInt(index)];
					Vector3 pointAfter = (Vector3)positions[Mathf.FloorToInt(index) + 1];

					Vector3 targetPos = Vector3.Lerp(pointBefore, pointAfter, t);

					//Forward
					Vector3 forwardBefore = (Vector3)forwards[Mathf.FloorToInt(index)];
					Vector3 forwardAfter = (Vector3)forwards[Mathf.FloorToInt(index) + 1];

					Vector3 targetForw = Vector3.Lerp(forwardBefore, forwardAfter, t);

					bodyPart.transform.forward = targetForw;
					bodyPart.transform.position = targetPos;
					bodyPart.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
					bodyPart.transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f));
				}
			}
		}
	}

	//Function to update the positions when the head reach the world limit
	public void updateBodipartPos(Vector3 _modifier)
	{
		for (int i = 0; i < positions.Count; ++i)
		{
			positions[i] = (Vector3)positions[i] - _modifier;
		}
	}

	//public void addBodyPart(int _nb)
	//{
	//	for (int i = 0; i < _nb; ++i)
	//	{
	//		GameObject bodyPart = (GameObject)Instantiate(prefabBodyPart);
	//		bodyPart.name = "bodyPart " + (currentLenght + i);
	//		body.Add(bodyPart);
	//	}

	//	currentLenght += _nb;
	//	maxLenght = Mathf.FloorToInt((currentLenght + 1) * bodyMargin / GetComponent<PlayerController>().boostedSpeed) + 1;
	//}

	//public void removeBodyPart(int _nb)
	//{
	//	for (int i = 0; i < _nb; ++i)
	//	{
	//		GameObject bodyPart = (GameObject)body[body.Count - 1];
	//		body.RemoveAt(body.Count - 1);
	//		Destroy(bodyPart);
	//	}

	//	currentLenght -= _nb;
	//	maxLenght = Mathf.FloorToInt((currentLenght + 1) * bodyMargin / GetComponent<PlayerController>().boostedSpeed) + 1;
	//}

	public void addSpirit(GameObject _spirit)
	{
		//for (int i = 0; i < body.Count - 1; i += spiritSpacing)
		for (int i = 0; i < body.Length - 1; i += spiritSpacing)
		{
			GameObject bodyPart = (GameObject)body[i];
			if (!bodyPart.GetComponentInChildren<Spirit>())
			{
				_spirit.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

				_spirit.transform.position = new Vector3(bodyPart.transform.position.x, bodyPart.transform.position.y + 1.0f, bodyPart.transform.position.z);
				_spirit.transform.parent = bodyPart.transform;
				_spirit.GetComponent<Spirit>().collected = true;
				_spirit.GetComponent<Spirit>().afraid = false;
				_spirit.GetComponent<Spirit>().freezed = false;

				return;
			}
		}
	}
}
