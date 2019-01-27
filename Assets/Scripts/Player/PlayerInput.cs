using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[Header("Player Components")]
	Rigidbody2D r2d;
	SpriteRenderer m_sRend;
    LineRenderer m_lRend;
	Animator animator;
	PlayerHealth pHealth;

	public float moveSpeed;
	[SerializeField] private FamiliarController familiar;
	
	[SerializeField] private bool m_FacingRight;
	[SerializeField] private bool m_crouching;
	[SerializeField] private bool m_moving;
	[SerializeField] private bool m_grounded;
	[SerializeField] private bool m_shot;
    private Vector2 m_Velocity = Vector2.zero;
    [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float jumpForce = 500f;

	[SerializeField] private GameObject weapon_disk;
	[SerializeField] private Transform arm;
	[SerializeField] private Transform handPoint;
    [SerializeField] private float m_bulletSpeed;
	[SerializeField] private float m_beamForce;
	[SerializeField] private float m_pushForce;
	[SerializeField] private float m_costBeam;
	[SerializeField] private float m_beamRange;
	[SerializeField] private float m_costDisk;

	private Vector2 aim;
    [SerializeField] private float groundCheckDistance;
    private bool isAiming;
    private Transform targetTelekinesis;
    private Vector3 targetTelekinesisOffset;
    private Vector2 newVelocity;
    private bool doJump;
	private bool allowAbilities;
    private bool m_meditating;
    [SerializeField] private float maxAirSpeed;
	[SerializeField] public Transform feetAnchor;

    private void Start() {
		if (r2d == null)
			r2d = GetComponent<Rigidbody2D>();
		if(m_sRend == null)
			m_sRend = GetComponent<SpriteRenderer>();
		if (m_lRend == null)
            m_lRend = GetComponent<LineRenderer>();
		if (animator == null)
			animator = GetComponent<Animator>();
		if(pHealth == null) {
			pHealth = GetComponent<PlayerHealth>();
		}
	}

	private void Update() {
		GroundCheck();

		//Movement
		newVelocity = r2d.velocity;
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
		} else{
            newVelocity.x = 0f;
			m_moving = false;
		}

		//Jumping
        if(Input.GetButton("Jump") && m_grounded){
			doJump = true;
		}else{
			doJump = false;
		}
        
		//Animation
		animator.SetBool("Jump", !m_grounded && m_shot);
		animator.SetBool("Run", m_moving);
		animator.SetBool("Idle", !m_moving && m_grounded);
		animator.SetBool("Pet", m_meditating);
		if(animator.GetBool("Idle")){
			animator.SetFloat("5Second Limit", animator.GetFloat("5Second Limit") + Time.deltaTime);
		}else{
			animator.SetFloat("5Second Limit", 0f);
		}
		
		//Abilities
		Aiming();
		///Meditating
		if (Input.GetKey(KeyCode.Joystick1Button3) && !m_moving){
			// m_meditating = true;
			if (!EnergyBar.EnergyFull()) { //show meditate animation.
			if(Vector2.Distance(feetAnchor.position, familiar.transform.position) > familiar.petDistance){
				familiar.Teleport();
			}
				EnergyBar.Drain(-1f, true);
				isAiming = false;
				m_meditating = true;
				familiar.Pet(true);

				bool right = (familiar.transform.position.x - transform.position.x ) > 0f ? true : false;
				if (!m_FacingRight && right) {
					Flip(m_FacingRight);
					m_FacingRight = true;
				}else if(m_FacingRight && !right){
					Flip(m_FacingRight);
					m_FacingRight = false;
				}
			}else{
				m_meditating = false;
				familiar.Pet(false);
				EnergyBar.Drain(0f, false);
			}
		} else if (m_meditating) { //stop meditate animation.
			EnergyBar.Drain(0f, false);
			m_meditating = false;
			familiar.Pet(false);
		} else{

		}

		if(isAiming){
			animator.SetFloat("5Second Limit", 0f);
			allowAbilities = true;
		}

		//Lazors //TODO put this back in allowAbilities when non-fried-kris found a workaround
		if (allowAbilities){
			if (Input.GetAxis("LeftTrigger") > 0.5f) {
				if (m_shot) {
					return;
				}

				if (EnergyBar.HasEnergy(m_costBeam * 2)) {
					EnergyBar.Drain(m_costBeam, true);
				}

				Shoot(true);
			}else if(Input.GetAxis("LeftTrigger") == 0f){
				m_lRend.SetPosition(0, handPoint.position);
				m_lRend.SetPosition(1, handPoint.position);
					if(!m_meditating) EnergyBar.Drain(0, false);
			}

			///Discs
			if (Input.GetAxis("RightTrigger") > .5f) {
				if (!m_shot) {
					m_shot = true;
					Shoot(false);
				}
			} else {
				m_shot = false;
			}
		}
	}

	void FixedUpdate() {
		Move();
		Telekinesis();
	}

	#region CharacterController
	// a jumps
	// b crouch
	// left toggle for axis.
	private void Move(){
		r2d.velocity = Vector2.SmoothDamp(r2d.velocity, newVelocity, ref m_Velocity, movementSmoothing);
		if (doJump) {
			r2d.AddForce(new Vector2(0f, jumpForce));
		} else {

		}
		Vector2 vel = r2d.velocity;
		vel.y = Mathf.Clamp(vel.y, -maxAirSpeed, maxAirSpeed);
		r2d.velocity = vel;
	}

	private void GroundCheck(){
		RaycastHit2D hit2D;
		Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);
		if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), groundCheckDistance, 1 << 9)) {
			m_grounded = true;
		} else {
			m_grounded = false;
		}
	}

	private void Flip(bool toRight) {
		Vector3 scl = transform.localScale;
		scl.x *= -1;
		transform.localScale = scl;
	}

	//TODO work on arm rotation when running
	private void Aiming(){
		float hor = Input.GetAxis("AimHor");
		float ver = -Input.GetAxis("AimVer") * (m_FacingRight ? 1 : -1);
		Vector3 targetRot = new Vector3 (0f, 0f, Mathf.Atan2 (ver, hor) * 180 / Mathf.PI);
		if(hor == 0f && ver == 0f){
			targetRot = new Vector3(0f, 0f, 180f);
		}
		arm.localEulerAngles = -targetRot;
		
		if(hor != 0f || ver != 0f){
			aim = -(arm.position - handPoint.position).normalized;
			isAiming = true;
		}else{
			isAiming = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Enemy") {
			pHealth.Hurt();
		}
		if(collision.tag == "Achievement") {
			Achievements.AwardAchievement(collision.GetComponent<Achievement>().achievement.index);
		}
	}
	#endregion

	#region Abilities
	private void Shoot(bool hold) {
		if(!isAiming) return;
        if (hold) { //beam
			if(EnergyBar.HasEnergy(m_costBeam)){
				RaycastHit2D hit = Physics2D.Raycast(handPoint.position, aim, m_beamRange, ~(1 << 10));
					r2d.AddForce(-aim * m_beamForce);
					m_lRend.SetPosition(0, handPoint.position);
				if(hit.collider != null) {
					m_lRend.SetPosition(1, hit.point);
				}else{
					m_lRend.SetPosition(1, handPoint.position + (Vector3)aim * m_beamRange);
				}
				EnergyBar.Drain(m_costBeam, true);
			}
        }else { //disk
            if (EnergyBar.HasEnergy(m_costDisk)) {
                EnergyBar.Drain(m_costDisk);
                GameObject temp = Instantiate(weapon_disk, handPoint.position, Quaternion.identity);
                temp.GetComponent<Rigidbody2D>().velocity = aim * m_bulletSpeed;
            }
		}
    }

	private void Telekinesis(){
		if(Input.GetButtonDown("Telekinesis")){
			RaycastHit2D hit = Physics2D.Raycast(handPoint.position, aim, Mathf.Infinity, 1<<12);
			if(hit.collider != null){
				targetTelekinesis = hit.collider.transform;
				targetTelekinesis.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

				targetTelekinesisOffset = targetTelekinesis.position - transform.position;

				targetTelekinesis.SetParent(transform, true);
				m_lRend.SetPosition(0, handPoint.position);
				m_lRend.SetPosition(1, targetTelekinesis.position);
			}
		}else if(Input.GetButtonUp("Telekinesis") && targetTelekinesis != null){
			targetTelekinesis.parent = null;
			targetTelekinesis.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

			targetTelekinesis.GetComponent<Rigidbody2D>().velocity = (aim * m_pushForce);
			
			targetTelekinesis = null;

			m_lRend.SetPosition(0, handPoint.position);
			m_lRend.SetPosition(1, handPoint.position);
		}

		if(targetTelekinesis != null){
			//targetTelekinesis.position = transform.position + (Vector3)(aim*targetTelekinesisOffset);
			targetTelekinesis.parent = arm;
		}
	}
	#endregion
}