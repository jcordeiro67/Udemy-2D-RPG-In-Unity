using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public string mainMenuScene;
	public string loadGameScene;
	public int musicToPlay;

    void Start() {
		//Play Background music
		AudioManager.instance.PlayBGM(musicToPlay);
		//Disable Player
		PlayerController.instance.gameObject.SetActive(false);
		//Disable the game menu
		GameMenu.instance.gameObject.SetActive(false);
		//Disable battleManager
		BattleManager.instance.gameObject.SetActive(false);
    }

	public void QuitToMain() {

		Destroy(GameManager.instance.gameObject);
		Destroy(PlayerController.instance.gameObject);
		Destroy(GameMenu.instance.gameObject);
		Destroy(AudioManager.instance.gameObject);
		Destroy(BattleManager.instance.gameObject);

		SceneManager.LoadScene(mainMenuScene);
	}

	public void LoadLastSave() {


		Destroy(BattleManager.instance.gameObject);
		Destroy(GameMenu.instance.gameObject);
		Destroy(GameManager.instance.gameObject);
		Destroy(PlayerController.instance.gameObject);

		SceneManager.LoadScene(loadGameScene);
	}
}
