using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[Header("Player Components")]
	Rigidbody2D r2d;
	SpriteRenderer m_sRend;

	public float moveSpeed;

	[SerializeField] private bool m_FacingRight;
	[SerializeField] private bool m_crouching;

	private void Start() {
		if (r2d == null)
			r2d = GetComponent<Rigidbody2D>();
		if(m_sRend == null)
			m_sRend = GetComponent<SpriteRenderer>();
	}

	// a jumps
	// b crouch
	// left toggle for axis.
	// Update is called once per frame
	void FixedUpdate() {
		if (Input.GetAxis("Horizontal") > 0f) {
			if (r2d.velocity.x > 5f) {
				return;
			}

			r2d.AddForce(Vector2.right * moveSpeed);

			if (!m_FacingRight) {
				Flip(m_FacingRight);
				m_FacingRight = true;
			}

		} else if (Input.GetAxis("Horizontal") < 0f) {
			if (r2d.velocity.x < -5f) {
				return;
			}

			r2d.AddForce(Vector2.left * moveSpeed);

			if (m_FacingRight) {
				Flip(m_FacingRight);
				m_FacingRight = false;
			}

		} else {
			r2d.velocity = Vector2.zero;
		}

		if (Input.GetAxis("Vertical") < 0f) { //crouching
			m_crouching = true;
			//DoCrouching();
		}
	}

	void Flip(bool toRight) {
			m_sRend.flipX = toRight;
	}
}
