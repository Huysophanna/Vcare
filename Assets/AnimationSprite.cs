using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSprite : MonoBehaviour {

	[SerializeField] private Animator[] maleanim = new Animator[5];
	[SerializeField] private Animator[] femaleanim = new Animator[5];
	[SerializeField] private GameObject AM1;
	[SerializeField] private GameObject AM2;
	[SerializeField] private GameObject AM3;
	[SerializeField] private GameObject AM4;
	[SerializeField] private GameObject AM5;
	[SerializeField] private GameObject AF1;
	[SerializeField] private GameObject AF2;
	[SerializeField] private GameObject AF3;
	[SerializeField] private GameObject AF4;
	[SerializeField] private GameObject AF5;


	//private string gender = PlayerPrefs.GetString("Gender");
	private string gender = "M";
	private int newUser;

	// Use this for initialization
	void Start () {
		newUser = PlayerPrefs.GetInt ("firstLaunch");
		Debug.Log ("new User " + newUser);
		if (newUser != 1) {
			Debug.Log ("I am a new user");
			PlayerPrefs.SetInt ("firstLaunch", 1);
			int newAcc = Random.Range(0, 5);
			setAnimation (newAcc, true);
		} else {
			Debug.Log ("I am an existing user");
			var exsAcc = PlayerPrefs.GetInt ("Index");
			setAnimation (exsAcc, false);
		}
	}

	void setAnimation(int index, bool newUser) {
		Debug.Log ("My index " + index + newUser);
		switch(index){
		case 0:
			if (newUser) {
				PlayerPrefs.SetInt ("Index", 0);
				checkUserGender (0);
			} else {
				checkUserGender (0);
			}
			break;
		case 1:
			if (newUser) {
				PlayerPrefs.SetInt ("Index", 1);
				checkUserGender (1);
			} else {
				checkUserGender (1);
			}
			break;
		case 2:
			if (newUser) {
				PlayerPrefs.SetInt ("Index", 2);
				checkUserGender (2);
			} else {
				checkUserGender (2);
			}
			break;
		case 3:
			if (newUser) {
				PlayerPrefs.SetInt ("Index", 3);
				checkUserGender (3);
			} else {
				checkUserGender (3);
			}
			break;
		case 4:
			if (newUser) {
				PlayerPrefs.SetInt ("Index", 4);
				checkUserGender (4);
			} else {
				checkUserGender (4);
			}
			break;
		default:
			Debug.Log ("Out of bounce");
			break;
		}
	}

	void checkUserGender(int index) {
		Debug.Log ("checking user gender " +  index + "   ======" + gender);
		if (gender == "M") {
			switch (index) {
			case 0:
				AM1.SetActive (true);
				break;
			case 1:
				AM2.SetActive (true);
				break;
			case 2:
				AM3.SetActive (true);
				break;
			case 3:
				AM4.SetActive (true);
				break;
			case 4:
				AM5.SetActive (true);
				break;
			default:
				Debug.Log ("Index out of bounce");
				break;
			}
			maleanim [index].Play ("M" + index);
		} else if (gender == "F") {
			switch (index) {
			case 0:
				AF1.SetActive (true);
				break;
			case 1:
				AF2.SetActive (true);
				break;
			case 2:
				AF3.SetActive (true);
				break;
			case 3:
				AF4.SetActive (true);
				break;
			case 4:
				AF5.SetActive (true);
				break;
			default:
				Debug.Log ("Index out of bounce");
				break;
			}
			femaleanim [index].Play ("F" + index);
		} else {
			Debug.Log ("Problem with get gender");
		}
}
}