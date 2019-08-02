using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour {

	public static UIFade instance;

	public Image fadeScreen;
	public float fadeSpeed;

	private bool shouldFadeToBlack;
	private bool shouldFadeFromBlack;

	void Awake () {

		if (instance == null) {
			instance = this;
		}else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	void Update () {
		
		if(shouldFadeToBlack){
			GameManager.instance.fadingBetweenAreas = true;
			//Fades the fadeImage alpha channel to full opaque
			fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 
				Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
			//Turn off fading to black when value is reached
			if (fadeScreen.color.a == 1f) {
				shouldFadeToBlack = false;
			}
		}

		if (shouldFadeFromBlack) {
			//Fades the fadeImage alpha channel to full transparent
			fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 
				Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
			//Turn off fade from black when value is reached
			if (fadeScreen.color.a == 0f) {
				shouldFadeFromBlack = false;
			}
			GameManager.instance.fadingBetweenAreas = false;
		}
	}

	public void FadeToBlack(){
		shouldFadeToBlack = true;
		shouldFadeFromBlack = false;
	}

	public void FadeFromBlack(){
		shouldFadeFromBlack = true;
		shouldFadeToBlack = false;
	}
}
