using UnityEngine;
using System.Collections;

public class RingClouds : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().gauges.addGauge();
            Destroy(transform.parent.gameObject, 2.0f);
        }
    }
}
