using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHogger : Enemy {

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileVelocity;
    private float _attackTimer = 0f;

    public override void Update() {
        base.Update();

        if(_attackTimer >= attackDelay){
            Fire();
            _attackTimer = 0f;
        }else{
            _attackTimer += Time.deltaTime;
        }
    }

    private void Fire(){
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
        
        Vector2 direction = (player.position - transform.position).normalized;
        direction *= projectileVelocity;
        projRb.velocity = direction;
    }
}
