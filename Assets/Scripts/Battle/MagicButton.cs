using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicButton : MonoBehaviour {

	public string spellName;
	public int spellCost;
	public Text spellText;
	public Text costText;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

	public void Press() {

		if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTrun].currentMP >= spellCost) {
			BattleManager.instance.magicMenuUI.SetActive(false);
			BattleManager.instance.OpenTargetMenu(spellName);
			BattleManager.instance.activeBattlers[BattleManager.instance.currentTrun].currentMP -= spellCost;

		} else {

			//let player know they dont have enough MP
			BattleManager.instance.battleNotice.messageText.text = "Not Enough MP!";
			BattleManager.instance.battleNotice.Activate();
			BattleManager.instance.magicMenuUI.SetActive(false);
		}

	}
}
