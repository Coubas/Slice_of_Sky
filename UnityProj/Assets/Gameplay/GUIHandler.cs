using UnityEngine;
using System.Collections;

public class GUIHandler : MonoBehaviour
{
	GUIStyle styleSmall;
	GUIStyle styleRegular;
	GUIStyle styleRegularAlignRight;
	GUIStyle styleRegularAlignCenter;
	//Invert y axe checkbox
	GUIContent content = new GUIContent();
	Rect checkBoxRect;
	Rect labelRect;
	public Texture2D checkBoxTexture;
	public string label = "Invert Y Axis";
	//Score
	Rect scoreRect;
	//Timer
	Rect timerRect;
	//Combo
	Rect comboRect;

	void Awake()
	{
		styleSmall = new GUIStyle();
		styleSmall.fontSize = 16;
		styleSmall.normal.textColor = Color.white;
		styleSmall.fontStyle = FontStyle.Normal;

		styleRegular = new GUIStyle();
		styleRegular.fontSize = 32;
		styleRegular.normal.textColor = Color.white;
		styleRegular.fontStyle = FontStyle.Normal;

		styleRegularAlignRight = new GUIStyle();
		styleRegularAlignRight.fontSize = 32;
		styleRegularAlignRight.normal.textColor = Color.white;
		styleRegularAlignRight.fontStyle = FontStyle.Normal;
		styleRegularAlignRight.alignment = TextAnchor.MiddleRight;

		styleRegularAlignCenter = new GUIStyle();
		styleRegularAlignCenter.fontSize = 32;
		styleRegularAlignCenter.normal.textColor = Color.white;
		styleRegularAlignCenter.fontStyle = FontStyle.Normal;
		styleRegularAlignCenter.alignment = TextAnchor.MiddleCenter;

		//Invert y axe checkbox
		checkBoxRect = new Rect(Screen.width - 200, Screen.height - 34, 32, 32);
		labelRect = new Rect(Screen.width - 168, Screen.height - 24, 64, 16);

		//Score
		scoreRect = new Rect(5, 5, 128, 64);

		//Timer
		timerRect = new Rect(Screen.width - 128 - 5, 5, 128, 64);

		//Combo
		comboRect = new Rect(Screen.width * .5f, 5, 64, 64);
	}

	void Update()
	{
		
	}

	void OnGUI()
	{
		if (GameMaster.GM.levelTimer > .0f)
		{
			inGameGUI();
		}
		else
		{
			
		}
	}

	void inGameGUI()
	{
		//Invert y axe checkbox
		GUI.Label(labelRect, label, styleSmall);

		//Score
		GUI.Box(scoreRect, "Score : \n" + GameMaster.GM.score, styleRegular);

		//Timer
		int timeLeft = Mathf.RoundToInt(GameMaster.GM.levelTimer);
		GUI.Box(timerRect, "Time left : \n" + timeLeft / 60 + " : " + timeLeft % 60, styleRegularAlignRight);

		//Combo
		if (GameMaster.GM.combo > 0)
		{
			GUI.Box(comboRect, "x" + (GameMaster.GM.combo + 1), styleRegularAlignCenter);
		}
	}
}
