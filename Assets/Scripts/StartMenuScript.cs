using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using Unity3dAzure.AppServices;
using UnityEngine.UI;
using Tacticsoft;
using Prefabs;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Net;

public class StartMenuScript : MonoBehaviour {

	[SerializeField] private GameObject loadingScreen;

	private static StartMenuScript _instance;
	public static StartMenuScript Instance {
		get {
			return _instance;
		}
	}

	private static string preScene;
	public float speed;
	private string userAccessToken = "";
	private Message _message;
	private string AzureAppURL = "http://vcare.azurewebsites.net";

	// App Service Rest Client
//	private MobileServiceClient _client;

	// Use this for initialization
	void Start () {
//		// Create App Service client
//		_client = new MobileServiceClient (AzureAppURL);
	}

	void Awake() {
		_instance = this;
		GameManager.Instance.FBInit ();

	}

	// Update is called once per frame
	void Update () {

	}

	public void loadScence(string sceneName) {
		SceneManager.LoadScene (sceneName);
		preScene = sceneName;
		Debug.Log ("Loaded scene" + preScene);
	}

	public void backScene() {
		SceneManager.LoadScene (preScene);
		Debug.Log ("backScene " + preScene);
	}

	public void loadDashboard() {
		this.loadingScreen.SetActive (true);
		StartCoroutine (loadingScreenWithRealProgress());
	}

	public void FBlogin(GameObject loadingScreen) {
		List<string> permissions = new List<string>(){"public_profile", "email"};
		FB.LogInWithReadPermissions (permissions, OnFBAuthCompleted);
	}

	IEnumerator loadingScreenWithRealProgress() {
		yield return new WaitForSeconds (1);
		var ao = SceneManager.LoadSceneAsync ("Dashboard");
		ao.allowSceneActivation = false;

		if (!ao.isDone) {
			ao.allowSceneActivation = true;
		}
		yield return null;
	}

	void OnFBAuthCompleted (ILoginResult result) {
		if (result.Error != null) {
			Debug.Log (result.Error);
		} else {
			if (FB.IsLoggedIn) {
				userAccessToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
				PlayerPrefs.SetString ("FBAccessToken", userAccessToken);
		
				GameManager.Instance.isLoggedIn = true;
				GameManager.Instance.GetProfile ();
				PlayerPrefs.SetString ("IsAuthenticated", "true");

				Debug.Log ("FB user is logged in");

				StartCoroutine ("WaitForUserData");
			} else {
				Debug.Log ("FB user is not logged in");
			}
		}
			
	}

	IEnumerator WaitForUserData() {
		while (GameManager.Instance.profileName == null) {
			yield return null;
		}

		//identify whether the logged in user is a new user or existing user
		//new user will be sent to UserData scene
		GameManager.Instance.NewOrExistingUser (this.loadingScreen);
	}
}
