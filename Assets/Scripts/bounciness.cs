using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounciness : MonoBehaviour {

	public float speed = 5f;
	public Rigidbody2D rg;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		rg.AddForce (new Vector2 (Random.Range(-speed,speed),Random.Range(-speed,speed)));
	}


}
