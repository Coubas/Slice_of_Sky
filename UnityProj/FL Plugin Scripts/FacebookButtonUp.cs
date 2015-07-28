using UnityEngine;
using System.Collections;

// THIS SCRIPT LOGINS THE USER TO HIS/HER FACEBOOK ACC
// BIND THIS TO A BUTTON IN START MENU, ETC.
public class FacebookButtonUp : MonoBehaviour 
{

	// What happens when Facebook is already logged in from previous session:
	void Start () 
    {
        if (FacebookManager.Instance.LoggedIn)
        {
            gameObject.SetActive(false); // EDITABLE TO SUIT USAGE
        }
	}

	// On pressing the GUI button, initiate Facebook login sequence:
    void OnMouseUp()
    {
        FacebookManager.Instance.Login("publish_actions",
            delegate()
            {
                if (FacebookManager.Instance.LoggedIn)
                {
                    GameObject[] roots = UnityEngine.Object.FindObjectsOfType<GameObject>();
                    foreach (GameObject root in roots)
                    {
                        root.BroadcastMessage("OnFacebookLogin", SendMessageOptions.DontRequireReceiver);
                    }
                }
                
            },
            delegate(string reason)
            {

            });
    }

	// What happens when Facebook is logged in successfully:
    void OnFacebookLogin()
    {
        this.gameObject.SetActive(false); // EDITABLE TO SUIT USAGE
    }
}
