using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour {

	public GameObject gameManager;
	public GameObject UICanvas;
	public GameObject audioManager;
	public GameObject battleManager;
	public static EssentialsLoader instance;

	void Awake() {
		
		//Singleton Setup
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		
		if (GameManager.instance == null) {
			Instantiate(gameManager);
		}
		if (UIFade.instance == null) {
			Instantiate(UICanvas);
		}
			
		if (AudioManager.instance == null) {
			Instantiate(audioManager);
		}

		if (BattleManager.instance == null) {
			Instantiate(battleManager);
		}


	}
}
