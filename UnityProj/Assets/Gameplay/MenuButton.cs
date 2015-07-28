using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour
{
	public ParticleSystem particules;
	public AudioClip musicClip;
	public AudioSource sound;
	public CreditsText creditText;

	AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
	{
		AudioSource newAudio = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}

	// Use this for initialization
	void Start()
	{
		sound = AddAudio(musicClip, false, false, 0.3f);

	}

	// Update is called once per frame
	void Update()
	{
	}

	void OnMouseDown()
	{
		sound.Play();
		switch (name)
		{
			case "Play":
				Application.LoadLevel(1);
				break;
			case "Settings":
				Camera.main.transform.Rotate(0.0f, -90.0f, 0.0f, Space.World);
				break;
			case "Credits":
				Camera.main.transform.Rotate(0.0f, 90.0f, 0.0f, Space.World);
				creditText.isScrolling = true;
				break;
			case "BackCredits":
				Camera.main.transform.Rotate(0.0f, -90.0f, 0.0f, Space.World);
				creditText.isScrolling = false;
				break;
			case "BackSettings":
				Camera.main.transform.Rotate(0.0f, 90.0f, 0.0f, Space.World);
				break;
			case "Replay" :
				Application.LoadLevel(PlayerData.PD.getLastScore().level);
				break;
		}
	}
}
