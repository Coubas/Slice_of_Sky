using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script is the master 'gameplay' controller for the mock example
// Copy and integrate the following codes into your game

public class MockGameController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ScoreCount;
    public GUIText KillButton;			// Mock button to end game
    public MockGiveResults Scores;
    public int score = 0;				// Initial score
	public int FBUserscore;				// ** Facebook: for offline score sync (when user logged in later)
    public TextMesh ScoreWhite;

    public SpriteRenderer nextPlayer;
    public TextMesh nextPlayerText;

    List<Facebook.LeaderboardEntry> mLeaderboard = null;	// ** Facebook
    Facebook.LeaderboardEntry currentNext = null;			// ** Facebook

    void Start()
    {
        MockSaveController.CreateAndLoadData();

		// ** To check offline score from previous offline sessions
		// To be used to sync score with Facebook
		if (FacebookManager.Instance.LoggedIn)
		{
			FacebookManager.Instance.RequestScores(
				delegate(List<Facebook.LeaderboardEntry> leaderboard)
				{
				foreach (Facebook.LeaderboardEntry entry in leaderboard)
				{
					if (entry.Id == FacebookManager.Instance.UserID)
					{
						FBUserscore = entry.Score;
					}
				}
			},
			delegate(string reason)
			{
			}
			);
		}

		score = PlayerData.PD.getLastScore().score;
		Die();
    }

	//void OnMouseDown()
	//{
	//	// Mock 'gameplay': add score when screen is tapped. For illustration purposes only
	//	if (this.GetComponent<BoxCollider2D>().enabled == true)
	//		score++;
	//		ScoreWhite.text = "" + score;

	//	ClosestOpponent();		// ** Facebook
	//}

	public void SetScore(int _score)
	{
		score = _score;
	}

    public void LoadGame()
    {
		//MainMenu.SetActive(false);
		//this.GetComponent<BoxCollider2D>().enabled = true;
		//ScoreCount.SetActive(true);

		// ** Facebook
		//if (FacebookManager.Instance.LoggedIn)
		//{
		//	FacebookManager.Instance.RequestScores(
		//	delegate(List<Facebook.LeaderboardEntry> leaderboard)
		//	{
		//		mLeaderboard = leaderboard;
		//		ClosestOpponent();
		//	},
		//	delegate(string reason)
		//	{

		//	});
		//}
		//else
		//{
		//	HideNextPlayer();
		//}
        
    }

	// What happens when game ends:
    public void Die()
    {
		Scores.gameObject.SetActive(true);		// Call leaderboard
		Scores.OnActivate();					// Call leaderboard

		// Update scores online at Facebook
		if (score > MockSaveController.GetBestScore())
		{
			FacebookManager.Instance.PostScore(score,
				delegate()
				{
					Debug.Log("Post Succes");
				},
				delegate(string reason)
				{
					Debug.Log("Post failed : " + reason);
				});
		}

		Scores.Fill();		// Call leaderboard
		KillButton.enabled = false;
    }

	// ** Facebook: this is a neat function that displays the next highest ranked friend
	// Copy this part into your code if you want to use this feature
    void ClosestOpponent()
    {
        if (!FacebookManager.Instance.LoggedIn || mLeaderboard==null)
        {
            HideNextPlayer();
            return;
        }
        ShowNextPlayer();
        Facebook.LeaderboardEntry foundEntry = null;
        foreach (Facebook.LeaderboardEntry lbEntry in mLeaderboard)
        {
            if (lbEntry.Score > score)
            {
                if (foundEntry != null)
                {
                    if (lbEntry.Score < foundEntry.Score)
                    {
                        foundEntry = lbEntry;
                    }
                }
                else
                {
                    foundEntry = lbEntry;
                }
            }
        }
        if(foundEntry==null)
        {
            HideNextPlayer();
            return;
        }
        if(foundEntry!=currentNext)
        {

            nextPlayerText.text = foundEntry.Score.ToString();
            FacebookManager.Instance.RequestPicture(foundEntry.Id, 64, 64, false,
                delegate(Texture2D texture)
                {
                    nextPlayer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                },
                delegate(string reason)
                {
                });
            currentNext = foundEntry;
        }
        
    }

	// Continue from above:
	// ** Facebook: this is a neat function that displays the next highest ranked friend
    void HideNextPlayer()
    {
        nextPlayer.gameObject.SetActive(false);
        nextPlayerText.gameObject.SetActive(false);
    }

	// Continue from above:
	// ** Facebook: this is a neat function that displays the next highest ranked friend
    void ShowNextPlayer()
    {
        nextPlayer.gameObject.SetActive(true);
        nextPlayerText.gameObject.SetActive(true);
    }
}

