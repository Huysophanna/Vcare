using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DetailBubbleManager : MonoBehaviour {

	[SerializeField] private GameObject[] objgame;
	[SerializeField] private GameObject settingPanel;
	[SerializeField] private GameObject BGTransparency;
	[SerializeField] private GameObject Dragdroptext;
	[SerializeField] private GameObject TextInfo;

	private bool musicOption = true;
	private bool soundOption = true;

	int gamenum = 0;
	int objselect = 0;
	string objname;
	string stringinfo;
	GameObject focusGameobj;
	bool bubbleisselect = false;
	bool bubbleinfoisselect = false;
	// Use this for initialization
	void Start () {
		Debug.Log (objgame.Length);
	}

	public void bubbledraggedin (Vector2 mousepos, string objgamename){
		Vector2 distance = new Vector2 (mousepos.x - transform.position.x, mousepos.y - transform.position.y);

		Debug.Log ("X:" + distance.x.ToString() + "Y:" + distance.y.ToString());
		//Debug.Log (mousepos.x.ToString() + "" + mousepos.y.ToString());

		if ((distance.x > -1 && distance.x < 1) && (distance.y > -1 && distance.y < 1)){
			if (bubbleisselect) {
				Destroy (focusGameobj);
				objgame [objselect].SetActive (true);
			}
			for (gamenum = 0; gamenum < objgame.Length; gamenum++) {
				if (objgamename.CompareTo(objgame [gamenum].name) == 0) {
					objselect = gamenum;
				}
			}

			//Debug.Log ("Mouse is inside the bubble!!!!");
			bubbleisselect = true;
			objgame [objselect].SetActive (false);
			focusGameobj = (GameObject) Instantiate (objgame[objselect]);
			focusGameobj.GetComponent<CircleCollider2D> ().enabled = false;
			focusGameobj.SetActive (true);
		}

	} 


	public void bubbleSelected (string objgamename){
		if (bubbleinfoisselect) {
			TextInfo.SetActive (false);
		}

		for (gamenum = 0; gamenum < objgame.Length; gamenum++) {
			if (objgamename.CompareTo(objgame [gamenum].name) == 0) {
				objselect = gamenum;
			}
		}
		bubbleinfoisselect = true;

		switch (objselect) {
		case 0:
			stringinfo = objgame[objselect].name + " To open the Facebook Report";
			break;
		case 1:
			stringinfo = objgame[objselect].name + " To open the Avatar Report";
			break;
		case 2:
			stringinfo = objgame[objselect].name + " To open the Setting Report";
			break;
		case 3:
			stringinfo = objgame[objselect].name + " To open the Back Report";
			break;
		}
		TextInfo.SetActive (true);
		TextInfo.GetComponent<Text> ().text = stringinfo;

	}

	public void setDragDrop() {
		Dragdroptext.SetActive (true);
	}

	public void closeDragDrop() {
		Dragdroptext.SetActive (false);
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

	// Update is called once per frame
	void Update () {
		if (bubbleisselect) {
			focusGameobj.transform.localEulerAngles = new Vector2 (0, 0);
			focusGameobj.transform.position = new Vector2 (0, 0);
			SettingIsClicked ();
		}
	}
}
