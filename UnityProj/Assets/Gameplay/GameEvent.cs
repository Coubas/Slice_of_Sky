using UnityEngine;
using System.Collections;

public abstract class GameEvent : MonoBehaviour {
	public EventManager manager;

	public abstract void Begin();
	public abstract void End(float _minTimeUntilNext = .0f, float _maxTimeUntilNext = .0f);
}
