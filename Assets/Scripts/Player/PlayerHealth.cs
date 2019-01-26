using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

	[SerializeField] private int health = 3;

    // Update is called once per frame
    void Update() {
        
    }

	public void Hurt() {
		if(health >= 1) {
			health--;
		} else {
			//die();
		}
	}
}
