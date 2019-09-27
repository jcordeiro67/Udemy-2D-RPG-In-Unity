using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetButton : MonoBehaviour {

	public string moveName;
	public int activeBattlerTarget;
	public Text targetName;

	public void Press() {
		BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);
	}
}
