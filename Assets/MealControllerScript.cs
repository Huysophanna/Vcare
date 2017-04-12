﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using LitJson;
using UnityEngine.Networking;
using System.Net;
using UnityEngine.EventSystems;

public class MealControllerScript : MonoBehaviour {
	private string BLDMealSelection;
	private string restaurantSelected;
	private float menuItemPositionX;
	private float NewmenuItemPositionX;
	private float menuItemPositionY;
	private float NewmenuItemPositionY;
	private float foodContentPositionY;
	private float foodContentPositionX;
	private float foodContentSizeDeltaX;
	private float foodContentSizeDeltaY;


	private JsonData Json;
	private string[] item_names = new string[50];
	private string[] calories = new string[50];
	private string[] total = new string[50];
	private RectTransform FoodContentsRectangle;
	private GameObject[] FindTempItem;

	private String[] BrfTodayItem;
	private String[] BrfTodayItemCalories;
//	private List<string> LunchTodayItem = new List<string>();
//	private List<string> LunchTodayItemCalories = new List<string>();
//	private List<string> DinnerTodayItem = new List<string>();
//	private List<string> DinnerTodayCalories = new List<string>();

	[SerializeField] private GameObject BGTransparency;
	[SerializeField] private GameObject FoodMenuPanel;
	[SerializeField] private GameObject FoodContents;
	[SerializeField] private GameObject MenuItem;
	[SerializeField] private Text ItemNameText;
	[SerializeField] private GameObject SubTitleText;
	[SerializeField] private GameObject SuccessPanel;
	[SerializeField] private Text SuccessText;

	private static MealControllerScript _instance;
	public static MealControllerScript Instance {
		get {
			return _instance;
		}
	}

	void Awake() {
		_instance = this;
	}

	private string breakfastTodayCalories = "";
	private string lunchTodayCalories = "";
	private string dinnerTodayCalories = "";
	private string snackTodaycalories = "";
	
	// Use this for initialization
	void Start () {

		BrfTodayItem = new string[50];
		BrfTodayItemCalories = new string[50];

		Assert.IsNotNull (BGTransparency);
		Assert.IsNotNull (FoodMenuPanel);
		Assert.IsNotNull (FoodContents);
		Assert.IsNotNull (SubTitleText);
		Assert.IsNotNull (ItemNameText);
		Assert.IsNotNull (SuccessPanel);

		menuItemPositionX = MenuItem.transform.position.x;
		menuItemPositionY = MenuItem.transform.position.y;

		Debug.Log (menuItemPositionX);
		Debug.Log (menuItemPositionY);

		FoodContentsRectangle = FoodContents.GetComponent<RectTransform>();
		foodContentPositionY = FoodContents.transform.position.y;
		foodContentPositionX = FoodContents.transform.position.x;
		foodContentSizeDeltaX = FoodContentsRectangle.sizeDelta.x;
		foodContentSizeDeltaY = FoodContentsRectangle.sizeDelta.y;

		BLDMealSelection = PlayerPrefs.GetString ("BLDTodayMeal");

		for (int i = 0; i < 24; i++) {
			InstantiateFoodItem ("Loading");
		}

	}
	
	// Update is called once per frame
	void Update () {
		GetUserBLDMealToday ();
		
	}

	public void RestaurantIsClicked(string _restaurant) {
		restaurantSelected = _restaurant;
		BGTransparency.SetActive (true);
		FoodMenuPanel.SetActive (true);
		SubTitleText.SetActive (true);

		StartCoroutine(APIcall(restaurantSelected));
	}
	// Request food data 
	public IEnumerator APIcall(string brand)
	{
		string URL = "https://api.nutritionix.com/v1_1/search";
		string brand_name = "\""+brand+"\"";
		string jsonData = "";
		jsonData = "{\"appId\":\"56f421e1\",\"appKey\":\"dd320eab137447ef0e3b13796fca8230\",\"fields\":[\"item_name\",\"nf_calories\"],\"offset\":0,\"limit\":25,\"queries\":{\"brand_name\":"+brand_name+"},\"filters\":{\"item_type\":1}}";

		var request = new UnityWebRequest(URL, "POST");
		byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
		request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.Send();

		if (request.isError)
		{
			Debug.Log(request.error);
		}
		else
		{
			Debug.Log(request.downloadHandler.text);
			// Show result
			Json = JsonMapper.ToObject(request.downloadHandler.text);
			for (int i = 0; i < Json["hits"].Count; i++) {
				item_names[i] = Json["hits"][i]["fields"]["item_name"].ToString();
				calories[i] = Json["hits"][i]["fields"]["nf_calories"].ToString();
				total[i] = item_names[i] + "." + calories[i];


			FindTempItem = GameObject.FindGameObjectsWithTag("MenuItem");
				// TODO: need to store calories as key value paired with item name
			}

			for (int i = 0, j=1; i < Json["hits"].Count; i++, j++) {
				Debug.Log(item_names[i]);

				Transform TempChild = FindTempItem[i].transform.FindChild("ItemName");

				Text TempMenuItemText = TempChild.GetComponent<Text>();
				TempMenuItemText.text = j + ". " + item_names [i];

			}
		}
	}

