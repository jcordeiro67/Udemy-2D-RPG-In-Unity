using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Macros;
using System.ComponentModel;

public class Item : MonoBehaviour {

	public string itemName;

	[Header("Item Type")]
	public bool isItem;
	public bool isWeapon;
	public bool isArmor;

	[Header("Item Details")]
	public string itemDescription;
	public int value;
	public Sprite itemSprite;
	public int amountToChange;
	public bool affectHP, affectMP, affectStr, affectDef;

	[Header("Weapon/Armor Details")]
	public int weaponStrength;
	public int armorStrength;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UseItem(int charToUseOn) {

		CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];

		if (isItem) {
			if (affectHP) {
				selectedChar.currentHP += amountToChange;
				if (selectedChar.currentHP > selectedChar.maxHP) {
					selectedChar.currentHP = selectedChar.maxHP;
				}
			}

			if (affectMP) {
				selectedChar.currentMP += amountToChange;
				if (selectedChar.currentMP > selectedChar.maxMP) {
					selectedChar.currentMP = selectedChar.maxMP;
				}
			}

			if (affectStr) {
				selectedChar.strength += amountToChange;
			}
		}

		if (isWeapon) {
			if (selectedChar.equippedWeapon != "") {
				GameManager.instance.AddItem(selectedChar.equippedWeapon);
			}
			selectedChar.equippedWeapon = itemName;
			selectedChar.weaponPower = weaponStrength;
		}

		if (isArmor) {
			if (selectedChar.equippedArmor!= "") {
				GameManager.instance.AddItem(selectedChar.equippedArmor);
			}
			selectedChar.equippedArmor = itemName;
			selectedChar.armorPower = armorStrength;
		}

		GameManager.instance.RemoveItems(itemName);
	}
}
