using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragBackCatagory : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void OnDrag(PointerEventData eventData)
	{
		print("I'm being dragged!");
		Debug.Log(eventData.pointerEnter.name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
