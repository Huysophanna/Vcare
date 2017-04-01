using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class FBScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Awake() {
		
		GameManager.Instance.FBInit ();
	}


		
}