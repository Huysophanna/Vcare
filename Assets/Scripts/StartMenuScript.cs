using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour {

	private static string preScene;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}

	void Awake() {
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

	public void FBlogin() {
		List<string> permissions = new List<string> ();
		permissions.Add ("public_profile");

		FB.LogInWithReadPermissions (permissions, AuthCallBack);
	}

	void AuthCallBack (IResult result) {
		if (result.Error != null) {
			Debug.Log (result.Error);
		} else {
			if (FB.IsLoggedIn) {
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
		GameManager.Instance.NewOrExistingUser ();

	}
}
