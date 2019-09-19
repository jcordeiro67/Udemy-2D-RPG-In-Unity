using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleStarter : MonoBehaviour {

	public BattleType[] potentialBattles;
	public bool activateOnEnter, activateOnStay, activateOnExit;
	public float timeBetweenBattles = 10f;
	private float betweenBattleCounter;
	public bool deactivateAfterStarting;
	private bool inArea;


    // Start is called before the first frame update
    void Start() {
		betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
    }

    // Update is called once per frame
    void Update() {
		if (inArea && PlayerController.instance.canMove) {
			if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") !=0) {
				betweenBattleCounter -= Time.deltaTime;
			}

			if (betweenBattleCounter <= 0) {
				betweenBattleCounter = Random.Range(timeBetweenBattles * 0.5f, timeBetweenBattles * 1.5f);
				StartCoroutine(StartBattleCo());
			}
		}
    }

	private void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Player") {
			
			if (activateOnEnter) {
				//Start Battle
				StartCoroutine(StartBattleCo());

			} else {
				inArea = true;
			}
		}


	}

	private void OnTriggerExit2D(Collider2D other) {

		if (other.tag == "Player") {
			
			if (activateOnExit) {
				//Start Battle
				StartCoroutine(StartBattleCo());

			} else {
				inArea = false;
			}
		}

	}

	public IEnumerator StartBattleCo() {
		//Fade UI Screne
		UIFade.instance.FadeToBlack();

		//Set battle active to true in GameManager
		GameManager.instance.battleActive = true;

		//Select a random battle from potential battles
		int selectedBattle = Random.Range(0, potentialBattles.Length);

		//Set rewards and XP in BattleManager
		BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
		BattleManager.instance.rewardXP = potentialBattles[selectedBattle].rewardXP;

		//Pause
		yield return new WaitForSeconds(1.5f);

		//Start New Battle
		BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies);

		//Fade UI IN
		UIFade.instance.FadeFromBlack();

		//Deactivate if only used once
		if (deactivateAfterStarting == true) {
			gameObject.SetActive(false);
		}
	}

}
