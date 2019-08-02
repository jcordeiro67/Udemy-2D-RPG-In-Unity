using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTEst : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(){
		print(transform.name + " Trigger Entered");
	}
}
