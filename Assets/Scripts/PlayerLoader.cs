using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoader : MonoBehaviour {

	public GameObject PlayerPrefab;

	// Use this for initialization
	void Awake () {
		if (PlayerController.instance == null) {
			Instantiate(PlayerPrefab, gameObject.transform.position, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
