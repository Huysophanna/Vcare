using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DashboardScript : MonoBehaviour {

	[SerializeField] private GameObject settingPanel;
	private bool musicOption = true;
	private bool soundOption = true;

	// Use this for initialization
	void Start () {
		//make sure to drag and drop the menu panel gameobject in the Editor
		Assert.IsNotNull (settingPanel);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SettingIsClicked() {
		settingPanel.SetActive (true);
	}

	public void ClosePanel() {
		settingPanel.SetActive (false);
	}

	public void MusicOptToggle(bool val) {
		musicOption = val;
		Debug.Log ("Music Option: " + musicOption);
	}

	public void SoundOptToggle(bool val) {
		soundOption = val;
		Debug.Log ("Sound Option: " + soundOption);
	}

}
