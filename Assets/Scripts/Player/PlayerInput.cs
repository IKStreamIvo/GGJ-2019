﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[Header("Player Components")]
	Rigidbody2D r2d;
	SpriteRenderer m_sRend;
    LineRenderer m_lRend;

	public float moveSpeed;

	[SerializeField] private bool m_FacingRight;
	[SerializeField] private bool m_crouching;
	[SerializeField] private bool m_moving;
	[SerializeField] private bool m_grounded;
	[SerializeField] private bool m_shot;
    private Vector2 m_Velocity = Vector2.zero;
    [SerializeField] private float movementSmoothing = .05f;

	[SerializeField] private GameObject weapon_disk;
	[SerializeField] private Transform arm;
    [SerializeField] private float m_bulletSpeed;

    private void Start() {
		if (r2d == null)
			r2d = GetComponent<Rigidbody2D>();
		if(m_sRend == null)
			m_sRend = GetComponent<SpriteRenderer>();
		if (m_lRend == null)
            m_lRend = GetComponent<LineRenderer>();
	}

	private void Update() {
		if(Input.GetMouseButtonDown(0)){
			EnergyBar.Drain(.5f);
		}
		if(Input.GetMouseButtonDown(1)){
			EnergyBar.Drain(.5f, true);
		}else if(Input.GetMouseButtonUp(1)){
			EnergyBar.Drain(0f, false);
		}

		if (Input.GetAxis("LeftTrigger") > 0.1f) {
			if (EnergyBar.HasEnergy(1f)) {
				EnergyBar.Drain(1f, true);
				Shoot(true);
			}
		} else {
			EnergyBar.Drain(0f, false);
		}
	}

	// a jumps
	// b crouch
	// left toggle for axis.
	// Update is called once per frame
	void FixedUpdate() {
		Aiming();

		Vector2 newVelocity = r2d.velocity;
		float horInput = Input.GetAxis("Horizontal");
        if(horInput != 0f){
            newVelocity.x = moveSpeed * horInput;
            //left or right?
			bool right = horInput > 0f ? true : false;
			if (!m_FacingRight && right) {
				Flip(m_FacingRight);
				m_FacingRight = true;
			}else if(m_FacingRight && !right){
				Flip(m_FacingRight);
				m_FacingRight = false;
			}

			m_moving = true;
        }else{
            newVelocity.x = 0f;
			m_moving = false;
        }
        
        r2d.velocity = Vector2.SmoothDamp(r2d.velocity, newVelocity, ref m_Velocity, movementSmoothing);

		if (Input.GetAxis("Vertical") < -0.2f) { //crouching
			m_crouching = true;
			//DoCrouching();
		} else {
			m_crouching = false;
		}

		if (Input.GetAxis("RightTrigger") > 0.1f) {
			if (!m_shot) {
				m_shot = true;
				Shoot(false);
			}
		} else {
			m_shot = false;
		}
	}

	private void GroundCheck(){
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

/// <summary>
    /// Shooting
    /// </summary>
    /// <param name="hold">Is the beam supposed to show?</param>
    void Shoot(bool hold) {
        if (hold) { //beam
            r2d.AddForce(m_FacingRight ? Vector2.left * 10f : Vector2.right * 10f);
            RaycastHit2D hit;
            if(hit = Physics2D.Raycast(transform.position, transform.TransformDirection(m_FacingRight ? Vector2.right : Vector2.left), Mathf.Infinity, ~1<<10)) {
                m_lRend.SetPosition(0, transform.position);
                m_lRend.SetPosition(1, hit.point);
                Debug.Log(hit.point);
            }
        } else { //disk
            if (EnergyBar.HasEnergy(0.5f)) {
                EnergyBar.Drain(0.5f);
                GameObject temp = Instantiate(weapon_disk, transform.position, Quaternion.identity);
                temp.GetComponent<Rigidbody2D>().velocity = m_FacingRight ? Vector2.right * m_bulletSpeed : Vector2.left * m_bulletSpeed;
            }

        }
    }

	private void Telekinesis(){
		if(Input.GetButtonDown("Fire3")){

		}
	}

	private void Aiming(){
		float hor = Input.GetAxis("AimHor");
		float ver = Input.GetAxis("AimVer");
		Vector3 targetRot = new Vector3 (0f, 0f, Mathf.Atan2 (ver, hor) * 180 / Mathf.PI);
		if(hor == 0f && ver == 0f){
			targetRot = new Vector3(0f, 0f, 180f);
		}
		arm.localEulerAngles = targetRot;
	}
}