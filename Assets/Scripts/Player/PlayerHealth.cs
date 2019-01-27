using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

	[SerializeField] private static int health = 3;

    // Update is called once per frame
    void Update() {
        
    }

	public void Hurt() {
		if(health >= 1) {
			health--;
			SoundEffects.Play(SoundEffects.Clips.Hurt1); //TODO change hurt
		} else {
			//die();
		}
	}

	public static void Reheal() {
		health = 3;
	}
}
