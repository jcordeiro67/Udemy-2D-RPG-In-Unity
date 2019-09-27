using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
	public static QuestManager instance;

	public string[] questMarkerNames;
	public bool[] questMarkersComplete;

	private string questKeyString = "QuestMarker_";

    // Start is called before the first frame update
    void Start() {
		instance = this;
		questMarkersComplete = new bool[questMarkerNames.Length];

    }

	void Update() {
		
	}

	/// <summary>
	/// Gets the quest number.
	/// </summary>
	/// <returns>The quest number.</returns>
	/// <param name="questToFind">Quest to find.</param>
	public int GetQuestNumber(string questToFind) {
		for (int i = 0; i < questMarkerNames.Length; i++) {
			if (questMarkerNames[i] == questToFind) {
				return i;
			}
		}

		Debug.LogError("Quest " + questToFind + " does not exist");
		return 0;
	}

	/// <summary>
	/// Checks if quest complete.
	/// </summary>
	/// <returns><c>true</c>, if if quest complete was checked, <c>false</c> otherwise.</returns>
	/// <param name="questToCheck">Quest to check.</param>
	public bool CheckIfQuestComplete(string questToCheck){

		if (GetQuestNumber(questToCheck) != 0) {
			return questMarkersComplete[GetQuestNumber(questToCheck)];
		}

		return false;
	}

	/// <summary>
	/// Marks the quest complete.
	/// </summary>
	/// <param name="questToMarkComplete">Quest to mark complete.</param>
	public void MarkQuestComplete(string questToMarkComplete){
		questMarkersComplete[GetQuestNumber(questToMarkComplete)] = true;
		UpdateLocalQuestObjects();
	}

	/// <summary>
	/// Marks the quest incomplete.
	/// </summary>
	/// <param name="questToMarkIncomplete">Quest to mark incomplete.</param>
	public void MarkQuestIncomplete(string questToMarkIncomplete){
		questMarkersComplete[GetQuestNumber(questToMarkIncomplete)] = false;
		UpdateLocalQuestObjects();
	}

	/// <summary>
	/// Updates the local quest objects.
	/// </summary>
	public void UpdateLocalQuestObjects(){

		QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

		if (questObjects.Length > 0) {
			
			for (int i = 0; i < questObjects.Length; i++) {
				questObjects[i].CheckCompletion();
			}
		}
	}

	/// <summary>
	/// Saves the quest data to PlayerPrefs.
	/// </summary>
	public void SaveQuestData() {
		
		for (int i = 0; i < questMarkerNames.Length; i++) {
			if (questMarkersComplete[i]) {
				PlayerPrefs.SetInt(questKeyString + questMarkerNames[i], 1);
			}else {
				PlayerPrefs.SetInt(questKeyString + questMarkerNames[i], 0);
			}
		}
		Debug.Log("Quest Data Saved to PlayerPrefs");
	}

	/// <summary>
	/// Loads the quest data from PlayerPrefs.
	/// </summary>
	public void LoadQuestData() {
		for (int i = 0; i < questMarkerNames.Length; i++) {
			int valueToSet = 0;
			if (PlayerPrefs.HasKey(questKeyString + questMarkerNames[i])) {
				valueToSet = PlayerPrefs.GetInt(questKeyString + questMarkerNames[i]);
			}
			if (valueToSet == 0) {
				questMarkersComplete[i] = false;
			} else {
				questMarkersComplete[i] = true;
			}
		}
		Debug.Log("Quest Data Loaded from PlayerPrefs");
	}
}
