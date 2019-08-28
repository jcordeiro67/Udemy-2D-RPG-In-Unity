using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	public static Shop instance;

	public GameObject shopMenu;
	public GameObject buyMenu;
	public GameObject sellMenu;

	public Text goldAmount;

	public string[] itemsForSale = new string[40];

	public ItemButton[] buyItemButton;
	public ItemButton[] sellItemsButton;

	public Item selectedItem;
	public Text buyItemName, buyItemDescription, buyItemValue;
	public Text sellItemName, sellItemDescription, sellItemValue;

    // Start is called before the first frame update
    void Start()
    {
		instance = this;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy && !GameMenu.instance.theMenu.activeInHierarchy) {
			OpenShop();
		}
    }

	public void OpenShop() {

		shopMenu.SetActive(true);
		OpenBuyWindow();
		GameManager.instance.shopActive = true;
		goldAmount.text = GameManager.instance.currentGold.ToString() + "g";
	}

	public void CloseShop() {
		shopMenu.SetActive(false);
		GameManager.instance.shopActive = false;

	}

	public void OpenBuyWindow() {
		
		buyMenu.SetActive(true);
		buyItemButton[0].Press();
		if(sellMenu.activeInHierarchy){
			sellMenu.SetActive(false);
		}
		for (int i = 0; i < buyItemButton.Length; i++) {
			buyItemButton[i].buttonValue = i;

			if (itemsForSale[i] != "") {
				buyItemButton[i].buttonImage.gameObject.SetActive(true);
				buyItemButton[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
				buyItemButton[i].ammountText.text = "";
			}else{
				buyItemButton[i].buttonImage.gameObject.SetActive(false);
				buyItemButton[i].ammountText.text = "";
			}
		}
	}

	public void OpenSellWindow() {

		sellMenu.SetActive(true);
		sellItemsButton[0].Press();
		if (buyMenu.activeInHierarchy) {
			buyMenu.SetActive(false);
		}

		ShowSellItems();
	}

	public void CloseActiveWindow() {

		if (buyMenu.activeInHierarchy) {
			buyMenu.SetActive(false);
		}
		if (sellMenu.activeInHierarchy) {
			sellMenu.SetActive(false);
		}
	}

	public void SelectBuyItem(Item buyItem) {

		selectedItem = buyItem;
		buyItemName.text = selectedItem.itemName;
		buyItemDescription.text = selectedItem.itemDescription;
		buyItemValue.text = "Value: " + selectedItem.value + "g";
	}

	public void SelectSellItem(Item sellItem) {
		selectedItem = sellItem;
		sellItemName.text = selectedItem.itemName;
		sellItemDescription.text = selectedItem.itemDescription;
		sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f).ToString() + "g";
	}

	public void BuyItem() {
		
		if (selectedItem != null) {
			if (GameManager.instance.currentGold >= selectedItem.value) {
				GameManager.instance.currentGold -= selectedItem.value;
			
				GameManager.instance.AddItem(selectedItem.itemName);
			}
		}

		goldAmount.text = GameManager.instance.currentGold.ToString() + "g";
	}

	public void SellItem() {
		
		if (selectedItem != null) {
			GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);
			GameManager.instance.RemoveItems(selectedItem.itemName);
		}

		goldAmount.text = GameManager.instance.currentGold.ToString() + "g";
		ShowSellItems();

	}

	private void ShowSellItems()
	{
		GameManager.instance.SortItems();
		for (int i = 0; i < sellItemsButton.Length; i++) {
			sellItemsButton[i].buttonValue = i;
			if (GameManager.instance.itemsHeld[i] != "") {
				sellItemsButton[i].buttonImage.gameObject.SetActive(true);
				sellItemsButton[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
				sellItemsButton[i].ammountText.text = GameManager.instance.numberOfItems[i].ToString();
			}
			else {
				sellItemsButton[i].buttonImage.gameObject.SetActive(false);
				sellItemsButton[i].ammountText.text = "";
			}
		}
	}
}
