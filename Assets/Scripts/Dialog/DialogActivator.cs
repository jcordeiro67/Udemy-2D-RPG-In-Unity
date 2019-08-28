using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour {
	
	[Header ("Dialog Info")]
	public string[] lines;
	public string areaHintText;
	public bool isPerson = true;
	public bool autoActivateDialog = false;

	[Header ("Quest Settings")]
	public bool shouldActivateQuest;
	public string questToMark;
	public bool markComplete;
	private bool canActivate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (canActivate && Input.GetButtonDown("Fire1")&& !DialogManager.instance.dialogBox.activeInHierarchy && !autoActivateDialog) {
			DialogManager.instance.ShowDialog(lines, isPerson);
			//Activates a quest after dialog closes
			DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			canActivate = true;
			//AutoActivateDialog Added
			if (autoActivateDialog) {
				DialogManager.instance.ShowDialog(lines, isPerson);
			}
			if (!autoActivateDialog) {
				DialogManager.instance.ShowAreaHint(areaHintText);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){

		if (other.tag == "Player") {
			canActivate = false;
			DialogManager.instance.DisableAreaHint();
		}
	}
}
