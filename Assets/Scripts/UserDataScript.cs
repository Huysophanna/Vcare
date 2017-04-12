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
	[SerializeField] private GameObject AlwaysBtn;
	[SerializeField] private GameObject OftenBtn;
	[SerializeField] private GameObject NeverBtn;
	[SerializeField] private GameObject CloseBtn;
	[SerializeField] private GameObject LoadingPanel;



	private int userBirthMonth;
	private int userBirthMonthIndex;
	private int daysInMonth;
	private int userBirthYear;
	private int userBirthYearIndex;
	private int userBirthDay;
	private int userBirthDayIndex;
	private int userGenderIndex;
	private string userGender = "";

	private JsonData Json;
	private string[] item_names = new string[50];
	private string[] calories = new string[50];
	private string[] total = new string[50];
	private double BMI;
	private string AzureAuthorizedID;
	private int exerciseFrequency;

	Userdata[] AzureUserData;
	Userdata userdata;


	List<string> birthYear = new List<string>();
	List<string> birthDay = new List<string>();

	private static UserDataScript _instance;
	public static UserDataScript Instance {
		get {
			if (_instance == null) {
				GameObject UserData = new GameObject ("UserData");
				UserData.AddComponent<UserDataScript> ();
			}
			return _instance;
		}
	}
	void Awake() {
		_instance = this;
	}

	void Update() {
		//		UserDataScript.Instance.GetAzureUserData ();
	}

	void Start() {

		//Login with azure service
		GameManager.Instance.AuthenticateAzureService();
		StartCoroutine ("WaitForAccessToken");

		InitializeBirthSelection ();

		//make sure that the input field is not empty, drag and drop in the editor
		Assert.IsNotNull (userHeightInput);
		Assert.IsNotNull (userWeightInput);
		Assert.IsNotNull (userBirthYearDropDown);
		Assert.IsNotNull (userBirthDayDropDown);
		Assert.IsNotNull (popUpInfoText);
		Assert.IsNotNull (userGenderDropDown);
		Assert.IsNotNull (AlwaysBtn);
		Assert.IsNotNull (OftenBtn);
		Assert.IsNotNull (NeverBtn);
		Assert.IsNotNull (CloseBtn);

		//initialize popup alert text with username for NEW user
		popUpInfoText.text = "Hi there " + GameManager.Instance.profileName + ", \n\nWe'd like to know more about you, to assist you in a best way .";

		//if it is a NEW user, show alert info panel
		if (!PlayerPrefs.HasKey ("ExistingUser")) {
			AlertPanel.SetActive (true);
			BGTransparency.SetActive (true);
		}

	}


	public void SaveData() {
		userdata = PrepareUserData ();
		if (Validate (userdata)) {
			// TODO: Alert, exercise frequency
			PopupExerciseFrequency ();

			//initialized value if the default dropdown value is selected
			GenderOnChanged (userGenderDropDown.value);
			BirthDayOnChanged (userBirthDayDropDown.value);
			BirthMonthOnChanged (userBirthMonthDropDown.value);
			BirthYearOnChanged (userBirthYearDropDown.value);

			//Save data into storage
			PlayerPrefs.SetString("UserHeight", userHeightInput.text);
			PlayerPrefs.SetString("UserWeight", userWeightInput.text);
			PlayerPrefs.SetInt("UserGenderIndex", userGenderIndex);
			PlayerPrefs.SetInt("UserBirthDayIndex", userBirthDayIndex);
			PlayerPrefs.SetInt("UserBirthMonthIndex", userBirthMonthIndex);
			PlayerPrefs.SetInt("UserBirthYearIndex", userBirthYearIndex);

		}


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
		CaloriesInTake (BMI); 
	}

	/* ===============================================================================================
	 * Calories In Take Recommendation
	 * ===============================================================================================
	*/

	public void CaloriesInTake(double BMI)
	{
		PlayerPrefs.SetFloat("BMI_Score",(float)BMI);

		PlayerPrefs.SetInt("First_Time",1);
		int isActive = exerciseFrequency;
		int Gender = userGender == "Male"? 1 : 2;
		if (Gender == 1) {
			PlayerPrefs.SetString("Gender","M");
			int age = 2017 - userBirthYear;
			if (BMI < 18.5) {
				PlayerPrefs.SetInt("Calories_In_Take", 2500);
			} 
			else if (18.5 <= BMI && BMI <= 24.9) {
				if (isActive == 3) {
					if (2 <= age && age <= 3) {
						PlayerPrefs.SetInt("Calories_In_Take", 1000);
					} 
					else if (4 <= age && age <= 8)
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1800);
					} 
					else if (9 <= age && age <= 13) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2300);
					} else if (14 <= age && age <= 18) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 3000);
					} else if (19 <= age && age <= 30) 
					{
					} else if (31 <= age && age <= 50) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2800);
					} 
					else 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2600);
					}
				}
				else if(isActive == 2)
				{
					if (2 <= age && age <= 3) {
						PlayerPrefs.SetInt("Calories_In_Take", 1000);
					} 
					else if (4 <= age && age <= 8) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1600);
					} 
					else if (9 <= age && age <= 13) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2000);
					} else if (14 <= age && age <= 18) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2600);
					} else if (19 <= age && age <= 30) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2700);
					} else if (31 <= age && age <= 50) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2400);
					} 
					else 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2200);
					}
				}
				else
				{
					if (2 <= age && age <= 3) {
						PlayerPrefs.SetInt("Calories_In_Take", 1000);
					} 
					else if (4 <= age && age <= 8) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1400);					} 
					else if (9 <= age && age <= 13) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1800);
					} else if (14 <= age && age <= 18) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2200);
					} else if (19 <= age && age <= 30) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2400);
					} else if (31 <= age && age <= 50) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2200);
					} 
					else 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2000);
					}
				}
			} 
			else {
				PlayerPrefs.SetInt("Calories_In_Take", 1400);
			}

		} else {
			PlayerPrefs.SetString("Gender","F");
			int age = 2017 - userBirthYear;
			if (BMI < 18.5) {
				PlayerPrefs.SetInt("Calories_In_Take", 2500);
			} 
			else if (18.5 <= BMI && BMI <= 24.9) {
				if (isActive == 3) {
					if (2 <= age && age <= 3) {
						PlayerPrefs.SetInt("Calories_In_Take", 1000);
					} 
					else if (4 <= age && age <= 8) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1600);
					} 
					else if (9 <= age && age <= 13) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2000);
					} else if (14 <= age && age <= 18) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2400);
					} else if (19 <= age && age <= 30) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2400);
					} else if (31 <= age && age <= 50) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2200);
					} 
					else 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2000);
					}
				}
				else if(isActive == 2)
				{
					if (2 <= age && age <= 3) {
						PlayerPrefs.SetInt("Calories_In_Take", 1000);
					} 
					else if (4 <= age && age <= 8) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1600);
					} 
					else if (9 <= age && age <= 13) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1800);
					} else if (14 <= age && age <= 18) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2000);
					} else if (19 <= age && age <= 30) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2200);
					} else if (31 <= age && age <= 50) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2000);
					} 
					else 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1800);
					}
				}
				else
				{
					if (2 <= age && age <= 3) {
						PlayerPrefs.SetInt("Calories_In_Take", 1000);
					} 
					else if (4 <= age && age <= 8) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1200);
					} 
					else if (9 <= age && age <= 13) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1600);
					} else if (14 <= age && age <= 18) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1800);
					} else if (19 <= age && age <= 30) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 2000);
					} else if (31 <= age && age <= 50) 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1800);
					} 
					else 
					{
						PlayerPrefs.SetInt("Calories_In_Take", 1600);
					}
				}
			} 
			else { 
				PlayerPrefs.SetInt("Calories_In_Take", 1300);
			}

		}
		//insert user data to Azure service
		Insert ();
	}	

	public void ConfirmPopUpInfoAlert() {
		AlertPanel.SetActive (false);
		BGTransparency.SetActive (false);
	}

	void PopupExerciseFrequency() {
		popUpInfoText.text = "How often you do exercise?";
		AlertPanel.SetActive (true);
		NeverBtn.SetActive (true);
		OftenBtn.SetActive (true);
		AlwaysBtn.SetActive (true);
		CloseBtn.SetActive (false);
	}

	public void ExerciseFrequencyAction(int _frequency) {
		//freqency: 1 never, 2 often, 3 always
		exerciseFrequency = _frequency;
		PlayerPrefs.SetInt ("ExerciseFrequency", exerciseFrequency);

		//
		BMICalulation (userHeightInput.text,userWeightInput.text);

		//insert user data to Azure service
		Insert ();
	}


	/* ===============================================================================================
	 * CRUD OPERATION WITH AZURE SERVICE
	 * ===============================================================================================
	*/


	public void Insert ()
	{
		Userdata userdata = PrepareUserData ();
		StartCoroutine (GameManager.Instance._table.Insert<Userdata> (userdata, OnInsertCompleted));
	}

	public void loadDashboard() {
		LoadingPanel.SetActive (true);
		StartCoroutine (loadingScreenWithRealProgress());
	}

	IEnumerator loadingScreenWithRealProgress() {
		yield return new WaitForSeconds (1);
		var ao = SceneManager.LoadSceneAsync ("Dashboard");
		ao.allowSceneActivation = false;

		if (!ao.isDone) {
			ao.allowSceneActivation = true;
		}
		yield return null;
	}

	private void OnInsertCompleted (IRestResponse<Userdata> response)
	{
		if (!response.IsError && response.StatusCode == HttpStatusCode.Created) {
			Debug.Log ("OnInsertItemCompleted: " + response.Content + " status code:" + response.StatusCode + " data:" + response.Data);
			Userdata item = response.Data; // if successful the item will have an 'id' property value
			//			Debug.Log("JONG MER ====== "+item);
			//			_score = item;


			loadDashboard ();

//			SceneManager.LoadScene ("Dashboard");
		} else {
			Debug.LogWarning ("Insert Error Status:" + response.StatusCode + " Url: " + response.Url);

			if (response.StatusCode.ToString()  == "Conflict") {
				//Userdata is existed, do update operation instead
				Debug.Log ("UPDATING DATA OPERATOIN");
				UpdateData ();
			}

			if (response.StatusCode.ToString()  == "0") {
				//TODO: Show alert, No connection
				AlertPanel.SetActive (true);
				BGTransparency.SetActive (true);
				popUpInfoText.text = "You don't seem to have an active internet connection which is mandatory for us to store your data. Please retry.";
			}

		}
	}

	private Userdata PrepareUserData ()
	{
		Userdata userdata = new Userdata ();
		userdata.username = GameManager.Instance.profileName;
		userdata.birthDay = userBirthDay - 1;
		userdata.birthMonth = userBirthMonth - 1;
		userdata.birthYear = userBirthYear - 1;
		userdata.gender = userGender;
		userdata.height = userHeightInput.text;
		userdata.weight = userWeightInput.text;

		return userdata;
	}

	public void UpdateData ()
	{
		//		Userdata userdata = PrepareUserData ();
		//		if (Validate (userdata)) {
		StartCoroutine (GameManager.Instance._table.Update<Userdata> (userdata, OnUpdateScoreCompleted));
		//		}
	}

	private void OnUpdateScoreCompleted (IRestResponse<Userdata> response)
	{
		if (!response.IsError) {
			Debug.Log ("OnUpdateItemCompleted: " + response.Content);

			loadDashboard ();
//			SceneManager.LoadScene ("Dashboard");
		} else {
			Debug.LogWarning ("Update Error Status:" + response.StatusCode + " " + response.ErrorMessage + " Url: " + response.Url);
			//TODO: Show alert, No connection
		}
	}

	public void GetAzureUserData ()
	{
		//		ResetList ();
		Userdata userdata = PrepareUserData ();
		string filter = string.Format ("username eq '{0}'", GameManager.Instance.profileName);
		//string orderBy = "score desc";
		CustomQuery query = new CustomQuery (filter);
		Query (query);
	}

	private void Query (CustomQuery query)
	{
		StartCoroutine (GameManager.Instance._table.Query<Userdata> (query, OnReadCompleted));
	}

	private void OnReadCompleted (IRestResponse<Userdata[]> response)
	{
		if (!response.IsError) {
			Debug.Log ("OnReadCompleted: " + response.Url + " data: " + response.Content);
			AzureUserData = response.Data;


			//Update data and Display in UI
			foreach (var data in AzureUserData) {
				userHeightInput.text = data.height;
				userWeightInput.text = data.weight;
				userGenderDropDown.value = data.gender == "Male" ? 1 : 2;
				userBirthDayDropDown.value = data.birthDay;
				userBirthMonthDropDown.value = data.birthMonth;
				userBirthYearDropDown.value = data.birthYear - 1949;
			}


		} else {
			Debug.LogWarning ("Read Error Status:" + response.StatusCode + " Url: " + response.Url);
		}
	}

	IEnumerator WaitForAccessToken() {
		//		AzureAuthorizedID = PlayerPrefs.GetString ("AzureAuthorizedID");
		while (GameManager.Instance.AzureAuthorizedID == "") {
			yield return null;
		}
		Debug.Log (GameManager.Instance.AzureAuthorizedID == "");
		Debug.Log (GameManager.Instance.AzureAuthorizedID.ToString()+" JOL");
		GetAzureUserData ();

	}



	/* ===============================================================================================
	 * SECTION FOR UI VALIDATION
	 * ===============================================================================================
	*/


	//	/ <summary>
	//	/ Validate data before sending
	//	/ </summary>
	private bool Validate (Userdata userdata)
	{
		bool isGenderValid = true, isBirthDayValid = true, isBirthMonthValid = true, isBirthYearValid = true, isHeightValid = true, isWeightValid = true;

		// Validate birth day
		if (userGenderDropDown.value == 0) {
			isGenderValid = false;
			Debug.LogWarning ("gender input error");
			ShowErrorText ("Gender");
		}

		if (userBirthDayDropDown.value == 0) {
			isBirthDayValid = false;
			Debug.LogWarning ("birth day input error");
			ShowErrorText ("Day");
		}

		if (userBirthMonthDropDown.value == 0) {
			isBirthMonthValid = false;
			Debug.LogWarning ("birth month input error");
			ShowErrorText ("Month");
		}

		if (userBirthYearDropDown.value == 0) {
			isBirthYearValid = false;
			Debug.LogWarning ("birth year input error");
			ShowErrorText ("Year");
		}

		// Validate height
		if (String.IsNullOrEmpty(userHeightInput.text)) {
			isHeightValid = false;
			Debug.LogWarning ("height input error");
			ShowErrorText ("Height");
		}

		// Validate height
		if (String.IsNullOrEmpty(userWeightInput.text)) {
			isWeightValid = false;
			Debug.LogWarning ("weight input error");
			ShowErrorText ("Weight");
		}
		return (isGenderValid && isBirthDayValid && isBirthMonthValid && isBirthYearValid && isHeightValid && isWeightValid);
	}

	private void ShowErrorText (string gameObjectName)
	{
		Text text = GameObject.Find (gameObjectName).GetComponent<Text> ();
		if (text) {
			text.color = Color.red;
		}
	}



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
		if (PlayerPrefs.HasKey ("UserGenderIndex")) {
			userGenderDropDown.value = PlayerPrefs.GetInt ("UserGenderIndex");
		}
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
		userGenderIndex = _genderIndex;
		userGender = _genderIndex == 1 ? "Male" : "Female";
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
		} else if (userBirthMonthIndex == 2) {
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
