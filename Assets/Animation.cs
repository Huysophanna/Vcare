using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour {

	[SerializeField] private Animator[] anim;
	[SerializeField] private GameObject AvartarMale;
	[SerializeField] private GameObject AvartarFemale;

	private string gender = "Male";

	// Use this for initialization
	void Start () {
		anim = new Animator[2];

		if (gender == "Male") {
			AvartarMale.SetActive (true);
			anim [0].Play ("AvartarAnim");
		} else if (gender == "Female") {
			AvartarFemale.SetActive (true);
			anim [1].Play ("CharacterAnim");
		}

		//anim.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
