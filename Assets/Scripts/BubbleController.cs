using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour {

	[SerializeField] private bool isUp;
	[SerializeField] private float speed = 6f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0,0));
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1,1));

		max.x = max.x - 0.5f;
		min.x = min.x + 0.5f;

		max.y = max.y - 0.5f;
		min.y = min.y - 1.5f;

		//update position of the object
		Vector2 pos = transform.position;
		Vector2 direction = new Vector2 (0, -1);
		pos.x = Mathf.Clamp (pos.x, min.x, max.x);


		pos -=  direction * speed * Time.deltaTime;

		transform.position = pos;

		if (transform.position.x > max.x) {
				Destroy (gameObject);
		}
	}
	public void setSpeed(float setSpeed){
		speed = setSpeed;
	}
}
