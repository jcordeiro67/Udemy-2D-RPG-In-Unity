using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour {
	

	void Start () {

		var transitionName = transform.parent.GetComponent<AreaExit>().areaTransitionName;

		if (transitionName == PlayerController.instance.areaTransitionName) {
			PlayerController.instance.transform.position = transform.position;

			if (transform.parent.GetComponent<AreaExit>().resetPlayerOnEntrance) {
				PlayerController.instance.GetComponent<Animator>().SetFloat("lastMoveX", 0);
				PlayerController.instance.GetComponent<Animator>().SetFloat("lastMoveY", 0);
			}
		}

		UIFade.instance.FadeFromBlack();
	}

}
