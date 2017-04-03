using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MealControllerScript : MonoBehaviour {
	private string BLDMealSelection;
	private string restaurantSelected;
	private float menuItemPositionX;
	private float menuItemPositionY;
	private float foodContentPositionY;
	[SerializeField] private GameObject BGTransparency;
	[SerializeField] private GameObject FoodMenuPanel;
	[SerializeField] private GameObject FoodContents;
	[SerializeField] private GameObject MenuItem;
	[SerializeField] private Text ItemNameText;

	private static MealControllerScript _instance;
	public static MealControllerScript Instance {
		get {
			return _instance;
		}
	}

	void Awake() {
		_instance = this;
	}
	
	// Use this for initialization
	void Start () {
		Assert.IsNotNull (BGTransparency);
		Assert.IsNotNull (FoodMenuPanel);
		Assert.IsNotNull (FoodContents);
		Assert.IsNotNull (ItemNameText);

		menuItemPositionX = MenuItem.transform.position.x;
		menuItemPositionY = MenuItem.transform.position.y;
		foodContentPositionY = FoodContents.transform.position.y;

		BLDMealSelection = PlayerPrefs.GetString ("BLDTodayMeal");


		for (int i = 0; i < 25; i++) {
			InstantiateFoodItem ("Pizza");
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RestaurantIsClicked(string _restaurant) {
		restaurantSelected = _restaurant;

		BGTransparency.SetActive (true);
		FoodMenuPanel.SetActive (true);
	}

	public void DisableTransparent() {
		BGTransparency.SetActive (false);
		FoodMenuPanel.SetActive (false);
	}

	void InstantiateFoodItem(string _itemName){

		Debug.Log (menuItemPositionY);

		menuItemPositionX += 1f;
		menuItemPositionY -= 58f;
		foodContentPositionY -= 28;

		Debug.Log (menuItemPositionY);

		ItemNameText.text = _itemName;
		//instantiate each food menu item
		GameObject aMenuItem = (GameObject)Instantiate (MenuItem);
		aMenuItem.transform.SetParent (FoodContents.transform);
		aMenuItem.transform.localScale = new Vector2 (0.4f, 0.4f);
		aMenuItem.transform.position = new Vector2 (menuItemPositionX, menuItemPositionY);
		Debug.Log ("x: " + transform.position.x + "y: " + transform.position.y);

		RectTransform FoodContentsRectangle = FoodContents.GetComponent<RectTransform>();
		FoodContentsRectangle.sizeDelta = new Vector2(FoodContentsRectangle.sizeDelta.x, FoodContentsRectangle.sizeDelta.y + 42f);
		FoodContents.transform.position = new Vector2 (FoodContents.transform.position.x + 1f, foodContentPositionY);

		Debug.Log (FoodContents.transform.localScale.y);

	}

}
