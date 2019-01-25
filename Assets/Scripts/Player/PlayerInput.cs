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
	[SerializeField] private bool m_moving;
	[SerializeField] private bool m_grounded;

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
		if (Input.GetAxis("Horizontal") > 0.1f) {
			if (r2d.velocity.x > 5f) {
				return;
			}

			r2d.AddForce(Vector2.right * moveSpeed);

			if (!m_FacingRight) {
				Flip(m_FacingRight);
				m_FacingRight = true;
			}

			m_moving = true;
		} else if (Input.GetAxis("Horizontal") < -0.1f) {
			if (r2d.velocity.x < -5f) {
				return;
			}

			r2d.AddForce(Vector2.left * moveSpeed);

			if (m_FacingRight) {
				Flip(m_FacingRight);
				m_FacingRight = false;
			}

			m_moving = true;
		} else {
			r2d.velocity = new Vector2(0, r2d.velocity.y);
			m_moving = false;
		}

		if (Input.GetAxis("Vertical") < -0.2f) { //crouching
			m_crouching = true;
			//DoCrouching();
		} else {
			m_crouching = false;
		}

		RaycastHit2D hit2D;
		if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), 2f, 1 << 9)) {
			m_grounded = true;
		} else {
			m_grounded = false;
		}
	}

	void Flip(bool toRight) {
			m_sRend.flipX = toRight;
	}
}
