using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour {

	public GameObject gameManager;
	public GameObject UICanvas;
	public GameObject audioManager;

	void Start() {
		if (UIFade.instance == null) {
			Instantiate(UICanvas);
		}

		if (GameManager.instance == null) {
			Instantiate(gameManager);
		}

		if (AudioManager.instance == null) {
			Instantiate(audioManager);
		}

		DontDestroyOnLoad(gameObject);
	}
}
