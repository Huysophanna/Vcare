﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Unity3dAzure.AppServices;
using UnityEngine.SceneManagement;
using System.Net;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;
	public static GameManager Instance {
		get {
			if (_instance == null) {
				GameObject FBManager = new GameObject ("FBManager");
				FBManager.AddComponent<GameManager> ();
			}

			return _instance;
		}
	}

	private string userAccessToken = "";

	// App Service Table defined using a DataModel
	public MobileServiceTable<Userdata> _table;
	// App Service Rest Client
	public MobileServiceClient _client;
	private string AzureAppURL = "http://vcare.azurewebsites.net";

	void Awake() {
		_instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

	void Start() {
		userAccessToken = PlayerPrefs.GetString ("FBAccessToken");

		// Create App Service client
		_client = new MobileServiceClient (AzureAppURL);

		// Get App Service 'Highscores' table
		_table = _client.GetTable<Userdata> ("Userdata");

		if (PlayerPrefs.HasKey ("IsAuthenticated")) {
			SceneManager.LoadScene ("Dashboard");
			profileName = PlayerPrefs.GetString ("ProfileName");


		} else {
			SceneManager.LoadScene ("StartMenu");
		}
	}

	public void NewOrExistingUser() {
		//identify whether the user is new user
		if (!PlayerPrefs.HasKey ("ExistingUser")) {
			SceneManager.LoadScene ("UserData");
		} else {
			Debug.Log (PlayerPrefs.HasKey ("ExistingUser"));
			SceneManager.LoadScene ("Dashboard");
		}
	}

	private string authenticateOption = "";
	public void AuthenticateAzureService(string _option) {
		StartCoroutine (_client.Login (MobileServiceAuthenticationProvider.Facebook, userAccessToken, OnAzureLoginCompleted));
		authenticateOption = _option;
	}

	void OnAzureLoginCompleted (IRestResponse<MobileServiceUser> response)
	{
		Debug.Log ("OnLoginCompleted: " + response.Content + " Url:" + response.Url);

		if (!response.IsError && response.StatusCode == HttpStatusCode.OK) {
			MobileServiceUser mobileServiceUser = response.Data;
			GameManager.Instance._client.User = mobileServiceUser;
			Debug.Log ("Authorized UserId: " + GameManager.Instance._client.User.user.userId);

			if (authenticateOption == "FirstAuthenticate") {
				//identify whether the logged in user is a new user or existing user
				//new user will be sent to UserData scene
				GameManager.Instance.NewOrExistingUser ();
			}

		} else {
			Debug.LogWarning ("Authorization Error: " + response.StatusCode);
		}
	}


	/* ===============================================================================================
	 * SECTION FOR INITIALIZING FACEBOOK LOGIN
	 * ===============================================================================================
	*/


	public bool isLoggedIn { get; set;}
	public string profileName { get; set;}

	public void FBInit() {
		if (!FB.IsInitialized) {
			FB.Init (SetInit, OnHideUnity);
		} else {
			isLoggedIn = FB.IsLoggedIn;
		}
	}

	void SetInit() {
		if (FB.IsLoggedIn) {
			Debug.Log ("FB user is logged in");
			GetProfile ();
		} else {
			Debug.Log ("FB user is not logged in");
		}
		isLoggedIn = FB.IsLoggedIn;
	}

	void OnHideUnity(bool isGameShown) {
		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	public void GetProfile() {
		FB.API ("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
	}

	void DisplayUsername(IResult result) {

		if (result.Error == null) {
			profileName = result.ResultDictionary ["first_name"] + "";
			PlayerPrefs.SetString ("ProfileName", profileName);
		} else {
			Debug.Log (result.Error);
		}
	}
}
