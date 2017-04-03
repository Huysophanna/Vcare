using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity3dAzure.AppServices;
using System.Net;
using System;
using LitJson;
using UnityEngine.Networking;

public class UserDataScript : MonoBehaviour {

	[SerializeField] private InputField userHeightInput;
	[SerializeField] private InputField userWeightInput;
	[SerializeField] private Dropdown userGenderDropDown;
	[SerializeField] private Dropdown userBirthYearDropDown;
	[SerializeField] private Dropdown userBirthMonthDropDown;
	[SerializeField] private Dropdown userBirthDayDropDown;
	[SerializeField] private Text popUpInfoText;
	[SerializeField] private GameObject AlertPanel;
	[SerializeField] private GameObject BGTransparency;


	private int userBirthMonth;
	private int userBirthMonthIndex;
	private int daysInMonth;
	private int userBirthYear;
	private int userBirthYearIndex;
	private int userBirthDay;
	private int userBirthDayIndex;
	private string userGender = "";
	private JsonData Json;
	private string[] item_names = new string[50];
	private string[] calories = new string[50];
	private string[] total = new string[50];
	private double BMI;

	List<string> birthYear = new List<string>();
	List<string> birthDay = new List<string>();

	private static UserDataScript _instance;
	public static UserDataScript Instance {
		get {
			return _instance;
		}
	}
	void Awake() {
		_instance = this;
	}

	void Start() {
		
		InitializeBirthSelection ();

		//make sure that the input field is not empty, drag and drop in the editor
		Assert.IsNotNull (userHeightInput);
		Assert.IsNotNull (userWeightInput);
		Assert.IsNotNull (userBirthYearDropDown);
		Assert.IsNotNull (userBirthDayDropDown);
		Assert.IsNotNull (popUpInfoText);
		Assert.IsNotNull (userGenderDropDown);
//		StartCoroutine(APIcall());

		//initialize popup alert text with username for NEW user
		popUpInfoText.text = "Hi there " + GameManager.Instance.profileName + ", \n\nWe'd like to know more about you, to assist you in a best way .";

		//if it is a NEW user, show alert info panel
		if (!PlayerPrefs.HasKey ("ExistingUser")) {
			AlertPanel.SetActive (true);
			BGTransparency.SetActive (true);
		}

	}

	// Request food data 
	public IEnumerator APIcall()
	{
		string URL = "https://api.nutritionix.com/v1_1/search";
		string brand = "KFC";
		string brand_name = "\""+brand+"\"";
		string jsonData = "";
		jsonData = "{\"appId\":\"56f421e1\",\"appKey\":\"dd320eab137447ef0e3b13796fca8230\",\"fields\":[\"item_name\",\"nf_calories\"],\"offset\":0,\"limit\":50,\"queries\":{\"brand_name\":"+brand_name+"},\"filters\":{\"item_type\":1}}";

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
			Array.Sort (total);
			Array.Sort (item_names);
			for (int i = 0; i < Json["hits"].Count; i++) {
				calories[i] = total[i].Substring (total[i].IndexOf('.')+1);
			}
		}
	}
	public void SaveData() {
//		Insert ();

		//initialized value if the default dropdown value is selected
		GenderOnChanged (userGenderDropDown.value);
		BirthDayOnChanged (userBirthDayDropDown.value);
		BirthMonthOnChanged (userBirthMonthDropDown.value);
		BirthYearOnChanged (userBirthYearDropDown.value);

		// TODO: Save data into storage
		PlayerPrefs.SetString("UserHeight", userHeightInput.text);
		PlayerPrefs.SetString("UserWeight", userWeightInput.text);
		PlayerPrefs.SetString("UserGender", userGender);
		PlayerPrefs.SetInt("UserBirthDayIndex", userBirthDayIndex);
		PlayerPrefs.SetInt("UserBirthMonthIndex", userBirthMonthIndex);
		PlayerPrefs.SetInt("UserBirthYearIndex", userBirthYearIndex);
		BMICalulation (userHeightInput.text,userWeightInput.text);
//		SceneManager.LoadScene ("Dashboard");
	}

	/* ===============================================================================================
	 * Calculate BMI
	 * ===============================================================================================
	*/

	public void BMICalulation(string userHeight, string userWeight)
	{
		
		double Height = Int32.Parse(userHeight) / 100.0;
		int Weight = Int32.Parse(userWeight);
//		Debug.Log(Weight);
		BMI =  Math.Round(Weight / (Height*Height),1);
		Debug.Log (BMI); 
	}

	/* ===============================================================================================
	 * Calories In Take Recommendation
	 * ===============================================================================================
	*/

	public void CaloriesInTake(double BMI)
	{
		int isActive = 1;
		int Gender = userGender == "Male"? 1 : 2;
		if (Gender == 1) {
			int age = 2017 - userBirthYear;
			if (BMI < 18.5) {
			} 
			else if (18.5 <= BMI && BMI <= 24.9) {
				if (isActive == 3) {
				}
				else if(isActive == 2)
				{
					
				}
				else
				{
					
				}
			} 
			else {
			}

		} else {
			int age = 2017 - userBirthYear;
			if (BMI < 18.5) {
			} 
			else if (18.5 <= BMI && BMI <= 24.9) {
			} 
			else {
			}

		}

	}	

	public void ConfirmPopUpInfoAlert() {
		AlertPanel.SetActive (false);
		BGTransparency.SetActive (false);
	}
		

	/* ===============================================================================================
	 * CRUD OPERATION WITH AZURE SERVICE
	 * ===============================================================================================
	*/


	public void Insert ()
	{
		Userdata userdata = GetUserData ();
//		if (Validate (score)) {
		StartCoroutine (GameManager.Instance._table.Insert<Userdata> (userdata, OnInsertCompleted));
//		}
	}

	private void OnInsertCompleted (IRestResponse<Userdata> response)
	{
		if (!response.IsError && response.StatusCode == HttpStatusCode.Created) {
			Debug.Log ("OnInsertItemCompleted: " + response.Content + " status code:" + response.StatusCode + " data:" + response.Data);
			Userdata item = response.Data; // if successful the item will have an 'id' property value
//			Debug.Log("JONG MER ====== "+item);
//			_score = item;
		} else {
			Debug.LogWarning ("Insert Error Status:" + response.StatusCode + " Url: " + response.Url);

			Debug.Log (response.StatusCode.ToString());
			if (response.StatusCode.ToString()  == "Conflict") {
				//TODO: Userdata is existed, do update operation instead
				Debug.Log ("DO UPDATE OPERATOIN");
			}
		}

		SceneManager.LoadScene ("Dashboard");
	}

	private Userdata GetUserData ()
	{
		Userdata userdata = new Userdata ();
		userdata.username = GameManager.Instance.profileName;
		userdata.birthDay = userBirthDay;
		userdata.birthMonth = userBirthMonth;
		userdata.birthYear = userBirthYear;
		userdata.gender = userGender;
		userdata.height = userHeightInput.text;
		userdata.weight = userWeightInput.text;
		
		return userdata;
	}

	/// <summary>
	/// Validate data before sending
	/// </summary>
