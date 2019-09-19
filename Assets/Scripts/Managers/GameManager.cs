using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public CharStats[] playerStats;

	public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive, battleActive;

	public string[] itemsHeld;
	public int[] numberOfItems;
	public Item[] referenceItems;

	public int currentGold;

	public bool loadData = false;

	void Awake () {
		// Check if this already exists in scene
		if (instance == null) {
			instance = this;
		}else {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	void Start () {
		
		SortItems();

	}
	
	// Update is called once per frame
	void Update () {
		//stop player from moving if one of the menus or dialogs are open
		if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive) {
			PlayerController.instance.canMove = false;
		}else {
			PlayerController.instance.canMove = true;
		}

		if (loadData) {
			PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), 
				PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

			loadData = false;
		}

	}

	/// <summary>
	/// Gets the item details.
	/// </summary>
	/// <returns>The item details.</returns>
	/// <param name="itemToGrab">Item to grab.</param>
	public Item GetItemDetails(string itemToGrab) {

		for (int i = 0; i < referenceItems.Length; i++) {
			if (referenceItems[i].itemName == itemToGrab) {
				return referenceItems[i];
			}
		}

		return null;
	}

	/// <summary>
	/// Sorts the items.
	/// </summary>
	public void SortItems() {
		bool itemAfterSpace = true;

		while (itemAfterSpace) {
			itemAfterSpace = false;
			for (int i = 0; i < itemsHeld.Length - 1; i++) {
				if (itemsHeld[i] == "") {
					itemsHeld[i] = itemsHeld[i + 1];
					itemsHeld[i + 1] = "";
			
					numberOfItems[i] = numberOfItems[i + 1];
					numberOfItems[i + 1] = 0;

					if (itemsHeld[i] != "") {
						itemAfterSpace = true;
					}
				}
			}
		}
	}

	/// <summary>
	/// Adds the item.
	/// </summary>
	/// <param name="itemToAdd">Item to add.</param>
	public void AddItem(string itemToAdd) {

		int newItemPosition = 0;
		bool foundSpace = false;

		for (int i = 0; i < itemsHeld.Length; i++) {
			if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd) {
				newItemPosition = i;
				i = itemsHeld.Length;
				foundSpace = true;
			}
		}

		if (foundSpace) {
			bool itemExists = false;
			for (int i = 0; i < referenceItems.Length; i++) {
				if (referenceItems[i].itemName == itemToAdd) {

					itemExists = true;
					i = referenceItems.Length;
				}
			}

			if (itemExists) {
				itemsHeld[newItemPosition] = itemToAdd;
				numberOfItems[newItemPosition]++;
			} else{
				Debug.LogError(itemToAdd + " Does not exist!");
			}
		}

		GameMenu.instance.ShowItems();
	}

	/// <summary>
	/// Removes the items.
	/// </summary>
	/// <param name="itemToRemove">Item to remove.</param>
	public void RemoveItems(string itemToRemove) {
		
		bool foundItem = false;

		int itemPosition = 0;

		for (int i = 0; i < itemsHeld.Length; i++) {
			if (itemsHeld[i] == itemToRemove) {
				foundItem = true;
				itemPosition = i;

				i = itemsHeld.Length;
			}
		}

		if (foundItem) {
			numberOfItems[itemPosition]--;
			if (numberOfItems[itemPosition] <= 0) {
				itemsHeld[itemPosition] = "";
				//Fix Added
				GameMenu.instance.DefaultToNoItemSelected();
			}
			GameMenu.instance.ShowItems();
		} else {
			Debug.LogError("Couldn't find " + itemToRemove);
		}
	}

	/// <summary>
	/// Saves the game data.
	/// </summary>
	public void SaveGameData() {
		
		//Save Current Scene
		PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);

		//Save Position of Player in Scene
		PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
		PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
		PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

		//Save Current Gold
		PlayerPrefs.SetInt("Current_Gold", GameManager.instance.currentGold);

		//Save Quest Data
		//QuestManager.instance.SaveQuestData();

		//Save Player Stats
		for (int i = 0; i < playerStats.Length; i++) {
			
			if (playerStats[i].gameObject.activeInHierarchy) {
				PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
			} else {
				PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
			}

			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_level", playerStats[i].playerLevel);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Hp", playerStats[i].currentHP);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_maxHp", playerStats[i].maxHP);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Mp", playerStats[i].currentMP);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_maxMp", playerStats[i].maxMP);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Exp", playerStats[i].currentEXP);
			PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_armor", playerStats[i].equippedArmor);
			PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_weapon", playerStats[i].equippedWeapon);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_armorPwr", playerStats[i].armorPower);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_weaponPwr", playerStats[i].weaponPower);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_strength", playerStats[i].strength);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_defence", playerStats[i].defence);
		}

		// Store Inventory Items and Amount
		for (int i = 0; i < itemsHeld.Length; i++) {
			PlayerPrefs.SetString("ItemsInInventory_" + i, itemsHeld[i]);
			PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
		}

		Debug.Log("Game Data Saved");
	}

	/// <summary>
	/// Loads the game data.
	/// </summary>
	public void LoadGameData() {
		
		//Load Scene

		//Load Players position moved to Update
//		PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), 
//			PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

		PlayerController.instance.areaTransitionName = "";
		SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));

		//Load Players Current Gold
		GameManager.instance.currentGold = PlayerPrefs.GetInt("Current_Gold");

		//Load Players Quest Data
		//QuestManager.instance.LoadQuestData();

		//Load Players stats
		for (int i = 0; i < playerStats.Length; i++) {

			if(PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0){
				playerStats[i].gameObject.SetActive(false);
			} else {
				playerStats[i].gameObject.SetActive(true);
			}

			playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_level");
			playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Hp");
			playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_maxHp");
			playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Mp");
			playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_maxMp");
			playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Exp");
			playerStats[i].equippedArmor = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_armor");
			playerStats[i].equippedWeapon = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_weapon");
			playerStats[i].armorPower = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_armorPwr");
			playerStats[i].weaponPower = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_weaponPwr");
			playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_strength");
			playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_defence");
		}

		//Load Inventory
		for (int i = 0; i < itemsHeld.Length; i++) {
			itemsHeld[i] = PlayerPrefs.GetString("ItemsInInventory_" + i);
			numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
		}

		loadData = true;
		Debug.Log("Game Data Loaded");

	}
}
