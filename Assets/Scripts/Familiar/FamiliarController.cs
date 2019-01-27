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
    private Vector2 m_Velocity = Vector3.zero;
    private bool isPetted;
    [SerializeField] private float petDistance;

    private void Start(){
        if(player == null){
            player = GameObject.FindObjectOfType<PlayerInput>();
        }
    }

    private void Update(){
        float dist = Mathf.Abs(transform.position.x - player.transform.position.x);
        if(!isPetted){
            if(!moveCloser && dist > followDistance){
                moveCloser = true;
                animator.SetBool("Idle", false);
                animator.SetBool("Run", true);
                animator.SetFloat("5Second limit", 0f);
            }else if(moveCloser && dist <= stopFollowDistance){
                moveCloser = false;
                animator.SetBool("Run", false);
                animator.SetFloat("5Second limit", 0f);
                animator.SetBool("Idle", true);
            }

            //Idle time
            if(!moveCloser){
                animator.SetFloat("5Second limit", animator.GetFloat("5Second limit") + Time.deltaTime);
            }
        }else{
            if(dist <= petDistance){
                Debug.Log("petclose");
                moveCloser = false;
                animator.SetBool("Idle", false);
                animator.SetBool("Run", false);
                animator.SetFloat("5Second limit", 0f);

                animator.SetBool("Pet", true);
            }
        }
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
        }else{
            newVelocity.x = 0f;
        }
        
        rb.velocity = Vector2.SmoothDamp(rb.velocity, newVelocity, ref m_Velocity, movementSmoothing);
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
