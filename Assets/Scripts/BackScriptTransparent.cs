using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackScriptTransparent : MonoBehaviour {

	// Use this for initialization

	void OnMouseDown(){
		gameObject.SetActive (false);
		Debug.Log (gameObject.name.ToString());
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
