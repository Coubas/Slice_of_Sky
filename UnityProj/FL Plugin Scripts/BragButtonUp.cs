using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// THIS SCRIPT CALLS UP FB's POSTTOWALL COMMAND
// ALLOW USER TO POST TO THEIR FACEBOOK WALL

// ******************IMPORTANT*****************
// To edit the text in the post, please go to FacebookManager.cs
// Search Posttowall
// Edit all the texts and image links there
// ********************************************

public class BragButtonUp : MonoBehaviour {

    void OnMouseUp()
    {
        FacebookManager.Instance.PostToWall(gameController.score); 
    }

    public MockGameController gameController;
}
