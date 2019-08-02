using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour {
	
	[SerializeField] private string areaToLoad;
	[SerializeField] public string areaTransitionName;
	[SerializeField] private bool shouldLoadAfterFade;
	[SerializeField] public bool resetPlayerOnEntrance;

	public float waitToLoad = 1;

	void Update () {
		if (shouldLoadAfterFade) {
			waitToLoad -= Time.deltaTime;
			if (waitToLoad <= 0) {
				shouldLoadAfterFade = false;
				SceneManager.LoadScene(areaToLoad);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other){

		if (other.tag == "Player" && areaToLoad != "") {
			//SceneManager.LoadScene(areaToLoad);
			shouldLoadAfterFade = true;
			UIFade.instance.FadeToBlack();
			PlayerController.instance.areaTransitionName = areaTransitionName;
		}
	}
}
