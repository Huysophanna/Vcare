using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TogglePanel : MonoBehaviour {

	[SerializeField] private GameObject settingPanel;
	[SerializeField] private GameObject BGTransparency;

	// Use this for initialization
	void Start () {
		//make sure to drag and drop the menu panel gameobject in the Editor
		Assert.IsNotNull (settingPanel);
		Assert.IsNotNull (BGTransparency);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SettingIsClicked() {
		Debug.Log ("Transparent Active");
		settingPanel.SetActive (true);
		BGTransparency.SetActive (true);
	}

	public void ClosePanel() {
		settingPanel.SetActive (false);
		BGTransparency.SetActive (false);
	}
}
