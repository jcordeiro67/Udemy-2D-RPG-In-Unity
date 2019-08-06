using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

	public GameObject theMenu;
	public CharStats[] playerStats;
	public GameObject[] windows;
	public int openWindowSound;
	public int closeWindowSound;
	public int buttonClickSound;

	[Header("Stats UI Components")]
	public Text[] nameText;
	public Text[] hpText;
	public Text[] mpText;
	public Text[] levlText;
	public Text[] expText;
	public Slider[] expSlider;
	public Image[] charImage;
	public GameObject[] charStatHolder;

	[Header("Player Stats")]
	public GameObject[] statusButtons;
	public Text statusName;
	public Text statusHP;
	public Text statusMP;
	public Text statusStr;
	public Text statusDef;
	public Text statusWeapon;
	public Text statusWpnPwr;
	public Text statusArmor;
	public Text statusArmorPwr;
	public Text statusExp;
	public Image statusImage;
	public Text goldAmountText;

	[Header("Items Info")]
	public ItemButton[] itemButtons;
	public string selectedItem;
	public Item activeItem;
	public Text itemName;
	public Text itemDesc; 
	public Text useButtonText;

	public GameObject itemCharChoicePanel;
	public Text[] itemCharChoiceNames;

	public static GameMenu instance;


	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire2") && !Shop.instance.shopMenu.activeInHierarchy) {
			if (theMenu.activeInHierarchy) {
				CloseMenu();
			}else{
				theMenu.SetActive(true);
				UpdateMainStats();
				GameManager.instance.gameMenuOpen = true;
				AudioManager.instance.PlaySFX(openWindowSound);

			}
		}
	}

	public void UpdateMainStats() {
		playerStats = GameManager.instance.playerStats;

		for (int i = 0; i < playerStats.Length; i++) {
			if (playerStats[i].gameObject.activeInHierarchy) {
				charStatHolder[i].SetActive(true);
				nameText[i].text = playerStats[i].charName;
				hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
				mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
				levlText[i].text = "Level " + playerStats[i].playerLevel;
				expText[i].text = playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
				charImage[i].sprite = playerStats[i].charImage;
				expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
				expSlider[i].value = playerStats[i].currentEXP;
			} else {
				charStatHolder[i].SetActive(false);
			}
		}
		goldAmountText.text = GameManager.instance.currentGold.ToString() + "g";
	}

	public void ToggleWindow(int windowNumber){
		
		UpdateMainStats();

		for (int i = 0; i < windows.Length; i++) {
			if (i == windowNumber && !windows[i].activeInHierarchy) {
				windows[i].SetActive(!windows[i].activeInHierarchy);
			}else {
				windows[i].SetActive(false);
			}
		}

		CloseItemCharChoicePanel();
	}

	public void CloseMenu(){
		
		for (int i = 0; i < windows.Length; i++) {
			windows[i].SetActive(false);
		}
		theMenu.SetActive(false);
		GameManager.instance.gameMenuOpen = false;
		AudioManager.instance.PlaySFX(closeWindowSound);
		CloseItemCharChoicePanel();
	}

	public void OpenStats() {
		
		//update stats info
		UpdateMainStats();
		StatusChar(0);
		for (int i = 0; i < statusButtons.Length; i++) {
			statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
			statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
		}
	}

	public void StatusChar(int selected) {
		
		statusName.text = playerStats[selected].charName;
		statusHP.text = "" + playerStats[selected].currentHP + " / " + playerStats[selected].maxHP;
		statusMP.text = "" + playerStats[selected].currentMP + " / " + playerStats[selected].maxMP;
		statusStr.text = playerStats[selected].strength.ToString();
		statusDef.text = playerStats[selected].defence.ToString();

		if (playerStats[selected].equippedWeapon != "") {
			statusWeapon.text = playerStats[selected].equippedWeapon;

		}else {
			statusWeapon.text = "None";
		}
		statusWpnPwr.text = playerStats[selected].weaponPower.ToString();

		if (playerStats[selected].equippedArmor != "") {
			statusArmor.text = playerStats[selected].equippedArmor;
		}else {
			statusArmor.text = "None";
		}
		statusArmorPwr.text = playerStats[selected].armorPower.ToString();
		statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();
		statusImage.sprite = playerStats[selected].charImage;
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

	public void SelectItem(Item newItem) {
		
		activeItem = newItem;

		if (activeItem.isItem) {
			useButtonText.text = "Use";
		}
		if (activeItem.isArmor || activeItem.isWeapon) {
			useButtonText.text = "Equipt";
		}

		itemName.text = activeItem.itemName;
		itemDesc.text = activeItem.itemDescription;
	}

	public void DiscardItem(){
		
		if (activeItem != null) {
			GameManager.instance.RemoveItems(activeItem.itemName);
		}

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

	public void UseItem(int selectChar) {

		activeItem.UseItem(selectChar);
		CloseItemCharChoicePanel();
	}

	//Testing Fix
	public void DefaultToNoItemSelected() {
		activeItem = null;
		itemName.text = "Item:";
		itemDesc.text = "No Item Selected!";
		itemCharChoicePanel.SetActive(false);
	}

	public void SaveGame(){

		GameManager.instance.SaveGameData();
		QuestManager.instance.SaveQuestData();
	}

	public void LoadSavedGame(){

		GameManager.instance.LoadGameData();
		QuestManager.instance.LoadQuestData();
	}

	public void PlayButtonClick() {

		AudioManager.instance.PlaySFX(buttonClickSound);
	}
	//Show Equipped item using activeItem and isWeapon
}
