using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movescript : MonoBehaviour
{
	Rigidbody2D rb;
	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}
	void FixedUpdate() {
		rb.velocity = new Vector2(-2f, 0f);
	}
}
