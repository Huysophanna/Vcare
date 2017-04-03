using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity3dAzure.AppServices;
using System.Net;
using System;

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
	private int userGenderIndex;
	private string userGender = "";
	private string AzureAuthorizedID;


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


		//initialize popup alert text with username for NEW user
		popUpInfoText.text = "Hi there " + GameManager.Instance.profileName + ", \n\nWe'd like to know more about you, to assist you in a best way .";

		//if it is a NEW user, show alert info panel
		if (!PlayerPrefs.HasKey ("ExistingUser")) {
			AlertPanel.SetActive (true);
			BGTransparency.SetActive (true);
		}

	}

	public void SaveData() {
		Insert ();

		//initialized value if the default dropdown value is selected
		GenderOnChanged (userGenderDropDown.value);
		BirthDayOnChanged (userBirthDayDropDown.value);
		BirthMonthOnChanged (userBirthMonthDropDown.value);
		BirthYearOnChanged (userBirthYearDropDown.value);

		// TODO: Save data into storage
		PlayerPrefs.SetString("UserHeight", userHeightInput.text);
		PlayerPrefs.SetString("UserWeight", userWeightInput.text);
		PlayerPrefs.SetInt("UserGenderIndex", userGenderIndex);
		PlayerPrefs.SetInt("UserBirthDayIndex", userBirthDayIndex);
		PlayerPrefs.SetInt("UserBirthMonthIndex", userBirthMonthIndex);
		PlayerPrefs.SetInt("UserBirthYearIndex", userBirthYearIndex);
	
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
		Userdata userdata = PrepareUserData ();
		if (Validate (userdata)) {
			StartCoroutine (GameManager.Instance._table.Insert<Userdata> (userdata, OnInsertCompleted));
		}
	}

	private void OnInsertCompleted (IRestResponse<Userdata> response)
	{
		if (!response.IsError && response.StatusCode == HttpStatusCode.Created) {
			Debug.Log ("OnInsertItemCompleted: " + response.Content + " status code:" + response.StatusCode + " data:" + response.Data);
			Userdata item = response.Data; // if successful the item will have an 'id' property value
//			Debug.Log("JONG MER ====== "+item);
//			_score = item;

			SceneManager.LoadScene ("Dashboard");
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
		Userdata userdata = PrepareUserData ();
		if (Validate (userdata)) {
			StartCoroutine (GameManager.Instance._table.Update<Userdata> (userdata, OnUpdateScoreCompleted));
		}
	}

	private void OnUpdateScoreCompleted (IRestResponse<Userdata> response)
	{
		if (!response.IsError) {
			Debug.Log ("OnUpdateItemCompleted: " + response.Content);
			SceneManager.LoadScene ("Dashboard");

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
			Userdata[] items = response.Data;
//			_isPaginated = false; // default query has max. of 50 records and is not paginated so disable infinite scroll 
//			_scores = items.ToList ();
//			HasNewData = true;
			Debug.Log(response.Data.ToString());
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
		Debug.Log (userGender);
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
