using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReportDataScript : MonoBehaviour {
	[SerializeField] private Text displayName;
	[SerializeField] private Text BrfCaloriesIntakeValue;
	[SerializeField] private Text LunchCaloriesIntakeValue;
	[SerializeField] private Text DinnerCaloriesIntakeValue;
	[SerializeField] private Text SnackCaloriesIntakeValue;

	private string breakfastTodayCalories = "";
	private string lunchTodayCalories = "";
	private string dinnerTodayCalories = "";
	private string snackTodayCalories = "";

	// Use this for initialization
	void Start () {
		displayName.text = GameManager.Instance.profileName;
		GetMealData ();

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("asdfasdfsdf" + breakfastTodayCalories == "");
		Debug.Log (PlayerPrefs.GetString ("breakfastTodayCalories") == "");
	}

	private void GetMealData() {
		breakfastTodayCalories = PlayerPrefs.GetString ("breakfastTodayCalories");
		lunchTodayCalories = PlayerPrefs.GetString ("lunchTodayCalories");
		dinnerTodayCalories = PlayerPrefs.GetString ("dinnerTodayCalories");
		snackTodayCalories = PlayerPrefs.GetString ("snackTodayCalories");


		BrfCaloriesIntakeValue.text = breakfastTodayCalories;
		LunchCaloriesIntakeValue.text = lunchTodayCalories;
		DinnerCaloriesIntakeValue.text = dinnerTodayCalories;
		SnackCaloriesIntakeValue.text = snackTodayCalories;

		if (breakfastTodayCalories == "") {
			BrfCaloriesIntakeValue.text = "0";
			Debug.Log ("1");
		}
		if (lunchTodayCalories == "") {
			LunchCaloriesIntakeValue.text = "0";
			Debug.Log ("2");
		}
		if (dinnerTodayCalories == "") {
			DinnerCaloriesIntakeValue.text = "0";
			Debug.Log ("3");
		}
		if (snackTodayCalories == "") {
			SnackCaloriesIntakeValue.text = "0";
			Debug.Log ("4");
		}



	}

	public void BackBtnIsClicked() {
		SceneManager.LoadScene ("Dashboard");
	}
}
