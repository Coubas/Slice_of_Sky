using UnityEngine;
using System.Collections;

// **********INITIATE FACEBOOK PLUGIN**************
// Integrate the following code into main menu manager.
// Facebook must be initialised properly when game starts
// ************************************************

public class MenuManager : MonoBehaviour 
{
	// Initialise Facebook, and check if Facebook is logged in from previous session
	void Start () 
    {
        FacebookManager fbMgr = FacebookManager.Instance;

        if (!fbMgr.InitCalled)
        {
            fbMgr.Initialize(
                delegate()
                {
                    if (fbMgr.LoggedIn)
                    {
                        facebookButton.SetActive(false); // EDITABLE TO SUIT USAGE
                    }
                });
        }
	}

    public GameObject facebookButton;
}
