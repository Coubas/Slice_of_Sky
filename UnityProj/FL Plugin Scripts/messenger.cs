using UnityEngine;
using System.Collections;

// This script tells the mock game controller that the game has ended. That is all.
// Nothing related to Facebook.

public class messenger : MonoBehaviour
{
		public enum Action
		{
				SendMessage,
				Disable
		}
		public Action myType = Action.SendMessage;
		public GameObject target;
		public string Message;


		void OnMouseUp ()
		{
				if (target) {
						target.SendMessage (Message);
						if (myType == Action.Disable) {
								gameObject.SetActive (false);
						}
				} else
						Application.OpenURL (Message);

        
		}
     
}
