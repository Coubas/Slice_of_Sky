using UnityEngine;
using System.Collections;

public class AutoDestroyParticleSystem : MonoBehaviour {
    public float timeBeforeDestroy;
    private float timer;

	// Use this for initialization
	void Start () {
        timer = .0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer >= timeBeforeDestroy)
        {
            Destroy(gameObject);
        }
    }
}
