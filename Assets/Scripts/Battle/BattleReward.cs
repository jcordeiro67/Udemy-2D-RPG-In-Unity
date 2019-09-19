using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleReward : MonoBehaviour {

	public static BattleReward instance;

	public Text xpText, itemText;
	public GameObject rewardScrene;

	public string[] rewardItems;
	public int xpEarned;

    // Start is called before the first frame update
    void Start() {
		instance = this;

    }

    // Update is called once per frame
    void Update() {
		if (Input.GetKeyDown(KeyCode.Y)) {
			OpenRewardScrene(100, new string[] {"Iron Sword", "Iron Armor"});
		}
    }

	public void OpenRewardScrene(int xp, string[] rewards) {

		xpEarned = xp;
		rewardItems = rewards;

		xpText.text = "Everyone earned " + xpEarned + " xp!";
		itemText.text = "";

		for (int i = 0; i < rewardItems.Length; i++) {
			itemText.text += rewards[i] + "\n";
		}

		rewardScrene.SetActive(true);

	}

	public void CloseRewardScrene() {
		//Reward XP when window closes
		for (int i = 0; i < GameManager.instance.playerStats.Length; i++) {
			if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy) {
				GameManager.instance.playerStats[i].AddExp(xpEarned);
			}
		}
		//Reward Items earned in battle
		for (int i = 0; i < rewardItems.Length; i++) {
			GameManager.instance.AddItem(rewardItems[i]);
		}

		rewardScrene.SetActive(false);
		GameManager.instance.battleActive = false;

	}
}
