using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrollView : MonoBehaviour {

	Vector2 scrollPosition;
	Touch touch;
	public GUISkin ScrollSkin;
	public MockGiveResults Results;
	private int entrycount = 0;
	
	List<Facebook.LeaderboardEntry> mLeaderboard = null;
	
	void OnEnable()
	{
		if (FacebookManager.Instance.LoggedIn)
		{
			FacebookManager.Instance.RequestScores(
				delegate(List<Facebook.LeaderboardEntry> leaderboard)
				{
					foreach (Facebook.LeaderboardEntry entry in leaderboard)
					{
						entry.RankLoc = entrycount;
						entrycount += 1;
						if (entry.Id == FacebookManager.Instance.UserID)
						{
							if (entry.Score < int.Parse (Results.ScoreBest.text))
							{
								entry.Score = int.Parse (Results.ScoreBest.text);
							}
						}
					}
				
					mLeaderboard = leaderboard;
					foreach (Facebook.LeaderboardEntry entry in mLeaderboard)
					{
						Facebook.LeaderboardEntry lbEntry = entry;
						FacebookManager.Instance.RequestPicture(entry.Id, 64, 64, false,
																delegate(Texture2D texture)
																{
							lbEntry.Picture = texture;
						},
						delegate(string reason)
						{
						
						}
						);
					}
				},
				delegate(string reason)
				{
				}
			);
		}
	}

	void OnGUI () 
	{ 
		GUI.skin = ScrollSkin;		// Mock skin. EDITABLE/CHANGABLE TO SUIT
		
		// Scale GUI based on screen size ** Experimental: may not work perfectly on some screens
		int sw = Screen.width;
		int sh = Screen.height;
		
		float w = sw/100.0f;
		float h = sh/100.0f;
		
		
		if (mLeaderboard!=null)
		{
			int rank = 0;

			float heightFactor = h * 8;
			scrollPosition = GUI.BeginScrollView(new Rect(w*18, h*35, w*50, h*30), scrollPosition, new Rect(0, 0, 700, 64*mLeaderboard.Count), GUIStyle.none, GUIStyle.none);
			foreach (Facebook.LeaderboardEntry entry in mLeaderboard)
			{
				rank += 1; // Display rank
				
				// Assign string and texture to GUI
				GUI.Box(new Rect(0, entry.RankLoc * heightFactor, 64, 64), "" + rank.ToString());
				GUI.Box(new Rect(w * 12, entry.RankLoc * heightFactor, 400, 64), "" + entry.Name);
				GUI.Box(new Rect(w * 44, entry.RankLoc * heightFactor, 64, 64), "" + entry.Score.ToString());
				GUI.DrawTexture(new Rect(w * 5, entry.RankLoc * heightFactor, heightFactor, heightFactor), entry.Picture, ScaleMode.ScaleToFit);
			}

			GUI.EndScrollView();
		}

	}
	
	// For touch screens scrolling
	void Update()
	{
		if(Input.touchCount > 0)
		{
			touch = Input.touches[0];
			if (touch.phase == TouchPhase.Moved)
				scrollPosition.y += touch.deltaPosition.y;
		}
	}
}
