﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;


public class UserDataScript : MonoBehaviour {

	[SerializeField] private InputField userHeight;
	[SerializeField] private InputField userWeight;
	[SerializeField] private Dropdown userBirthYearDropDown;
	[SerializeField] private Dropdown userBirthDayDropDown;
	private string userBirthMonth = "";
	private int userBirthMonthIndex;
	private int daysInMonth;
	private JsonData Data;
	private JsonData Json;
	private JsonData List;
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
//		Debug.Log (userHeight.text);
//		Debug.Log (userWeight.text);
		StartCoroutine(APIcall());

	}

	public IEnumerator APIcall()
	{
		string URL = "https://api.nutritionix.com/v1_1/search";

		string jsonData = "";
		jsonData = "{\"appId\":\"56f421e1\",\"appKey\":\"24b1553f1e6a835b8e66c3cbc62a7820\",\"queries\":{\"item_name\":\"Kids Fries\",\"brand_name\":\"MacDonalds\"}}";

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
			// Show results as text
//			Debug.Log("Returning:" + request.downloadHandler.text);
//			Data = JsonMapper.ToObject(request.downloadHandler.);
//			Debug.Log(Data);
//			Json =  JsonUtility.ToJson(request.downloadHandler.text);
			Data = JsonMapper.ToObject(request.downloadHandler.text);
		    
			foreach (JsonData record in Data["hits"]) {
				Debug.Log(record.ToString());
			}

//			for (int i = 0; i < Data["hits"].Count; i++) {
//				Debug.Log (Data["hits"][i]["fields"]["brand_name"]);
//			}

//			Debug.Log (List);
//						JSONObject j = new JSONObject(request.downloadHandler.text);

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
