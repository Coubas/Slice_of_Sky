using UnityEngine;
using System.Collections;

public class CreditsText : MonoBehaviour {
	public Vector3 endPos;
	public float speed;
	public bool isScrolling;

	private Vector3 startPos;
	private float time;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		time = .0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (isScrolling)
		{
			time += Time.deltaTime;
			transform.position = Vector3.Lerp(startPos, endPos, time * speed);
		}
		else if (transform.position != startPos)
		{
			time = .0f;
			transform.position = startPos;
		}
		
	}
}
