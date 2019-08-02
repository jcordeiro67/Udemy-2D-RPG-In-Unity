using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
	private bool canOpen;
	//For Later Use in Shop UI
	//public Sprite npcSprite;
	public string areaHintText;
	public string[] itemsForSale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove && !Shop.instance.shopMenu.activeInHierarchy) {
			
			Shop.instance.itemsForSale = itemsForSale;
			Shop.instance.OpenShop();
		}
    }

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			canOpen = true;
			DialogManager.instance.ShowAreaHint(areaHintText);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			canOpen = false;
			DialogManager.instance.DisableAreaHint();
		}
	}
}
