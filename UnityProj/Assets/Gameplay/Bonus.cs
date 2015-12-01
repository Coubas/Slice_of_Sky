using UnityEngine;
using System.Collections;

public abstract class Bonus : MonoBehaviour {
    
	void Start () {

	}
	
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			applyBonus(other.gameObject);
			Destroy(gameObject);
		}
	}

	protected abstract void applyBonus(GameObject _player);
}
