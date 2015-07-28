using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InviteButtonUp : MonoBehaviour {

	public GameObject Button;

	// Use this for initialization
	void Start () {
		if (!FacebookManager.Instance.LoggedIn) {
			Button.SetActive(false);
		}
	}

    void OnMouseUp()
    {
		FacebookManager.Instance.SendInvitations("Try to chase me down now! Download free!","Select friends to invite."); 
    }
}
