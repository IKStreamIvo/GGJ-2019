using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Animator _animator;
    [Header("Enemy Configuration")]
    [SerializeField] private int _health;
    [SerializeField] private int _range;
    [SerializeField] public float attackDelay;
    public Transform player {get; private set;}

    public virtual void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void Update(){

    }

    public int getHealth(){
        return _health;
    }

    public virtual void Damage(int dmg = 1){
        _health -= dmg;
        if(_health <= 0){
            Die();
        }
        //update gfx?
    }

    public virtual void Die(){
        Debug.Log("Die " + gameObject.name);
        Destroy(gameObject);
    }

    public bool isPlayerInRange(){
        float distance = Vector2.Distance(transform.position, player.position);
        if(distance <= _range){
            return true;
        }else{
            return false;
        }
    }
}
