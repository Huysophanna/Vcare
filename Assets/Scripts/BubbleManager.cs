using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour {

	[SerializeField] private GameObject Bubblegodown;


	// Use this for initialization
	void Start () {
		Invoke ("SpawnBubbledown", 1f);
		Invoke ("SpawnBubbledown", 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnBubbledown(){
		// this is the bottom left point of the Screen
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0,0));

		//this is the top right of the screen
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1,1));

		//instantiate an enemy
		GameObject aBubble = (GameObject)Instantiate (Bubblegodown);
		float randSize = Random.Range (0.3f, 0.5f);
		if (randSize >= 0.4f) {
			// big size so slow down speed
			aBubble.GetComponent<BubbleController>().setSpeed(3f);
		} else {
			// small size so increase speed
			aBubble.GetComponent<BubbleController>().setSpeed(7f);
		}
		aBubble.transform.localScale = new Vector2 (randSize,randSize);
		aBubble.transform.position = new Vector2 (Random.Range (min.x, max.x),min.y-2f);
		//Schedule another invoke
		Invoke ("SpawnBubbledown", Random.Range(0.1f,1f));
	}
}
