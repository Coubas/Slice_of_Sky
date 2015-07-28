using UnityEngine;
using System.Collections;

public class BodyCollider : MonoBehaviour {
	public PlayerController controller;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Body") && !controller.collisionToAvoid)
		{
			controller.collisionToAvoid = true;
			controller.avoidMade = false;
			controller.bodyPartPos = other.transform.position;
			controller.bodyPartForward = other.transform.forward;
			
		}
	}
}
