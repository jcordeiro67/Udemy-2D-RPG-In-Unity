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
	public GameObject uiButtonsHolder;
	//public BattleMove[] movesList;
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
			BattleStart(new string[] {"Troll", "Spider", "Troll", "Skeleton"} );
		}
		//END TESTING
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

		}

	}
}