	private void GetUserBLDMealToday() {

//		Debug.Log(PlayerPrefs.GetString ("BLDTodayMeal"));
//		Debug.Log(PlayerPrefs.GetString ("breakfastToday"));
//		Debug.Log(PlayerPrefs.GetString ("lunchToday"));
//		Debug.Log(PlayerPrefs.GetString ("dinnerToday"));

		breakfastTodayCalories = PlayerPrefs.GetString ("breakfastTodayCalories");
		lunchTodayCalories = PlayerPrefs.GetString ("lunchTodayCalories");
		dinnerTodayCalories = PlayerPrefs.GetString ("dinnerTodayCalories");
		snackTodaycalories = PlayerPrefs.GetString ("snackTodayCalories");

		Debug.Log(breakfastTodayCalories);
		Debug.Log(lunchTodayCalories);
		Debug.Log(dinnerTodayCalories);
		Debug.Log(snackTodaycalories);
	}

	public void EachItemIsClicked() {
//		string[] test = PlayerPrefsX.GetStringArray (BLDMealSelection + "Today");


		
		Text ItemClicked = EventSystem.current.currentSelectedGameObject.GetComponent<Text>();

		//get only quried data
		if (ItemClicked.text != "Loading") {
			// TODO: store data, playerprefs as of now
			//meal return, breakfast, lunch, dinner
//			
//			if (BLDMealSelection == "breakfast") {
//				BrfTodayItem.SetValue (ItemClicked.text, brfIndex);
//				BrfTodayItemCalories.SetValue (calories [Int32.Parse (ItemClicked.text.ToString ().Substring (0, 1)) - 1], brfIndex);
//				brfIndex = brfIndex + 1;
////				PlayerPrefs.SetInt ("BrfIndex", brfIndex);

//				PlayerPrefs.SetString (BLDMealSelection + "Today", ItemClicked.text);
	
//			}

			Debug.Log (BrfTodayItem.Length + " " + BrfTodayItemCalories.Length);
//
//			string[] ItemConvertToArray = SelectedTodayItem.ToArray ();
//			string[] BrfCaloriesToArray = SelectedTodayItemCalories.ToArray ();
//
//			PlayerPrefsX.SetStringArray (BLDMealSelection + "Today", BrfCaloriesToArray);
//			PlayerPrefsX.SetStringArray (BLDMealSelection + "TodayCalories", BrfCaloriesToArray);

			if (BLDMealSelection == "breakfast") {
				DashboardScript.Instance.AddBrfCalories (ItemClicked.text, calories[Int32.Parse(ItemClicked.text.ToString().Substring(0, 1)) - 1]);
			}

			if (BLDMealSelection == "lunch") {
				DashboardScript.Instance.AddLucnchCalories (ItemClicked.text, calories[Int32.Parse(ItemClicked.text.ToString().Substring(0, 1)) - 1]);
			}

			if (BLDMealSelection == "dinner") {
				DashboardScript.Instance.AddDinnerCalories (ItemClicked.text, calories[Int32.Parse(ItemClicked.text.ToString().Substring(0, 1)) - 1]);
			}

			if (BLDMealSelection == "snack") {
				DashboardScript.Instance.AddSnackCalories (ItemClicked.text, calories[Int32.Parse(ItemClicked.text.ToString().Substring(0, 1)) - 1]);
			}
				
//			Debug.Log (BLDMealSelection + " " + PlayerPrefs.GetString (BLDMealSelection + "Today") + PlayerPrefs.GetString (BLDMealSelection + "TodayCalories"));




			PresentSuccessPopup ();
		}
	}

	private void PresentSuccessPopup() {
		SuccessText.text = "Success!\nYour " + BLDMealSelection + " Item was selected.\nYou can check the detail in report section in the avatar screen.";
		SuccessPanel.SetActive (true);
		
	}

	public void ClosePopup() {
		SuccessPanel.SetActive (false);
		DisableTransparent ();

		SceneManager.LoadScene ("Dashboard");
	}

	public void DisableTransparent() {
		BGTransparency.SetActive (false);
		FoodMenuPanel.SetActive (false);
		SubTitleText.SetActive (false);


		for (int i = 0, j=1; i < FindTempItem.Length; i++, j++) {
			Transform TempChild = FindTempItem[i].transform.FindChild("ItemName");

			Text TempMenuItemText = TempChild.GetComponent<Text>();
			TempMenuItemText.text = "Loading";

		}
	}

	int i=0;

	void InstantiateFoodItem(string _itemName) {
		menuItemPositionX += 1f;
		menuItemPositionY -= 88f;
		foodContentPositionY -= 28;

//		Debug.Log (menuItemPositionY);

		ItemNameText.text = _itemName;

		//instantiate each food menu item
		GameObject aMenuItem = (GameObject)Instantiate (MenuItem);
		aMenuItem.transform.SetParent (FoodContents.transform);
		aMenuItem.transform.localScale = new Vector2 (0.4f, 0.4f);
		aMenuItem.transform.position = new Vector2 (menuItemPositionX, menuItemPositionY);
//		Debug.Log ("x: " + transform.position.x + "y: " + transform.position.y);

		FoodContentsRectangle.sizeDelta = new Vector2(FoodContentsRectangle.sizeDelta.x, FoodContentsRectangle.sizeDelta.y + 42f);   //42
		FoodContents.transform.position = new Vector2 (FoodContents.transform.position.x + 1f, foodContentPositionY);

//		Debug.Log (FoodContents.transform.localScale.y);

	}

		
	public void BackBtnIsClicked() {
		SceneManager.LoadScene ("Dashboard");
	}

}
