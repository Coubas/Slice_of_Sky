using UnityEngine;
using System.Collections;

public class LbEntry : MonoBehaviour 
{
    public TextMesh playerName;
    public TextMesh score;
    public SpriteRenderer avatar;

    public void Setup(string name, int score)
    {
        playerName.text = name;
        this.score.text = score.ToString();
    }

    public void SetupAvatar(Texture2D texture)
    {
        avatar.sprite = Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f,0.5f));
    }
   	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
