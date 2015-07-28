using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using SimpleJSON;

namespace Facebook
{
    public delegate void Callback();
    public delegate void Callback<T>(T value);

    public class Profile
    {
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        public Texture2D Picture
        {
            get
            {
                return picture;
            }
            set
            {
                picture = value;
            }
        }

        protected string id = "";
        protected string firstName = "";
        protected string lastName = "";
        protected Texture2D picture = null;
    }

    public class LeaderboardEntry
    {
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }

        public Texture2D Picture
        {
            get
            {
                return picture;
            }
            set
            {
                picture = value;
            }
        }

		public int RankLoc;

        protected string id = "";
        protected string name = "";
        protected int score = 0;
        protected Texture2D picture = null;
    }
}


public class FacebookManager
{
    private static FacebookManager instance = null;

    public static FacebookManager Instance
    {
        get
        {
			if (instance == null)
			{
				instance = new FacebookManager();
			}
                
            return instance;
        }
    }

    public bool InitCalled
    {
        get
        {
            return mInitCalled;
        }
    }

    public bool LoggedIn
    {
        get
        {
            return FB.IsLoggedIn;
        }
    }

    public string UserID
    {
        get
        {
            return FB.UserId;
        }
    }

    public Profile Profile
    {
        get
        {
            return mProfile;
        }
    }
    
    public List<Profile> Friends
    {
        get
        {
            return mFriends;
        }
    }

    public List<LeaderboardEntry> Leaderboard
    {
        get
        {
            return mLeaderboard;
        }
    }

    public void Initialize(Callback successCallback)
    {
        mInitCalled = true;
        FB.Init(
            delegate()
            {
                successCallback();
            });
    }

    public void Login(string permissionString,Callback successCallback, Callback<string> errorCallback)
    {
        FB.Login(permissionString,
            delegate(FBResult result)
            {
                if (result.Error != null)
                {
                    errorCallback(result.Error);
                }
                else
                {
                    successCallback();
                }
            });
    }

    public void Logout()
    {
        FB.Logout();
    }

    public void GetPermissions(Callback<List<string>> successCallback, Callback<string> errorCallback)
    {
        FB.API("/me/permissions", Facebook.HttpMethod.GET,
            delegate(FBResult result)
            {
                if (result.Error != null)
                {
                    errorCallback(result.Error);
                }
                else
                {
                    List<string> permissions = DeserializePermissions(result.Text);
                    successCallback(permissions);
                }
            });
    }

    public void RequestPlayerInfo(Callback<Profile> successCallback, Callback<string> errorCallback)
    {
        FB.API("/me?fields=id,first_name,last_name", Facebook.HttpMethod.GET,
            delegate(FBResult result)
            {
                if (result.Error != null)
                {
                    errorCallback(result.Error);
                }
                else
                {
                    Facebook.Profile profile = DeserializeProfile(result.Text);
                    successCallback(profile);
                }
            });
    }

    public void RequestPicture(string facebookId,int width,int height, bool ignoreCache, Callback<Texture2D> successCallback, Callback<string> errorCallback)
    {
        if (facebookId == "me")
            facebookId = FB.UserId;
        if (!ignoreCache && mPictureCache.ContainsKey(facebookId))
        {
            successCallback(mPictureCache[facebookId]);
            return;
        }
        string url = string.Format("/{0}/picture?width={1}&height={2}",facebookId,width,height);
        FB.API(url, Facebook.HttpMethod.GET, 
            delegate (FBResult result)
            {
                if (result.Error != null)
                {
                    errorCallback(result.Error);
                }
                else
                {
                    mPictureCache[facebookId] = result.Texture;
                    successCallback(result.Texture);
                }
            });
    }

    public void RequestFriends(Callback<List<Profile>> successCallback, Callback<string> errorCallback)
    {
        FB.API("/me?fields=friends.limit(100).fields(id,first_name,last_name)", Facebook.HttpMethod.GET,
            delegate(FBResult result)
            {
                if (result.Error != null)
                {
                    errorCallback(result.Error);
                }
                else
                {
                    List<Facebook.Profile> friends = DeserializeFriendProfiles(result.Text);
                    successCallback(friends);
                }
            });
    }

