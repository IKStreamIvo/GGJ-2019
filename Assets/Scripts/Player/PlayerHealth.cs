using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

	[SerializeField] private static int health = 3;
	[SerializeField] private bool invincable;

    // Update is called once per frame
    void Update() {
        
    }

	public void Hurt() {
		if(health >= 1) {
			SoundEffects.Play(7 + Random.Range(0, 3)); //TODO change hurt
			if (invincable)
				return;

			health--;
		} else {
			//die();
		}
	}

	public static void Reheal() {
		health = 3;
	}
}
