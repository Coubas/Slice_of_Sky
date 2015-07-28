using UnityEngine;
using System.Collections;

public class WindStream : MonoBehaviour {
	public float rotSpeed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GM.gamePaused)
			return;

		transform.Rotate(.0f, rotSpeed, .0f);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerController>().isInStream = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerController>().isInStream = false;
		}
	}
}
