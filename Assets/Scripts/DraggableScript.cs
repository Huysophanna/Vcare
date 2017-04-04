using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableScript : MonoBehaviour {
	string presentGameObjectTouch = "";
	string previousGameObjectTouch = "";
	bool setPrevious = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log (Input.touchCount);

	}

	void OnMouseUp(){
		
	}

	void OnMouseDown(){
		Debug.Log ("This is " + gameObject.name.ToString ());

		if (setPrevious) {
			previousGameObjectTouch = presentGameObjectTouch;
		}
		setPrevious = true;

		presentGameObjectTouch = gameObject.name.ToString();
		StartCoroutine ("ResetPresentGameObjectTouch");

		if (previousGameObjectTouch == presentGameObjectTouch) {
			Debug.Log ("2 dong");
			MealControllerScript.Instance.RestaurantIsClicked (gameObject.name.ToString ());
		}

	}	

	void OnMouseDrag(){
		Vector2 mousePosition = new Vector2 (Input.mousePosition.x,Input.mousePosition.y);
		Vector2 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
		transform.position = objPosition;
	}

	IEnumerator ResetPresentGameObjectTouch() {
		yield return new WaitForSeconds (0.5f);
		presentGameObjectTouch = "";
	}

}
