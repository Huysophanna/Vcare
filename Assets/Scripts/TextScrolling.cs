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

	// Use this for initialization
	void Start () {
		Stringinput = new string[10];
		First_Time = PlayerPrefs.GetInt("First_Time");
		BMI_Score = PlayerPrefs.GetFloat("BMI_Score");
		Calories_In_Take = PlayerPrefs.GetInt("Calories_In_Take");
		Information();
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
			Stringinput.SetValue(Info,0);
			PlayerPrefs.SetInt("First_Time",0);
			StartCoroutine (TextAnimated());
		}
		else
		{
			Stringinput.SetValue("Hello!", 0);
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
		if (Input.GetMouseButtonDown(0)) {
			if (characterindex < Stringinput [stringindex].Length) {
				characterindex = Stringinput [stringindex].Length;
			}else if(stringindex < Stringinput.Length-1){
				stringindex++;
				characterindex = 0;
			}
		}
	}
}
