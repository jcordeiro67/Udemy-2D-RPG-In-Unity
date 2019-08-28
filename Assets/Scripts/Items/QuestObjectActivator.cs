using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.VersionControl;

public class QuestObjectActivator : MonoBehaviour
{
	public GameObject objectToActivate;
	public string questToCheck;
	public bool activeIfComplete;

	private bool initialCheckDone;

    
	void Start () {
		
	}

    // Update is called once per frame
    void LateUpdate() {
		
		if (!initialCheckDone) {
			initialCheckDone = true;
			CheckCompletion();
		}
    }

	public void CheckCompletion() {
		if (QuestManager.instance.CheckIfQuestComplete(questToCheck)) {
			objectToActivate.SetActive(activeIfComplete);
		}
	}
}
