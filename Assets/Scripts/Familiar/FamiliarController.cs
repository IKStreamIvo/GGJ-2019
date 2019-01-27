using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarController : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput player;
    [SerializeField] private float followDistance;
    [SerializeField] private float stopFollowDistance;
    [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float movementSpeedMultiplier = 1.1f;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprRenderer;

    private bool facingRight = true;
    private bool moveCloser;
    private bool jumpy;
    private Vector2 m_Velocity = Vector3.zero;
    private bool isPetted;
    [SerializeField] public float petDistance;
    [SerializeField] private float groundCheckDistance;
    private bool m_grounded;
    [SerializeField] private float jumpDistanceTrigger;
    [SerializeField] private float jumpForce;
    [SerializeField] private float teleportDist;

    private void Start(){
        if(player == null){
            player = GameObject.FindObjectOfType<PlayerInput>();
        }
    }

    public void Teleport(){
        transform.position = player.feetAnchor.position;
    }

    private void Update(){
        float dist = Mathf.Abs(transform.position.x - player.feetAnchor.position.x);
        float ydist = Mathf.Abs(transform.position.y - player.feetAnchor.position.y);

        if(Vector2.Distance(transform.position, player.feetAnchor.position) > teleportDist){
            Teleport();
        }

        if(!isPetted){
            if(!moveCloser && dist > followDistance){
                moveCloser = true;
                animator.SetBool("Idle", false);
                animator.SetBool("Run", true);
                animator.SetFloat("5Second limit", 0f);
            }

            if(ydist > jumpDistanceTrigger){
//                Debug.Log("shouldJump");
                moveCloser = true;
                jumpy = true;
                animator.SetBool("Jump", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Run", false);
                animator.SetFloat("5Second limit", 0f);
            }

            if(moveCloser && dist <= stopFollowDistance && ydist <= jumpDistanceTrigger){
                moveCloser = false;
                animator.SetBool("Run", false);
                animator.SetFloat("5Second limit", 0f);
                animator.SetBool("Idle", true);
                animator.SetBool("Jump", false);
            }

            //Idle time
            if(!moveCloser){
                animator.SetFloat("5Second limit", animator.GetFloat("5Second limit") + Time.deltaTime);
            }
        }else{
            if(dist <= petDistance){
                moveCloser = false;
                animator.SetBool("Idle", false);
                animator.SetBool("Run", false);
                animator.SetFloat("5Second limit", 0f);

                animator.SetBool("Pet", true);
            }
        }

        
        animator.SetBool("Jump", m_grounded);
    }

    private void FixedUpdate() {
        Vector2 newVelocity = rb.velocity;
        if(moveCloser){
            newVelocity.x = player.moveSpeed * movementSpeedMultiplier;
            //left or right?
            float difference = player.transform.position.x - transform.position.x;
            if(difference > 0){
                newVelocity.x *= 1;
                if(!facingRight){
                    facingRight = true;
                    sprRenderer.flipX = false;
                }
            }else{
                newVelocity.x *= -1;
                if(facingRight){
                    facingRight = false;
                    sprRenderer.flipX = true;
                }
            }
            if(jumpy && m_grounded){
                rb.AddForce(new Vector2(0f, jumpForce));
                Debug.Log("forcejump");
            }
        }else{
            newVelocity.x = 0f;
        }
        
        rb.velocity = Vector2.SmoothDamp(rb.velocity, newVelocity, ref m_Velocity, movementSmoothing);
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

    public void Pet(bool state){
        if(isPetted != state){
            isPetted = state;
            if(!isPetted){
                animator.SetBool("Pet", false);
            }else{
                moveCloser = true;   
            }
        }
    }
}
