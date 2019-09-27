using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {

	public static BattleManager instance;

	public GameObject battleScene;
	public int battleMusic;
	public BattleChar[] playerPrefabs;
	public BattleChar[] enemyPrefabs;

	public Transform[] playerPositions;
	public Transform[] enemyPositions;

	public List<BattleChar> activeBattlers = new List<BattleChar>();
	public int currentTrun;
	public bool turnWaiting;
	public float enemyAttackWait = 1f;
	public GameObject enemyAttackEffect;
	public GameObject uiButtonsHolder;
	public BattleMove[] movesList;
	public DamageDisplay damageUI;
	public GameObject targetMenuUI;
	public BattleTargetButton[] targetButtons;
	public GameObject magicMenuUI;
	public MagicButton[] magicButtons;

	[Header("BattleItems Info")]
	public GameObject battleItemsMenu;
	public ItemButton[] itemButtons;
	public string selectedItem;
	public Item activeItem;
	public Text itemName;
	public Text itemDesc; 
	public Text useButtonText;

	public GameObject itemCharChoicePanel;
	public Text[] itemCharChoiceNames;
	public BattleNotification battleNotice;
	public int chanceToFlee = 35;
	public Text[] playerNames, playerHP, playerMP;
	private bool battleActive;
	private bool fleeing;
	public string gameOverScene;
	public int rewardXP;
	public string[] rewardItems;

    void Start() {
		
		//Singleton Setup
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
    }

    void Update() {
		
		if (battleActive) {
			if (turnWaiting) {
				if (activeBattlers[currentTrun].isPlayer) {
					uiButtonsHolder.SetActive(true);
				} else {
					uiButtonsHolder.SetActive(false);

					//enemy should attack
					StartCoroutine(EnemyMoveCo());
				}
			}

		}
    }

	public void BattleStart(string[] enemiesToSpawn) {

		if (!battleActive) {
			battleActive = true;
			GameManager.instance.battleActive = true;

			if (battleScene != null) {
				// Set battleScene position to align with camera
				battleScene.transform.position = new Vector3(Camera.main.transform.position.x, 
					Camera.main.transform.position.y, battleScene.transform.position.z);
				
				// Set battleScene to active
				battleScene.SetActive(true);
			}

			// Play Battle BGM
			AudioManager.instance.PlayBGM(battleMusic);

			// Position players at player Positions
			for (int i = 0; i < playerPositions.Length; i++) {

				// Check if player is active.
				if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy) {

					for (int j = 0; j < playerPrefabs.Length; j++) {
						//if active player is in list of battleChars
						if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName) {
							//Instantiate playerPrefab and set its position
							BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
							//parent prefab to position
							newPlayer.transform.parent = playerPositions[i];
							//add newPlayer to the activeBattlers list
							activeBattlers.Add(newPlayer);

							//Set stats for battle players
							//Create a new CharStats reference for thePlayer.
							CharStats thePlayer = GameManager.instance.playerStats[i];
							//set stats
							activeBattlers[i].currentHP = thePlayer.currentHP;
							activeBattlers[i].maxHP = thePlayer.maxHP;
							activeBattlers[i].currentMP = thePlayer.currentMP;
							activeBattlers[i].maxMP = thePlayer.maxMP;
							activeBattlers[i].strength = thePlayer.strength;
							activeBattlers[i].defence = thePlayer.defence;
							activeBattlers[i].wpnPower = thePlayer.weaponPower;
							activeBattlers[i].armrPower = thePlayer.armorPower;
						}
					}
				}
			}
			// Position Enemies at enemy positions
			for (int i = 0; i < enemiesToSpawn.Length; i++) {
				//Check for blank space in list
				if (enemiesToSpawn[i] != "") {
					for (int j = 0; j < enemyPrefabs.Length; j++) {
						if (enemyPrefabs[j].charName == enemiesToSpawn[i]) {
							BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
							newEnemy.transform.parent = enemyPositions[i];
							activeBattlers.Add(newEnemy);

						}
					}
				}
			}
		}

		turnWaiting = true;
		//To force player first in battle set currentTurn to 0.
		//currentTurn = 0;
		currentTrun = Random.Range(0, activeBattlers.Count);
		UpdateUIStats();
		battleActive = true;
	}

	public void NextTurn() {

		currentTrun++;
		// Check if current turn exceeds the activeBattlers list reset to 0
		if (currentTrun >= activeBattlers.Count) {
			currentTrun = 0;
		}

		turnWaiting = true;
		UpdateBattle();
		UpdateUIStats();
	}

	public void UpdateBattle() {

		bool allEnemiesDead = true;
		bool allPlayersDead = true;

		for (int i = 0; i < activeBattlers.Count; i++) {
			if (activeBattlers[i].currentHP <= 0) {
				activeBattlers[i].currentHP = 0;

			}
			if (activeBattlers[i].currentHP == 0) {
				//handle dead player
				if (activeBattlers[i].isPlayer) {
					activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].deadSprite;
					activeBattlers[i].hasDied = true;
				}
				//handle dead enemy
				if (!activeBattlers[i].isPlayer) {
					activeBattlers[i].EnemyFade();
					activeBattlers[i].hasDied = true;
				}

			} else {
				
				if(activeBattlers[i].isPlayer){
					allPlayersDead = false;
					activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].aliveSprite;
				} else {
					allEnemiesDead = false;
				}
			}
		}

		//End of battle
		if (allEnemiesDead || allPlayersDead) {
			if (allEnemiesDead) {
				//end battle in victory
				StartCoroutine(EndBattleCo());
			} else {
				//end battle in failure
				StartCoroutine(GameOverCo());
			}

			//battleScene.SetActive(false);
			//GameManager.instance.battleActive = false;
			//battleActive = false;
			//StartCoroutine(EndBattleCo());

			//TESTING END BATTLE PAUSE
			//StartCoroutine(PauseUntilEnd(2f));
			//END TESTING

		} else {
			
			while (activeBattlers[currentTrun].currentHP == 0) {
				currentTrun++;

				if (currentTrun >= activeBattlers.Count) {
					currentTrun = 0;
				}
			}
		}
	}

	public IEnumerator EnemyMoveCo() {

		turnWaiting = false;
		yield return new WaitForSeconds(enemyAttackWait);
		EnemyAttack();
		yield return new WaitForSeconds(1f);
		NextTurn();
	}

	//TESTING END OF BATTLE
	public IEnumerator PauseUntilEnd(float waitTime) {

		yield return new WaitForSeconds(waitTime);
		battleScene.SetActive(false);
	}
	//END TESTING
	public void EnemyAttack() {

		//chose player to attack from list of players
		List<int> players = new List<int>();
		for (int i = 0; i < activeBattlers.Count; i++) {
			if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0) {
				players.Add(i);
			}
		}

		//Display Enemy Attack Effect
		if (enemyAttackEffect != null) {
			//Instantiate Enemy Attack Effect
			Instantiate(enemyAttackEffect, activeBattlers[currentTrun].transform.position, activeBattlers[currentTrun].transform.rotation);
		}

		//Select random player to attack
		int selectedTarget = players[Random.Range(0, players.Count)];
		//Select which move of availaible move list to use for attack
		int selectAttack = Random.Range(0, activeBattlers[currentTrun].movesAvailiable.Length);

		int movePower = 0;
		//find correct attact from movesAvailiable list
		for (int i = 0; i < movesList.Length; i++) {
			if (movesList[i].moveName == activeBattlers[currentTrun].movesAvailiable[selectAttack]) {
				Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
				movePower = movesList[i].movePower;
			}
		}

		//Apply damage to selected player
		DealDamage(selectedTarget, movePower);

	}

	public void DealDamage(int target, int movePower) {
		//Attackers total attack power
		float attackPower = activeBattlers[currentTrun].strength + activeBattlers[currentTrun].wpnPower;
		//Defenders total defensive power
		float defencePower = activeBattlers[target].defence + activeBattlers[target].armrPower;
		//Calculate a damage variable with a slight randomness
		float damageCalc = (attackPower / defencePower) * movePower * Random.Range(.9f, 1.1f);
		int damageToGive = Mathf.RoundToInt(damageCalc);

		//Debug.Log(activeBattlers[currentTrun].charName + " is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].charName);
		//Deduct damageToGive from target
		activeBattlers[target].currentHP -= damageToGive;

		Instantiate(damageUI, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamageText(damageToGive);
		UpdateUIStats();
	}

	public void UpdateUIStats() {

		for (int i = 0; i < playerNames.Length; i++) {
			if (activeBattlers.Count >= 1) { // Changed > To >=
				if (activeBattlers[i].isPlayer) {
					
					BattleChar playerData = activeBattlers[i];

					playerNames[i].gameObject.SetActive(true);
					playerNames[i].text = playerData.charName;
					playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
					playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;

				} else {
					
					playerNames[i].gameObject.SetActive(false);
				}
			} else {
				playerNames[i].gameObject.SetActive(false);
			}
		}
	}

	public void PlayerAttack(string moveName , int selectedTarget) {
		
		int movePower = 0;
		//find correct attact from movesAvailiable list
		for (int i = 0; i < movesList.Length; i++) {
			if (movesList[i].moveName == moveName) {
				Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
				movePower = movesList[i].movePower;
			}
		}

		//Instantiate(damageUI, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation).SetDamageText(movePower);
		//Display Enemy Attack Effect
		if (enemyAttackEffect != null) {
			Instantiate(enemyAttackEffect, activeBattlers[currentTrun].transform.position, activeBattlers[currentTrun].transform.rotation);
		}
		DealDamage(selectedTarget, movePower);

		uiButtonsHolder.SetActive(false);
		targetMenuUI.SetActive(false);
		NextTurn();
	}

	public void OpenTargetMenu(string moveName) {
		//Open Menu
		targetMenuUI.SetActive(true);
		//Create Enemies List
		List<int> Enemies = new List<int>();
		for (int i = 0; i < activeBattlers.Count; i++) {
			if (!activeBattlers[i].isPlayer) {
				Enemies.Add(i);

			}
		}
		//Set Target Button for Enemies
		for (int i = 0; i < targetButtons.Length; i++) {
			if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0) {
				targetButtons[i].gameObject.SetActive(true);
				targetButtons[i].moveName = moveName;
				targetButtons[i].activeBattlerTarget = Enemies[i];
				targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;


			} else {
				targetButtons[i].gameObject.SetActive(false);
			}
		}
	}

	public void OpenMagicMenu() {

		magicMenuUI.SetActive(true);
		for (int i = 0; i < magicButtons.Length; i++) {
			if (activeBattlers[currentTrun].movesAvailiable.Length > i) {
				magicButtons[i].gameObject.SetActive(true);

				magicButtons[i].spellName = activeBattlers[currentTrun].movesAvailiable[i];
				magicButtons[i].spellText.text = magicButtons[i].spellName;

				for (int j = 0; j < movesList.Length; j++) {
					if (movesList[j].moveName == magicButtons[i].spellName) {
						magicButtons[i].spellCost = movesList[j].moveCost;
						magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
					}
				}

			} else {
				magicButtons[i].gameObject.SetActive(false);
			}
		}
	}

	public void Flee() {

		//random chance to flee battle
		int fleeSuccess = Random.Range(0, 100);
		if (fleeSuccess <= chanceToFlee) {
			// End Battle
			fleeing = true;
			StartCoroutine(EndBattleCo());

		} else {
			NextTurn();
			battleNotice.messageText.text = "Couldn't Escape from Battle";
			battleNotice.Activate();
		}
	}

	//*************************   START ITEM IN BATTLE
	public void ShowItemsWindow() {

		if (battleActive) {
			battleItemsMenu.SetActive(true);
			ShowItems();
		}

	}

	public void ShowItems() {

		GameManager.instance.SortItems();
		for (int i = 0; i < itemButtons.Length; i++) {
			itemButtons[i].buttonValue = i;

			if (GameManager.instance.itemsHeld[i] != "") {
				itemButtons[i].buttonImage.gameObject.SetActive(true);
				itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
				itemButtons[i].ammountText.text = GameManager.instance.numberOfItems[i].ToString();
			}else{
				itemButtons[i].buttonImage.gameObject.SetActive(false);
				itemButtons[i].ammountText.text = "";
			}
		}
	}

	public void CloseItemInBattleWindow(){

		battleItemsMenu.SetActive(false);
	}


	public void SelectItemInBattle(Item newItem) {

		activeItem = newItem;

		if (activeItem.isItem) {
			useButtonText.text = "Use";
		}
		if (activeItem.isArmor || activeItem.isWeapon) {
			useButtonText.text = "Equipt";
			//useButtonText.gameObject.SetActive(false);
		}

		itemName.text = activeItem.itemName;
		itemDesc.text = activeItem.itemDescription;
	}

	public void OpenItemCharChoicePanel() {
		if (activeItem == null) {
			return;
		}
		itemCharChoicePanel.SetActive(true);
		for (int i = 0; i < itemCharChoiceNames.Length; i++) {
			itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
			itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
		}
	}

	public void CloseItemCharChoicePanel() {
		itemCharChoicePanel.SetActive(false);
	}

	public void UseItemsInBattle(int selectBattleChar) {

		activeItem.UseItem(selectBattleChar);
		UpdateUIStats();
		CloseItemCharChoicePanel();
		CloseItemInBattleWindow();
		NextTurn();
	}
	//************************   END ITEM IN BATTLE

	public IEnumerator EndBattleCo() {

		battleActive = false;
		uiButtonsHolder.SetActive(false);
		targetMenuUI.SetActive(false);
		magicMenuUI.SetActive(false);
		battleItemsMenu.SetActive(false);

		yield return new WaitForSeconds(0.5f);
		UIFade.instance.FadeToBlack();
		yield return new WaitForSeconds(1.5f);

		//Update Players - Transfer activeBattlers stats back to players stats
		for (int i = 0; i < activeBattlers.Count; i++) {
			if (activeBattlers[i].isPlayer) {
				for (int j = 0; j < GameManager.instance.playerStats.Length; j++) {
					if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName) {
						CharStats thePlayer = GameManager.instance.playerStats[j];
						thePlayer.currentHP = activeBattlers[i].currentHP;
						thePlayer.currentMP = activeBattlers[i].currentMP;
						//thePlayer.strength = activeBattlers[i].strength;
						//thePlayer.defence = activeBattlers[i].defence;
						//thePlayer.weaponPower = activeBattlers[i].wpnPower;
						//thePlayer.armorPower = activeBattlers[i].armrPower;

						// Add and new stats HERE when leaving the battle

					}
				}
			}
			//Remove Active battlers
			Destroy(activeBattlers[i].gameObject);
		}

		UIFade.instance.FadeFromBlack();
		battleScene.SetActive(false);
		activeBattlers.Clear();
		currentTrun = 0;

		if (fleeing) {
			GameManager.instance.battleActive = false;
			fleeing = false;
		} else {
			//Open Reward Screne
			BattleReward.instance.OpenRewardScrene(rewardXP, rewardItems);
		}
		AudioManager.instance.PlayBGM(Camera.main.GetComponent<CameraController>().musicToPlay);
	}

	public IEnumerator GameOverCo(){

		battleActive = false;
		UIFade.instance.FadeToBlack();
		yield return new WaitForSeconds(1f);
		battleScene.SetActive(false);
		SceneManager.LoadScene(gameOverScene);

	}
}
