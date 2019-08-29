using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	private bool battleActive;

    // Start is called before the first frame update
    void Start() {
		instance = this;
		DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update() {
		
		//TESTING AREA
		if (Input.GetKeyDown(KeyCode.T)) {
			BattleStart(new string[] {"Troll", "Skeleton", "Wizzard", "Skeleton", "Spider", "Wizzard"} );
		}

		if (Input.GetKeyDown(KeyCode.N)) {
			NextTurn();
		}
		//END TESTING

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
		currentTrun = Random.Range(0, activeBattlers.Count);
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
	}

	public void UpdateBattle() {

		bool allEnemiesDead = true;
		bool allPlayersDead = true;

		for (int i = 0; i < activeBattlers.Count; i++) {
			if (activeBattlers[i].currentHP <=0) {
				activeBattlers[i].currentHP = 0;

			}
			if (activeBattlers[i].currentHP == 0) {
				//handle dead battler
				//activeBattlers[i].hasDied = true;

			} else {
				
				if(activeBattlers[i].isPlayer){
					allPlayersDead = false;

				} else {
					allEnemiesDead = false;
				}
			}
		}

		//End of battle
		if (allEnemiesDead || allPlayersDead) {
			if (allEnemiesDead) {
				//end battle in victory
			} else {
				//end battle in failure
			}

			battleScene.SetActive(false);
			GameManager.instance.battleActive = false;
			battleActive = false;
		}
	}

	public IEnumerator EnemyMoveCo() {

		turnWaiting = false;
		yield return new WaitForSeconds(enemyAttackWait);
		EnemyAttack();
		yield return new WaitForSeconds(1f);
		NextTurn();
	}

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
		int movePower = 0;

		//Select which move of availaible move list to use for attack
		int selectAttack = Random.Range(0, activeBattlers[currentTrun].movesAvailiable.Length);
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
	}
}
