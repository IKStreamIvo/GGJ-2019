using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

	[SerializeField] private static int health = 3;
	[SerializeField] public List<Sprite> hearts;
	[SerializeField] public Sprite heartHolder;
	[SerializeField] private bool invincable;

	[SerializeField] public GameObject heartsObject;
	[SerializeField] public GameObject heartPrefab;

	public void Hurt() {
		if(health >= 1) {
			SoundEffects.Play(7 + Random.Range(0, 3)); //TODO change hurt
			if (invincable)
				return;

			health--;
			//hearts. = health;
			hearts = new List<Sprite>(health);
			Debug.Log("joejoe");
			UpdateUI();
		} else {
			//die();
		}
	}

	public void Reheal() {
		health = 3;
		for(int h = 0; h < health; h++) {
			hearts.Add(heartHolder);
		}
	}
	
	public void UpdateUI() {
		for (int h = 0; h < heartsObject.transform.childCount; h++) {
			Destroy(heartsObject.transform.GetChild(h).gameObject);
		}

		hearts = new List<Sprite>();

		for (int h = 0; h < health; h++) {
			GameObject tempHeart = Instantiate(heartPrefab);
			hearts.Add(heartHolder);
			tempHeart.transform.SetParent(heartsObject.transform, false);
		}
	}
}
