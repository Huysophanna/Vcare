using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DashboardScript : MonoBehaviour {

	[SerializeField] private SpriteRenderer spriteBG;

	// Use this for initialization
	void Start () {
		//ensure that spriteBG is not null, drag and drop in the editor
		Assert.IsNotNull (spriteBG);

		Vector3 tempScale = spriteBG.transform.localScale;

		//get the exact width of the sprite
		float bgWidth = spriteBG.sprite.bounds.size.x;

		//get the world height in terms of size value, camera height * 2
		float worldHeight = Camera.main.orthographicSize * 2f;
		float worldWidth = worldHeight / Screen.height * Screen.width;

		//the exact scale value needed to scale to any screen size
		tempScale.x = worldWidth / bgWidth;
		spriteBG.transform.localScale = tempScale;


		Debug.Log ("bgWidth: " + bgWidth);
		Debug.Log ("worldHeight: " + worldHeight);
		Debug.Log ("worldWidth: " + worldWidth);
		Debug.Log ("tempScale.x: " + worldWidth / bgWidth);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
