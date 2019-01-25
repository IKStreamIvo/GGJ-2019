using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarController : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput player;
    [SerializeField] private float followDistance;
    [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float movementSpeed;

    private bool moveCloser;
    private Vector2 m_Velocity = Vector3.zero;

    private void Start(){
        if(player == null){
            player = GameObject.FindObjectOfType<PlayerInput>();
        }
    }

    private void Update(){
        if(Vector2.Distance(transform.position, player.transform.position) > followDistance){
            moveCloser = true;
        }else{
            moveCloser = false;
        }
    }

    private void FixedUpdate() {
        Vector2 newVelocity = rb.velocity;
        if(moveCloser){
            newVelocity.x = movementSpeed;
            //left or right?
            float difference = player.transform.position.x - transform.position.x;
            if(difference > 0){
                newVelocity.x *= 1;
            }else{
                newVelocity.x *= -1;
            }
            Debug.Log(difference);
        }else{
            newVelocity.x = 0f;
        }
        
        rb.velocity = Vector2.SmoothDamp(rb.velocity, newVelocity, ref m_Velocity, movementSmoothing);
    }
}
