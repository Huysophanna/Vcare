﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginNavigation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoginbackScene() {
		Debug.Log ("Calling success!");
		Navigation nv = new Navigation ();
		nv.backScene ();
	}
}
