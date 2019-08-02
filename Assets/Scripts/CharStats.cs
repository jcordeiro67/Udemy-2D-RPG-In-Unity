using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour {

	public string charName;
	public int playerLevel = 1;
	public int currentEXP;
	public int maxLevel = 100;
	public int baseExp = 1000;
	public int currentHP;
	public int maxHP = 100;
	public int currentMP;
	public int maxMP = 30;
	public int strength;
	public int defence;
	public int weaponPower;
	public int armorPower;
	public string equippedWeapon;
	public string equippedArmor;
	public Sprite charImage;
	public int[] expToNextLevel;
	public int[] mpLvlBonus;


	// Use this for initialization
	void Start () {
		expToNextLevel = new int[maxLevel];
		mpLvlBonus = new int[maxLevel];
		expToNextLevel[1] = baseExp;
		mpLvlBonus[1] = maxMP;

		//for each item in expToNextLevel multiply previous level by 1.05
		for (int i = 2; i < expToNextLevel.Length; i++) {
			expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
		}

		//for each item in levelBonus add 5 to previous level
		for (int i = 2; i < mpLvlBonus.Length; i++) {
			mpLvlBonus[i] = mpLvlBonus[i - 1] + 5;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K)) {
			AddExp(500);
		}
	}

	public void AddExp(int expToAdd){
		currentEXP += expToAdd;

		if (playerLevel < maxLevel) {
			while (currentEXP >= expToNextLevel[playerLevel] && playerLevel < maxLevel) {
				currentEXP -= expToNextLevel[playerLevel];
				playerLevel++;
			
				//determine whether to add to str or def on odd or even
				if (playerLevel % 2 == 0) {
					strength++;
				} else {
					defence++;
				}


				maxHP = Mathf.FloorToInt(maxHP * 1.05f);
				currentHP = maxHP;

				maxMP += mpLvlBonus[playerLevel];
				currentMP = maxMP;
			}
		}

		if (playerLevel >= maxLevel) {
			currentEXP = 0;
		}

	}
}
