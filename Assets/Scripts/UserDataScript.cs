using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UserDataScript : MonoBehaviour {

	[SerializeField] private InputField userHeight;
	[SerializeField] private InputField userWeight;
	[SerializeField] private Dropdown userBirthYearDropDown;
	[SerializeField] private Dropdown userBirthDayDropDown;
	private string userBirthMonth = "";
	private int userBirthMonthIndex;
	private int daysInMonth;

	List<string> birthYear = new List<string>();
	List<string> birthDay = new List<string>();

	void Start() {
		InitializeBirthSelection ();
		//make sure that the input field is not empty, drag and drop in the editor
		Assert.IsNotNull (userHeight);
		Assert.IsNotNull (userWeight);
		Assert.IsNotNull (userBirthYearDropDown);
		Assert.IsNotNull (userBirthDayDropDown);
	}

	void Update() {
		
	}

	public void SaveData() {
		// TODO: Save data into storage
		Debug.Log (userHeight.text);
		Debug.Log (userWeight.text);
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

	}

	public void BirthMonthOnChanged(int monthIndex) {
		userBirthMonthIndex = monthIndex + 1;
		userBirthMonth = monthIndex + 1 + "";
		Debug.Log (userBirthMonth);
		Debug.Log (userBirthMonthIndex + " index");


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

	

}
