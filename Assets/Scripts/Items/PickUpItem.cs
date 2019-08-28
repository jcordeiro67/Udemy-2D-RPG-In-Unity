using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(Item))]

public class PickUpItem : MonoBehaviour {

	public bool canPickUp;
	public AudioClip pickupSound;
	public GameObject pickupParticles;
	private Item item;



    // Start is called before the first frame update
    void Start() {
		item = GetComponent<Item>();

	}

	//Pickup Item
	void OnTriggerEnter2D(Collider2D other){

		if (other.tag == "Player" && canPickUp && PlayerController.instance.canMove) {
			//print(item.itemName); 
			GameManager.instance.AddItem(item.itemName);

			//play pickup sound and pickup particles
			if (pickupSound != null) {
				AudioSource.PlayClipAtPoint(pickupSound, transform.position);
			}
			if (pickupParticles != null) {
				Instantiate(pickupParticles, transform.position, Quaternion.identity);
			}
			//Destroy gameobject
			Destroy(gameObject);
		}
	}
}
