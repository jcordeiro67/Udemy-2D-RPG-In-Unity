using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	/// <summary>
	/// Instead of making each clip and AudioSource
	/// A different way would be to place and AudioClip
	/// in each Item that plays a sound.
	/// Using public AudioSource[] audioSource 
	/// in the audioManager and placing the two AudioAources 
	/// into the inspector and use AudioSource[0] for bgm 
	/// and AudioSource[1] for sfx in the code for AudioManager.
	/// 
	/// Then to call PlaySFX or PlayBGM instead of using an int 
	/// as the param for musicToPlay use a AudioClip passed in 
	/// from the objects script.
	/// </summary>

	public static AudioManager instance;
	public AudioSource[] sfx;
	[Range (0f, 1f)] public float sfxVolume = 1f;
	public AudioSource[] bgm;
	[Range (0f, 1f)] public float bgmVolume = 0.5f;
    
    void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);

    }

	public void PlaySFX(int sfxToPlay) {
		
		if (sfxToPlay < sfx.Length) {
			sfx[sfxToPlay].volume = sfxVolume;
			sfx[sfxToPlay].Play();
		}
	}

	public void PlayBGM(int musicToPlay) {

		if (!bgm[musicToPlay].isPlaying) {
			StopMusic();
			
			if (musicToPlay < bgm.Length) {
				bgm[musicToPlay].volume = bgmVolume;
				bgm[musicToPlay].Play();
			}
		}
	}

	public void StopMusic() {
		for (int i = 0; i < bgm.Length; i++) {
			if (bgm[i].isPlaying) {
				
				bgm[i].Stop();
			}
		}
	}
}
