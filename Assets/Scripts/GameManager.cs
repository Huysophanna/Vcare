using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;

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

	void Awake() {
		_instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

	void Start() {
		SceneManager.LoadScene ("Dashboard");
		//if (PlayerPrefs.HasKey ("IsAuthenticated")) {
			//SceneManager.LoadScene ("Dashboard");
			//profileName = PlayerPrefs.GetString ("ProfileName");
		//} else {
			//SceneManager.LoadScene ("StartMenu");
		//}
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
