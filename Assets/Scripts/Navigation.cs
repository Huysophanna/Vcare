using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour {
	private static string preScene;
	public float speed;

	// Use this for initialization
	void Start () {
		
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
}
