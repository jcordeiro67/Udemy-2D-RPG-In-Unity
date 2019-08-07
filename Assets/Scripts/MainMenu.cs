using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public string startScene;
	public int menuBgm;
	public string loadingScene;

	public GameObject continueButton;

    // Start is called before the first frame update
    void Start() {
		AudioManager.instance.PlayBGM(menuBgm);

		if (PlayerPrefs.HasKey("Current_Scene")) {
			continueButton.SetActive(true);
		} else {
			continueButton.SetActive(false);
		}
    }

	public void Continue() {
		SceneManager.LoadScene(loadingScene);
	}

	public void NewGame() {
		
		SceneManager.LoadScene(startScene);
	}

	public void ExitGame() {
		Application.Quit();
	}
}
