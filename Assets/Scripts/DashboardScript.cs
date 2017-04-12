using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class DashboardScript : MonoBehaviour {

	private static DashboardScript _instance;
	public static DashboardScript Instance {
		get {
			return _instance;
		}
	}

	void Awake() {
		_instance = this;
	}

	[SerializeField] private GameObject settingPanel;
	[SerializeField] private GameObject BGTransparency;
	[SerializeField] private Text displayName;
	[SerializeField] private GameObject CatagorySelection;
	[SerializeField] private GameObject MealContent;
	[SerializeField] private GameObject SnackContent;
	[SerializeField] private Image healthBar;
	[SerializeField] private Image BMIBar;
	[SerializeField] private Image caloriesIntakeBar;
	[SerializeField] private Text BMITitleText;
	[SerializeField] private Text BMIPercentageText;


	public Text healthProgressPercentage;
	public Text caloriesIntakeProgressPercentage;
	private float healthProgressValue;
	private float BMIProgressValue;
	private float caloriesIntakeProgressValue;
	private string BMI_Status = "";
	private int CaloriesInTake;
	private float caloriesPlayerIsHaving;
	private string[] BrfTodayInput;


	private bool musicOption = true;
	private bool soundOption = true;
	private float BMIValue;
	private string breakfastToday = "";
	private string lunchToday = "";
	private string dinnerToday = "";
	private string snackToday = "";
	private string breakfastTodayCalories = "";
	private string lunchTodayCalories = "";
	private string dinnerTodayCalories = "";
	private string snackTodayCalories = "";
	private string BLDMealSelection = "";

	private int breakfastTodayCaloriesInt;
	private int lunchTodayCaloriesInt;
	private int dinnerTodayCaloriesInt;
	private int snackTodayCaloriesInt;

	private int breakfastTodayCaloriesIntTotal;
	private int lunchTodayCaloriesIntTotal;
	private int dinnerTodayCaloriesIntTotal;
	private int snackTodayCaloriesIntTotal;

	// Use this for initialization

	void OnMouseDrag() {
		Debug.Log ("DRAGGING");
	}

	void Start () {

		// TODO: To calculate and add calories based on player's food
		caloriesPlayerIsHaving = 200;

		//make sure to drag and drop the menu panel gameobject in the Editor
		Assert.IsNotNull (settingPanel);
		Assert.IsNotNull (BGTransparency);
		Assert.IsNotNull (displayName);
		Assert.IsNotNull (CatagorySelection);
		Assert.IsNotNull (MealContent);

		displayName.text = GameManager.Instance.profileName;
		Debug.Log (GameManager.Instance.profileName);

		//get BMI value
		BMIValue = PlayerPrefs.GetFloat("BMI_Score");

		//Reauthenticate for auto login user
		if (PlayerPrefs.HasKey ("ExistingUser")) {
			GameManager.Instance.AuthenticateAzureService ();
		}

//		Debug.Log (PlayerPrefs.HasKey ("ExistingUser"));

		//set user to be an existing user
		PlayerPrefs.SetString ("ExistingUser", "true");

	}
	
	// Update is called once per frame
	void Update () {
		
		GetUserBLDMealToday ();

		UpdateHealthProgressBar ();
		UpdateBMIProgressBar ();
		UpdateCaloriesIntakeProgressBar ();
	}

	private void UpdateHealthProgressBar() {
//		Debug.Log ((float)healthBar.fillAmount);
		healthProgressValue = 0.8f;
		if (healthProgressValue <= 1) {

			healthProgressValue = (float)Math.Round (healthProgressValue, 2, MidpointRounding.AwayFromZero );
			healthBar.fillAmount = healthProgressValue;
			healthProgressPercentage.text = ((float)healthBar.fillAmount) * 100 + "%";
		} else
			return;
	}

	private void GetUserBLDMealToday() {
		
		BLDMealSelection = PlayerPrefs.GetString ("BLDTodayMeal");
		breakfastToday = PlayerPrefs.GetString ("breakfastToday");
		lunchToday = PlayerPrefs.GetString ("lunchToday");
		dinnerToday = PlayerPrefs.GetString ("dinnerToday");
		breakfastTodayCalories = PlayerPrefs.GetString ("breakfastTodayCalories");
		lunchTodayCalories = PlayerPrefs.GetString ("lunchTodayCalories");
		dinnerTodayCalories = PlayerPrefs.GetString ("dinnerTodayCalories");
		snackTodayCalories = PlayerPrefs.GetString ("snackTodayCalories");

		if (PlayerPrefs.GetString ("breakfastTodayCalories") == null) {
			breakfastTodayCaloriesInt = int.Parse(PlayerPrefs.GetString ("breakfastTodayCalories"));
			lunchTodayCaloriesInt = int.Parse(PlayerPrefs.GetString ("lunchTodayCalories"));
			dinnerTodayCaloriesInt = int.Parse(PlayerPrefs.GetString ("dinnerTodayCalories"));
			snackTodayCaloriesInt = int.Parse(PlayerPrefs.GetString ("snackTodayCalories"));
		}

		Debug.Log(breakfastTodayCalories);
		Debug.Log(lunchTodayCalories);
		Debug.Log(dinnerTodayCalories);
		Debug.Log(snackTodayCalories);
	}

	private void UpdateCaloriesIntakeProgressBar() {
		CaloriesInTake = PlayerPrefs.GetInt ("Calories_In_Take");
		Debug.Log (CaloriesInTake);
		caloriesIntakeProgressValue = caloriesPlayerIsHaving / (float)CaloriesInTake;
		Debug.Log (caloriesPlayerIsHaving);
		Debug.Log (caloriesIntakeProgressValue);

		caloriesIntakeProgressValue = (float)Math.Round (caloriesIntakeProgressValue, 2, MidpointRounding.AwayFromZero );
		caloriesIntakeBar.fillAmount = caloriesIntakeProgressValue;
		caloriesIntakeProgressPercentage.text = ((float)caloriesIntakeBar.fillAmount) * 100 + "%";
	}

	public void AddBrfCalories(string _item, string _calories) {
		
		breakfastTodayCaloriesIntTotal += int.Parse(_calories);
		PlayerPrefs.SetString (BLDMealSelection + "Today", _item);
		PlayerPrefs.SetString (BLDMealSelection + "TodayCalories", _calories);

		Debug.Log (breakfastTodayCaloriesIntTotal);
	}

	public void AddLucnchCalories(string _item, string _calories) {
		Instance.lunchTodayCaloriesIntTotal += int.Parse(_calories);
		PlayerPrefs.SetString (BLDMealSelection + "Today", _item);
		PlayerPrefs.SetString (BLDMealSelection + "TodayCalories", _calories);
		Debug.Log (Instance.lunchTodayCaloriesIntTotal);
	}

	public void AddDinnerCalories(string _item, string _calories) {
		dinnerTodayCaloriesIntTotal += int.Parse(_calories);
		PlayerPrefs.SetString (BLDMealSelection + "Today", _item);
		PlayerPrefs.SetString (BLDMealSelection + "TodayCalories", _calories);
		Debug.Log (dinnerTodayCaloriesIntTotal);
	}

	public void AddSnackCalories(string _item, string _calories) {
		snackTodayCaloriesIntTotal += int.Parse(_calories);
		PlayerPrefs.SetString (BLDMealSelection + "Today", _item);
		PlayerPrefs.SetString (BLDMealSelection + "TodayCalories", _calories);
		Debug.Log (snackTodayCaloriesIntTotal);
	}

	private void UpdateBMIProgressBar() {
		//		Debug.Log ((float)healthBar.fillAmount);

		BMI_Status = PlayerPrefs.GetString ("BMIStatus");;
		//0.1 means underweight
		if (BMI_Status == "underweight") {
			BMIProgressValue = 0.1f;
			BMITitleText.text = "BM I: Underweight";
			BMIPercentageText.text = BMIValue + "";


			//0.2 means normal
		} else if (BMI_Status == "normal") {
			BMIProgressValue = 0.2f;
			BMITitleText.text = "BM I: Normal";
			BMIPercentageText.text = BMIValue + "";

			//0.3 means overweight
		} else if (BMI_Status == "overweight") {
			BMIProgressValue = 0.3f;
			BMITitleText.text = "BM I: Overweight";
			BMIPercentageText.text = BMIValue + "";

		}
			
		BMIProgressValue = (float)Math.Round (BMIProgressValue, 2, MidpointRounding.AwayFromZero );
		BMIBar.fillAmount = BMIProgressValue * 3;



	}

	public void ReportDataIsClicked() {
		SceneManager.LoadScene ("ReportData");
	}

	public void SettingIsClicked() {
		settingPanel.SetActive (true);
		BGTransparency.SetActive (true);
	}

	public void ClosePanel() {
		settingPanel.SetActive (false);
		BGTransparency.SetActive (false);
	}

	public void MusicOptToggle(bool val) {
		musicOption = val;
		Debug.Log ("Music Option: " + musicOption);
	}

	public void SoundOptToggle(bool val) {
		soundOption = val;
		Debug.Log ("Sound Option: " + soundOption);
	}

	public void SideMenuIsClicked() {
		SceneManager.LoadScene ("UserData");

	}

	public void LogOut() {
		PlayerPrefs.DeleteKey ("IsAuthenticated");
		SceneManager.LoadScene ("StartMenu");
		PlayerPrefs.DeleteAll ();

		GameManager.Instance.FBLogOut ();

	}

	public void MealButtonIsClicked() {
		CatagorySelection.SetActive (false);
		MealContent.SetActive (true);
	}

	public void SnackButtonIsClicked() {
		CatagorySelection.SetActive (false);
		SnackContent.SetActive (true);
	}

	public void EnterTodayMeal(string _buttonName) {
		//store to know what type of meal they click to select, brf lunch or dinner
		PlayerPrefs.SetString ("BLDTodayMeal", _buttonName);

		SceneManager.LoadScene ("MealSelection");
//		Debug.Log (_buttonName);
	}


}