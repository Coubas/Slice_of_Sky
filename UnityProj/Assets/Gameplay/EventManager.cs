﻿using UnityEngine;
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

        if(PlayerData.PD.gaugesLvl.Length <= 0 || (PlayerData.PD.gaugesLvl.Length > 0 && PlayerData.PD.gaugesLvl[0] > 0 && PlayerData.PD.gaugesLvl[1] > 0))
        {
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
	}

    public void eventEnded(float _min = -1.0f, float _max = -1.0f)
	{
        if(minTimeBetweenEvents == -1.0f || _max == -1.0f)
        {
		    timeUntilNextEvent = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
        }
        else
        {
            timeUntilNextEvent = Random.Range(_min, _max);
        }
		eventCurrentlyHappening = false;
	}
}
