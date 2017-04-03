using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackScriptTransparent : MonoBehaviour {

	[SerializeField] private GameObject Detailbubble;
	// Use this for initialization

	void OnMouseDown(){
		Detailbubble.GetComponent<DetailBubbleManager> ().ClosePanel();
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
