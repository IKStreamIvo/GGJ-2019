using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement : MonoBehaviour{
	public Achievements._Achievement achievement;

	public void RetrieveData() {
		achievement = Achievements.me.retrieveAchievement(achievement.index);
	}
}