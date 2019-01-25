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
	[SerializeField] private bool m_shot;
	private Vector2 m_Velocity = Vector2.zero;
	[SerializeField] private float movementSmoothing = .05f;
	[SerializeField] private float jumpForce;
	[SerializeField] private float distFromGround = 2f;

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
		GroundCheck();
		Vector2 newVelocity = r2d.velocity;
		float horInput = Input.GetAxis("Horizontal");
		float verInput = Input.GetAxis("Vertical");
		if (horInput != 0f) {
			newVelocity.x = moveSpeed * horInput;
			//left or right?
			bool right = horInput > 0f ? true : false;
			if (!m_FacingRight && right) {
				Flip(m_FacingRight);
				m_FacingRight = true;
			} else if (m_FacingRight && !right) {
				Flip(m_FacingRight);
				m_FacingRight = false;
			}

			m_moving = true;
		} else {
			newVelocity.x = 0f;
			m_moving = false;
		}

		r2d.velocity = Vector2.SmoothDamp(r2d.velocity, newVelocity, ref m_Velocity, movementSmoothing);

		if (verInput < -0.2f) { //crouching
			m_crouching = true;
			//DoCrouching();
		} else {
			m_crouching = false;
		}

		if (Input.GetButtonDown("Jump")) {
			if (m_grounded) {
				r2d.AddForce(new Vector2(0f, jumpForce));
				m_grounded = false;
			}
		}

		if(Input.GetAxis("LeftTrigger") > 0.1f) {
			Shoot(true);
		}

		if(Input.GetAxis("RightTrigger") > 0.1f && !m_shot) {
			Shoot(false);
			m_shot = true;
		} else {
			m_shot = false;
		}
	}

	private void GroundCheck() {
		RaycastHit2D hit2D;
		if (Physics2D.Raycast(transform.position, Vector2.down, distFromGround, 1 << 9)) {
			Debug.DrawRay(transform.position, Vector2.down * distFromGround, Color.red);
			m_grounded = true;
		} else {
			m_grounded = false;
		}
	}

	void Flip(bool toRight) {
			m_sRend.flipX = toRight;
	}

	/// <summary>
	/// Shooting
	/// </summary>
	/// <param name="hold">Is the beam supposed to show?</param>
	void Shoot(bool hold) {
		if (hold) { //beam

		} else { //disk
			GameObject temp = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
			temp.AddComponent<Rigidbody2D>();
			temp.GetComponent<Rigidbody2D>().velocity = m_FacingRight ? Vector2.right : Vector2.left;
		}
	}
}
