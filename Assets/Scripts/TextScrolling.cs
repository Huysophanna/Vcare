using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScrolling : MonoBehaviour {

	public string[] Stringinput;
	public float characterdelay = 0.1f;
	public Text txt;
	private int First_Time,Calories_In_Take;
	private float BMI_Score;
	private string BMI_Status,Info;
	int characterindex=0;
	int stringindex=0;
	private string BLDMealSelection;
	private string BrfTodaySelection = "";
	private string BrfTodaySelectionCalories;
	private string LunchTodaySelection = "";
	private string LunchTodaySelectionCalories = "";
	private string DinnerTodaySelection = "";
	private string DinnerTodaySelectionCalories = "";


	private List<string> SelectedTodayItem = new List<string>();
	private List<string> SelectedTodayItemCalories = new List<string>();
	private List<string> LunchTodayItem = new List<string>();
	private List<string> LunchTodayItemCalories = new List<string>();
	private List<string> DinnerTodayItem = new List<string>();
	private List<string> DinnerTodayCalories = new List<string>();


	// Use this for initialization
	void Start () {
		BLDMealSelection = PlayerPrefs.GetString ("BLDTodayMeal");
		Stringinput = new string[10];
		First_Time = PlayerPrefs.GetInt("First_Time");
		BMI_Score = PlayerPrefs.GetFloat("BMI_Score");
		Calories_In_Take = PlayerPrefs.GetInt("Calories_In_Take");
		Information();



		Debug.Log (BLDMealSelection + " " + PlayerPrefs.GetString (BLDMealSelection + "Today") + PlayerPrefs.GetString (BLDMealSelection + "TodayCalories"));
	}

	public void Information()
	{
		if(First_Time == 1)
		{
			if (BMI_Score <= 18.5)
				BMI_Status = "underweight";
			else if (18.5 <= BMI_Score && BMI_Score <= 24.9)
				BMI_Status = "normal";
			else
				BMI_Status = "overweight";
			Info = "OK! You BMI Score is " + BMI_Score + ". So, you are " + BMI_Status + ". The daily calories in take for you is approximately " + Calories_In_Take + " kcal.";

			PlayerPrefs.SetString ("BMIStatus", BMI_Status);

			Stringinput.SetValue(Info,0);
			PlayerPrefs.SetInt("First_Time",0);
			StartCoroutine (TextAnimated());
		}
		else
		{
			Stringinput.SetValue("Hello! Please get back everytime and select every meal you have", 0);
			StartCoroutine (TextAnimated());
		}
	}

	IEnumerator TextAnimated(){
		while (true) {
			yield return new WaitForSeconds (characterdelay);
			if (characterindex > Stringinput [stringindex].Length) {
				continue;
			}
			txt.text = Stringinput [stringindex].Substring (0, characterindex);
			characterindex++;
		}
	}


	// Update is called once per frame
	void Update () {
		BrfTodaySelection = PlayerPrefs.GetString("breakfastToday");
		LunchTodaySelection = PlayerPrefs.GetString("lunchToday");
		DinnerTodaySelection = PlayerPrefs.GetString("dinnerToday");


		if (Input.GetMouseButtonDown(0)) {
			if (characterindex < Stringinput [stringindex].Length) {
				characterindex = Stringinput [stringindex].Length;
			} else if(stringindex < Stringinput.Length-1){
				stringindex++;
				characterindex = 0;
			}
		}
	}
}
