using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour {

	public GameObject gameManager;
	public GameObject UICanvas;

	void Start() {
		if (UIFade.instance == null) {
			Instantiate(UICanvas);
		}

		if (GameManager.instance == null) {
			Instantiate(gameManager);
		}

		DontDestroyOnLoad(gameObject);
	}
}