//	private bool Validate (Userdata userdata)
//	{
//		bool isUsernameValid = true, isScoreValid = true;
//		// Validate username
//		if (String.IsNullOrEmpty (highscore.username)) {
//			isUsernameValid = false;
//			Debug.LogWarning ("Error, player username required");
//		}
//		// Validate score
//		if (!(highscore.score > 0)) {
//			isScoreValid = false;
//			Debug.LogWarning ("Error, player score should be greater than 0");
//		}
//		return (isUsernameValid && isScoreValid);
//	}



	/* ===============================================================================================
	 * SECTION FOR INITIALIZING THE DROP DOWN CANVAS UI FOR DATE, MONTH AND YEAR
	 * ===============================================================================================
	*/


	void InitializeBirthSelection() {
		for (int i = 1950; i <= 2015; i++) {
			string year = i + "";
			birthYear.Add (year);
		}
		userBirthYearDropDown.AddOptions (birthYear);

		//auto-fill user birth data if it has been set before
		if (PlayerPrefs.HasKey ("UserHeight")) {
			userHeightInput.text = PlayerPrefs.GetString ("UserHeight");
		}
		if (PlayerPrefs.HasKey ("UserWeight")) {
			userWeightInput.text = PlayerPrefs.GetString ("UserWeight");
		}
		if (PlayerPrefs.HasKey ("UserBirthDayIndex")) {
			userBirthDay = PlayerPrefs.GetInt("UserBirthDayIndex");
			userBirthDayDropDown.value = userBirthDay;
		}
		if (PlayerPrefs.HasKey ("UserBirthMonthIndex")) {
			userBirthMonth = PlayerPrefs.GetInt("UserBirthMonthIndex") - 1;
			userBirthMonthDropDown.value = userBirthMonth;
		}
		if (PlayerPrefs.HasKey ("UserBirthYearIndex")) {
			userBirthYear = PlayerPrefs.GetInt("UserBirthYearIndex");
			userBirthYearDropDown.value = userBirthYear;
		}

	}

	public void GenderOnChanged(int _genderIndex) {
		userGender = _genderIndex == 0 ? "Male" : "Female";
	}

	public void BirthDayOnChanged(int _dayIndex) {
		userBirthDayIndex = _dayIndex;
		userBirthDay = _dayIndex + 1;
//		Debug.Log (userBirthDay);
	}

	public void BirthMonthOnChanged(int _monthIndex) {
		userBirthMonthIndex = _monthIndex + 1;
		userBirthMonth = _monthIndex + 1;
//		Debug.Log (userBirthMonth);

		//TODO: Dynamically update the day for the selected month

		//logic to identify each days in a month
		if (userBirthMonthIndex == 1 || userBirthMonthIndex == 3 || userBirthMonthIndex == 5 || userBirthMonthIndex == 7 || userBirthMonthIndex == 8 || userBirthMonthIndex == 10 || userBirthMonthIndex == 12) {
			daysInMonth = 31;
		} else if (userBirthMonthIndex == 2){
			daysInMonth = 28;
		} else {
			daysInMonth = 30;
		}
		//start from 28th, in the Unity Editor has already set from 1st to 27th
		for(int i=28; i<=daysInMonth; i++) {
			string day = i + "";
			birthDay.Add (day);
		}
		userBirthDayDropDown.AddOptions (birthDay);
	}


	public void BirthYearOnChanged(int _yearIndex) {
		userBirthYearIndex = _yearIndex;
		userBirthYear = _yearIndex + 1950;
//		Debug.Log (userBirthYear);
	}

}
