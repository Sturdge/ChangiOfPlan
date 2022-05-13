using UnityEngine;
using System.Collections;

public class soundManager : MonoBehaviour {

    [SerializeField]
    AudioClip[] audio = new AudioClip[0];

	AudioSource soundFX;

	AudioSource music;

	void Start()
	{

		soundFX = gameObject.AddComponent<AudioSource>();

		music = gameObject.AddComponent<AudioSource>();


	}

	public void playSFX( int x )
	{

		soundFX.clip = audio [x];

		soundFX.Play ();
		
	}

	public void playMusic( int x )
	{

		music.clip = audio [x];

        music.loop = true;

		music.Play ();
		
	}

}
