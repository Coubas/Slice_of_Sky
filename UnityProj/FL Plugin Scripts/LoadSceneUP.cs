using UnityEngine;
using System.Collections;

// Simply load the mock game scene when pressed, nothing related to Facebook.
public class LoadSceneUP : MonoBehaviour {

	public string Scene;

	void OnMouseUp () {
		Application.LoadLevel (Scene); 
	}
}
