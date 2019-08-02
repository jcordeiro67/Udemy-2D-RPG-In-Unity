using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


/// <summary>
/// Dialog manager.
/// Shows the Dialog Window
/// Loops through the NPC dialog line when the player cycles through the
/// user input key. 
/// Removes any control characters from the dialogLines.
/// Opens and Closes the areaHint Dialog.
/// </summary>

public class DialogManager : MonoBehaviour {
	
	public static DialogManager instance;

	[Header("Dialog UI Objects")]
	public Text dialogText;
	public Text nameText;
	public Text areaHintText;
	public GameObject dialogBox;
	public GameObject nameBox;
	public GameObject areaHintBox;

	[Header("Objects from Dialog Activator")]
	public string[] dialogLines;
	public int currentLine;

	//Quest related variables
	private string questToMark;
	private bool markQuestComplete;
	private bool shouldMarkQuest;

	private bool justStarted;


	void Start () {

		instance = this;
	}

	void Update () {
		if (dialogBox != null && dialogBox.activeInHierarchy) {
			
			if (Input.GetButtonUp("Fire1")) {
				
				if (!justStarted) {
					currentLine++;
					
					if (currentLine >= dialogLines.Length) {
						dialogBox.SetActive(false);
						GameManager.instance.dialogActive = false;

						if (shouldMarkQuest) {
							shouldMarkQuest = false;
							if (markQuestComplete) {
								QuestManager.instance.MarkQuestComplete(questToMark);
							} else {
								QuestManager.instance.MarkQuestIncomplete(questToMark);
							}
						}

					} else {
						CheckIfName();
						dialogText.text = dialogLines[currentLine];
					}

				} else {
					
					justStarted = false;
				}
			}
			if (Input.GetButtonDown("Cancel")) {
				dialogBox.SetActive(false);
				GameManager.instance.dialogActive = false;
				justStarted = false;
			}
		}
	}

	/// <summary>
	/// Shows the dialog.
	/// </summary>
	/// <param name="newLines">New lines.</param>
	/// <param name="isPerson">If set to <c>true</c> is person.</param>
	public void ShowDialog(string[] newLines, bool isPerson){

		dialogLines = newLines;

		currentLine = 0;
		CheckIfName();

		dialogText.text = dialogLines[currentLine];
		dialogBox.SetActive(true);

		justStarted = true;

		nameBox.gameObject.SetActive(isPerson);
		GameManager.instance.dialogActive = true;
	}

	/// <summary>
	/// Checks the name of the dialogLines for control characters.
	/// </summary>
	public void CheckIfName(){

		if (dialogLines[currentLine].StartsWith("n-")) {
			//nameText.text = dialogLines[currentLine];
			nameText.text = dialogLines[currentLine].Replace("n-", "");
			currentLine++;

		}
	}

	/// <summary>
	/// Shows the area hint.
	/// </summary>
	/// <param name="areaHint">Area hint.</param>
	public void ShowAreaHint(string areaHint){
		if (areaHint != "") {
			areaHintBox.SetActive(true);

			areaHintText.text = areaHint.Replace("---", "\n");
		}
	}

	/// <summary>
	/// Disables the area hint.
	/// </summary>
	public void DisableAreaHint(){
		if (areaHintBox.activeInHierarchy) {
			areaHintBox.SetActive(false);
		}
	}

	public void ShouldActivateQuestAtEnd(string questName, bool markComplete) {
		questToMark = questName;
		markQuestComplete = markComplete;

		shouldMarkQuest = true;
	}
}
