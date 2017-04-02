﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DashboardScript : MonoBehaviour {

	[SerializeField] private GameObject settingPanel;
	[SerializeField] private GameObject BGTransparency;
	[SerializeField] private Text displayName;
	private bool musicOption = true;
	private bool soundOption = true;

	// Use this for initialization
	void Start () {
		//make sure to drag and drop the menu panel gameobject in the Editor
		Assert.IsNotNull (settingPanel);
		Assert.IsNotNull (BGTransparency);
		Assert.IsNotNull (displayName);

		displayName.text = GameManager.Instance.profileName;
		Debug.Log (GameManager.Instance.profileName);

		//set user to be an existing user
		PlayerPrefs.SetString ("ExistingUser", "true");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SettingIsClicked() {
		settingPanel.SetActive (true);
		BGTransparency.SetActive (true);
	}

	public void ClosePanel() {
		settingPanel.SetActive (false);
		BGTransparency.SetActive (false);
	}

	public void MusicOptToggle(bool val) {
		musicOption = val;
		Debug.Log ("Music Option: " + musicOption);
	}

	public void SoundOptToggle(bool val) {
		soundOption = val;
		Debug.Log ("Sound Option: " + soundOption);
	}

	public void SideMenuIsClicked() {
		SceneManager.LoadScene ("UserData");

	}

	public void LogOut() {
		PlayerPrefs.DeleteKey ("IsAuthenticated");
		SceneManager.LoadScene ("StartMenu");
	}

}