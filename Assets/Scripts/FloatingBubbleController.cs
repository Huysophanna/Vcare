using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBubbleController : MonoBehaviour {
	
	[SerializeField] private GameObject detailbubbleobj;

	public float speed = 5f;
	// Use this for initialization

	void OnMouseUp(){
		detailbubbleobj.GetComponent<DetailBubbleManager>().bubbleSelected (gameObject.name);
		detailbubbleobj.GetComponent<DetailBubbleManager>().closeDragDrop();
	}

	void OnMouseDown(){
		detailbubbleobj.GetComponent<DetailBubbleManager>().setDragDrop();
	}	

	void OnMouseDrag(){
		Vector2 mousePosition = new Vector2 (Input.mousePosition.x,Input.mousePosition.y);
		Vector2 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
		transform.position = objPosition;
		detailbubbleobj.GetComponent<DetailBubbleManager>().closeDragDrop();
		detailbubbleobj.GetComponent<DetailBubbleManager>().bubbledraggedin (objPosition, gameObject.name);
		//	Debug.Log (gameObject.name);
	}


	void Start () {
	}

	// Update is called once per frame
	void Update () {

		// this is the bottom left point of the Screen
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0,0));

		//this is the top right of the screen
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1,1));

		//This is the floating movement -- adding force to the rigibody
		Vector2 pos = transform.position;

		if (transform.position.y > max.y || transform.position.y < min.y || transform.position.x > max.x || transform.position.x < min.x) {
			transform.position = new Vector2 (Random.Range(min.x,max.x),Random.Range(min.y,max.y));
		}



	}


}
