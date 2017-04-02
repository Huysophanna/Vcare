using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageCalculation : MonoBehaviour {

	public Image healthBar;
	public float val;
	public Text txt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (val < 1) {
			healthBar.fillAmount = val;
			txt.text = ((float)healthBar.fillAmount) * 100 + "%";
		} else
			return;
	}
}