    public void PostScore(int score, Callback successCallback, Callback<string> errorCallback)
    {
        WWWForm payload = new WWWForm();
        payload.AddField("score", score);
        FB.API("/"+FB.UserId +"/scores", HttpMethod.POST,
            delegate(FBResult result)
            {
                if (result.Error != null)
                {
                    errorCallback(result.Error);
                }
                else
                {
                    successCallback();
                }
            },
            payload
            );
    }

    public void RequestScores(Callback<List<LeaderboardEntry>> successCallback, Callback<string> errorCallback)
    {
        FB.API("/app/scores?fields=score,user.limit(20)", Facebook.HttpMethod.GET,
            delegate(FBResult result)
            {
                if (result.Error != null)
                {
                    errorCallback(result.Error);
                }
                else
                {
                    Debug.Log(result.Text);
                    List<LeaderboardEntry> leaderboard = DeserializeScores(result.Text);
                    successCallback(leaderboard);
                }
            });
    }

    public void SendInvitation(string message, string to, int score, string title)
    {
        string[] recipient = { to };
        FB.AppRequest(message, recipient, new List<object>(), null, null, "{\"challenge_score\":" + score + "}", title,
            delegate(FBResult result)
            {
            });
    }

    public void SendInvitations(string message, string title)
    {
		FB.AppRequest(message, null, new List<object>(), null, null, "", title,
            delegate(FBResult result)
            {
            });
    }

	// EDIT POST TO WALL INFO HERE
    public void PostToWall(int score)
    {
        FB.Feed(
                linkCaption: "I just scored " + score.ToString() + " Chase Him Down",
                picture: "http://www.friendsmash.com/images/logo_large.jpg",
                linkName: "Check out my score!",
                link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
                );
    }

    Facebook.Profile DeserializeProfile(string json)
    {
        Facebook.Profile profile = new Facebook.Profile();
        JSONNode root = JSON.Parse(json);
        profile.Id = root["id"];
        profile.FirstName = root["first_name"];
        profile.LastName = root["last_name"];
        return profile;
    }

    List<Facebook.Profile> DeserializeFriendProfiles(string json)
    {
        List<Facebook.Profile> friends = new List<Facebook.Profile>();
        JSONNode root = JSON.Parse(json);
        JSONNode data = root["friends"]["data"];
        for (int i = 0; i < data.Count; i++)
        {
            Facebook.Profile profile = new Facebook.Profile();
            profile.Id = data[i]["id"];
            profile.FirstName = data[i]["first_name"];
            profile.LastName = data[i]["last_name"];
            friends.Add(profile);
        }
        return friends;
    }

    List<LeaderboardEntry> DeserializeScores(string json)
    {
        List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();
        JSONNode root = JSON.Parse(json);
        JSONNode data = root["data"];
        for (int i = 0; i < data.Count; i++)
        {
            LeaderboardEntry entry = new LeaderboardEntry();
            entry.Id = data[i]["user"]["id"];
            entry.Name = data[i]["user"]["name"];
            entry.Score = data[i]["score"].AsInt;
            leaderboard.Add(entry);
            if (entry.Id == FB.UserId)
                entry.Id = "me";
        }
        return leaderboard;
    }

    List<string> DeserializePermissions(string json)
    {
        List<string> permissions = new List<string>();
        JSONNode root = JSON.Parse(json);
        JSONNode data = root["data"];
        for (int i = 0; i < data.Count; i++)
        {
            JSONClass dataEntry = data[i].AsObject;
            for (int j = 0; j < dataEntry.Count; j++)
            {
                string key = dataEntry.KeyAtIndex(j);
                permissions.Add(key);
            }
        }
        return permissions;
    }

    bool mInitCalled = false;
    Profile mProfile = null;
    List<Profile> mFriends = null;
    List<LeaderboardEntry> mLeaderboard = null;
    Dictionary<string, Texture2D> mPictureCache = new Dictionary<string, Texture2D>();
}
