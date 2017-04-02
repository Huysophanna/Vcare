using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScrolling : MonoBehaviour {

	public string[] Stringinput;
	public float characterdelay = 0.1f;
	public Text txt;

	int characterindex=0;
	int stringindex=0;

	// Use this for initialization
	void Start () {
		StartCoroutine (TextAnimated ());
	}
	IEnumerator TextAnimated(){
		while (1 == 1) {
			yield return new WaitForSeconds (characterdelay);
			if (characterindex > Stringinput [stringindex].Length) {
				continue;
			}
			txt.text = Stringinput [stringindex].Substring (0, characterindex);
			characterindex++;
		}
	}


	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			if (characterindex < Stringinput [stringindex].Length) {
				characterindex = Stringinput [stringindex].Length;
			}else if(stringindex < Stringinput.Length-1){
				stringindex++;
				characterindex = 0;
			}
		}
	}
}
