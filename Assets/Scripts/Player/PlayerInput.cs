using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	Rigidbody2D r2d;

	public float moveSpeed;

	private void Start() {
		if(r2d == null) {
			r2d = GetComponent<Rigidbody2D>();
		}
	}


	// a jumps
	// b crouch
	// left toggle for axis.
	// Update is called once per frame
	void Update() {
		if(Input.GetAxis("Horizontal") > 0f) {
			r2d.AddForce(Vector2.right * moveSpeed);
		} else if (Input.GetAxis("Horizontal") < 0f){
			r2d.AddForce(Vector2.left * moveSpeed);
		} else {
			r2d.velocity = Vector2.zero;
		}
    }
}
