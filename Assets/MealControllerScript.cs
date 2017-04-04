using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using LitJson;
using UnityEngine.Networking;
using System.Net;

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
	[SerializeField] private GameObject BGTransparency;
	[SerializeField] private GameObject FoodMenuPanel;
	[SerializeField] private GameObject FoodContents;
	[SerializeField] private GameObject MenuItem;
	[SerializeField] private Text ItemNameText;
	private RectTransform FoodContentsRectangle;


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

		Debug.Log (menuItemPositionX);
		Debug.Log (menuItemPositionY);

		FoodContentsRectangle = FoodContents.GetComponent<RectTransform>();
		foodContentPositionY = FoodContents.transform.position.y;
		foodContentPositionX = FoodContents.transform.position.x;
		foodContentSizeDeltaX = FoodContentsRectangle.sizeDelta.x;
		foodContentSizeDeltaY = FoodContentsRectangle.sizeDelta.y;

		BLDMealSelection = PlayerPrefs.GetString ("BLDTodayMeal");

		for (int i = 0; i < 30; i++) {
			InstantiateFoodItem ("Loading");
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RestaurantIsClicked(string _restaurant) {
		restaurantSelected = _restaurant;
		BGTransparency.SetActive (true);
		FoodMenuPanel.SetActive (true);
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
			}

			GameObject[] FindTempItem;
			FindTempItem = GameObject.FindGameObjectsWithTag("MenuItem");

			for (int i = 0; i < Json["hits"].Count; i++) {
				Debug.Log(item_names[i]);

				Transform TempChild = FindTempItem[i].transform.FindChild("ItemName");

				Text TempMenuItemText = TempChild.GetComponent<Text>();
				TempMenuItemText.text = item_names [i];

			}
				

			
		}
	}
	public void DisableTransparent() {
		BGTransparency.SetActive (false);
		FoodMenuPanel.SetActive (false);
	}

	int i=0;

	void InstantiateFoodItem(string _itemName) {
		menuItemPositionX += 1f;
		menuItemPositionY -= 58f;
		foodContentPositionY -= 28;

//		Debug.Log (menuItemPositionY);

		ItemNameText.text = _itemName;

		//instantiate each food menu item
		GameObject aMenuItem = (GameObject)Instantiate (MenuItem);
		aMenuItem.transform.SetParent (FoodContents.transform);
		aMenuItem.transform.localScale = new Vector2 (0.4f, 0.4f);
		aMenuItem.transform.position = new Vector2 (menuItemPositionX, menuItemPositionY);
//		Debug.Log ("x: " + transform.position.x + "y: " + transform.position.y);

		FoodContentsRectangle.sizeDelta = new Vector2(FoodContentsRectangle.sizeDelta.x, FoodContentsRectangle.sizeDelta.y + 42f);
		FoodContents.transform.position = new Vector2 (FoodContents.transform.position.x + 1f, foodContentPositionY);

//		Debug.Log (FoodContents.transform.localScale.y);

	}

	public void BackBtnIsClicked() {
		SceneManager.LoadScene ("Dashboard");
	}

}
