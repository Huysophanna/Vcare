using System.Collections;
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
			return _instance;
		}
	}

	private string userAccessToken = "";

	// App Service Table defined using a DataModel
	public MobileServiceTable<Userdata> _table;
	// App Service Rest Client
	public MobileServiceClient _client;
	private string AzureAppURL = "http://vcare.azurewebsites.net";
	public string AzureAuthorizedID = "";

	void Awake() {
		_instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

	void Start() {

		// Create App Service client
		_client = new MobileServiceClient (AzureAppURL);

		// Get App Service 'Highscores' table
		_table = _client.GetTable<Userdata> ("Userdata");


		if (PlayerPrefs.HasKey ("IsAuthenticated") && PlayerPrefs.HasKey ("ExistingUser")) {
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

//	private string authenticateOption = "";
	public void AuthenticateAzureService() {
		StartCoroutine ("WaitForAccessToken");
//		authenticateOption = _option;
	}

	void AuthenticateAzureServiceStart() {
		StartCoroutine (_client.Login (MobileServiceAuthenticationProvider.Facebook, userAccessToken, OnAzureLoginCompleted));
	}

	void OnAzureLoginCompleted (IRestResponse<MobileServiceUser> response)
	{
		Debug.Log ("OnLoginCompleted: " + response.Content + " Url:" + response.Url);

		if (!response.IsError && response.StatusCode == HttpStatusCode.OK) {
			MobileServiceUser mobileServiceUser = response.Data;
			GameManager.Instance._client.User = mobileServiceUser;
			Debug.Log ("Authorized UserId: " + GameManager.Instance._client.User.user.userId);

			AzureAuthorizedID = GameManager.Instance._client.User.user.userId;
			Debug.Log (AzureAuthorizedID);
		} else {
			Debug.LogWarning ("Authorization Error: " + response.StatusCode);
		}
	}

	IEnumerator WaitForAccessToken() {
		userAccessToken = PlayerPrefs.GetString ("FBAccessToken");
		while (userAccessToken == null) {
			yield return null;
		}
		AuthenticateAzureServiceStart ();

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
