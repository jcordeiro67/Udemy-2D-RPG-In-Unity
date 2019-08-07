using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
	public float waitToLoadTime;
	public Text loadingText;

    // Start is called before the first frame update
    void Start() {
		
    }

    // Update is called once per frame
    void Update() {
		
		if (waitToLoadTime > 0) {
			
			waitToLoadTime -= Time.deltaTime;
			if (waitToLoadTime <= 0) {
				SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));

				GameManager.instance.LoadGameData();
				QuestManager.instance.LoadQuestData();

			}
		}
    }
}
