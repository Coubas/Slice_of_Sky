using UnityEngine;
using System.Collections;

public abstract class GameEvent : MonoBehaviour {
	public EventManager manager;

	public abstract void Begin();
	public abstract void End();
}
