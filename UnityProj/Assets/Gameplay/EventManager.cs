using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
	public float timeUntilNextEvent;
	public float minTimeBetweenEvents;
	public float maxTimeBetweenEvents;
	public GameEvent[] events;

	private bool eventCurrentlyHappening;

	// Use this for initialization
	void Start () {
		eventCurrentlyHappening = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMaster.GM.gamePaused)
			return;

		if (timeUntilNextEvent > .0f)
		{
			timeUntilNextEvent -= Time.deltaTime;
		}
		else if (!eventCurrentlyHappening)
		{
			int id = Random.Range(0, events.Length);
			events[id].Begin();
			eventCurrentlyHappening = true;
		}
	}

	public void eventEnded()
	{
		timeUntilNextEvent = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
		eventCurrentlyHappening = false;
	}
}
