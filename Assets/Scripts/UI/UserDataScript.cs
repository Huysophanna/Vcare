using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserDataScript : MonoBehaviour {
	[SerializeField] private InputField userHeight;
	[SerializeField] private InputField userWeight;
	[SerializeField] private Dropdown userBirthYearDropDown;
	[SerializeField] private Dropdown userBirthMonthDropDown;
	[SerializeField] private Dropdown userBirthDayDropDown;
	[SerializeField] private Text popUpInfoText;
	[SerializeField] private GameObject AlertPanel;
	[SerializeField] private GameObject BGTransparency;

	private string userBirthMonth = "";
	private int userBirthMonthIndex;
	private int daysInMonth;
	private int userBirthYear;
	private int userBirthYearIndex;
	private int userBirthDay;
	private int userBirthDayIndex;


	List<string> birthYear = new List<string>();
	List<string> birthDay = new List<string>();

	void Start() {
		InitializeBirthSelection ();

		//make sure that the input field is not empty, drag and drop in the editor
		Assert.IsNotNull (userHeight);
		Assert.IsNotNull (userWeight);
		Assert.IsNotNull (userBirthYearDropDown);
		Assert.IsNotNull (userBirthDayDropDown);
		Assert.IsNotNull (popUpInfoText);


		//initialize popup alert text with username for NEW user
		popUpInfoText.text = "Hi there " + GameManager.Instance.profileName + ", \n\nWe'd like to know more about you, to assist you in a best way .";

		//if it is a NEW user, show alert info panel
		if (!PlayerPrefs.HasKey ("ExistingUser")) {
			AlertPanel.SetActive (true);
			BGTransparency.SetActive (true);
		}


	}

	public void SaveData() {
		// TODO: Save data into storage
		PlayerPrefs.SetString("UserHeight", userHeight.text);
		PlayerPrefs.SetString("UserWeight", userWeight.text);
		PlayerPrefs.SetInt("UserBirthDayIndex", userBirthDayIndex);
		PlayerPrefs.SetInt("UserBirthMonthIndex", userBirthMonthIndex);
		PlayerPrefs.SetInt("UserBirthYearIndex", userBirthYearIndex);

		SceneManager.LoadScene ("Dashboard");
	}

	public void ConfirmPopUpInfoAlert() {
		AlertPanel.SetActive (false);
		BGTransparency.SetActive (false);
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
		if (PlayerPrefs.HasKey ("UserHeight")) {
			userHeight.text = PlayerPrefs.GetString ("UserHeight");
		}
		if (PlayerPrefs.HasKey ("UserWeight")) {
			userWeight.text = PlayerPrefs.GetString ("UserWeight");
		}
		if (PlayerPrefs.HasKey ("UserBirthDayIndex")) {
			userBirthDayDropDown.value = PlayerPrefs.GetInt("UserBirthDayIndex");
		}
		if (PlayerPrefs.HasKey ("UserBirthMonthIndex")) {
			userBirthMonthDropDown.value = PlayerPrefs.GetInt("UserBirthMonthIndex") - 1;
		}
		if (PlayerPrefs.HasKey ("UserBirthYearIndex")) {
			userBirthYearDropDown.value = PlayerPrefs.GetInt("UserBirthYearIndex");
		}

	}

	public void BirthDayOnChanged(int _dayIndex) {
		userBirthDayIndex = _dayIndex;
		userBirthDay = _dayIndex + 1;
	}

	public void BirthMonthOnChanged(int _monthIndex) {
		userBirthMonthIndex = _monthIndex + 1;
		userBirthMonth = _monthIndex + 1 + "";

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
	}

}
