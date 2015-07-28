using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManagerStartMenu : MonoBehaviour
{
	public Animator startButton;
	public Animator creditsButton;
	public Animator leaderboardButton;
	public Animator settingsButton;
	public Animator exitButton;
	public Animator settingsPanel;
	public Toggle soundToggle;
	public Slider soundSlider;
	public Animator creditsPanel;
	public Animator mainLogo;
	public AnimationClip mainLogoCreditAnim;

	private float savedVolume = 1.0f;
	private bool creditsShowed = false;
	private float timeLeft = .0f;

	void Update()
	{
		if (creditsShowed)
		{
			if (timeLeft == .0f)
			{
				timeLeft = mainLogoCreditAnim.length;
			}

			timeLeft -= Time.deltaTime;

			if (timeLeft <= .0f)
			{
				startButton.SetBool("isHidden", false);
				creditsButton.SetBool("isHidden", false);
				leaderboardButton.SetBool("isHidden", false);
				settingsButton.SetBool("isHidden", false);
				exitButton.SetBool("isHidden", false);

				timeLeft = .0f;
				creditsShowed = false;

				creditsPanel.SetBool("isSliding", false);
				mainLogo.SetBool("isIdling", true);
			}
		}
	}

	public void StartGame()
	{
		Application.LoadLevel(1);
	}

	public void OpenSettings()
	{
		startButton.SetBool("isHidden", true);
		creditsButton.SetBool("isHidden", true);
		leaderboardButton.SetBool("isHidden", true);
		settingsButton.SetBool("isHidden", true);
		exitButton.SetBool("isHidden", true);

		settingsPanel.enabled = true;
		settingsPanel.SetBool("isHidden", false);
	}

	public void CloseSettings()
	{
		startButton.SetBool("isHidden", false);
		creditsButton.SetBool("isHidden", false);
		leaderboardButton.SetBool("isHidden", false);
		settingsButton.SetBool("isHidden", false);
		exitButton.SetBool("isHidden", false);

		settingsPanel.SetBool("isHidden", true);
	}

	public void MuteSound(bool _isSound)
	{
		AudioListener.pause = !_isSound;
		if (_isSound)
		{
			if (savedVolume != .0f)
				soundSlider.value = savedVolume;
		}
		else
		{
			savedVolume = soundSlider.value;
			soundSlider.value = .0f;
		}
	}

	public void SetVolume(float _vol)
	{
		AudioListener.volume = _vol;

		if (_vol == 0 && soundToggle.isOn)
			soundToggle.isOn = false;
		else if (_vol > 0 && !soundToggle.isOn)
			soundToggle.isOn = true;
	}

	public void ShowCredits()
	{
		startButton.SetBool("isHidden", true);
		creditsButton.SetBool("isHidden", true);
		leaderboardButton.SetBool("isHidden", true);
		settingsButton.SetBool("isHidden", true);
		exitButton.SetBool("isHidden", true);

		creditsPanel.enabled = true;
		creditsPanel.SetBool("isSliding", true);
		mainLogo.enabled = true;
		mainLogo.SetBool("isIdling", false);

		creditsShowed = true;
	}

	public void invertAxis(bool _invert)
	{
		PlayerData.PD.invertYAxis = _invert;
	}
}
