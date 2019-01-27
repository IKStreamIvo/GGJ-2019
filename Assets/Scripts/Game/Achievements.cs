using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Achievements : MonoBehaviour{

	[System.Serializable]
	public class _Achievement {
		public string objectName;
		public int index;
		public bool achieved;

		Achievement achievement;
	}

	[Header("Supposed to be equal to each other. Achievements 0 = AchievementObjects 0")]
	public List<_Achievement> achievements;
	public List<GameObject> achievementObjects;

	public static Achievements me;

	public string dataPath;

	public void Start() {
		me = this;
		dataPath = Application.dataPath + "/Achievements";
	}

	public static void AwardAchievement(int achievement) {
		me.achievements[achievement].achieved = true;
		SoundEffects.Play(SoundEffects.Clips.Ping);
	}

	public void SaveAchievements() {
		if(File.Exists(dataPath + ".txt")) {
			File.Delete(dataPath + ".txt");
		}

		StreamWriter sw = File.CreateText(dataPath + ".txt");

		foreach (_Achievement achievement in achievements) {
			string achievementString = achievement.objectName + ":" + achievement.index + ":" + achievement.achieved;
			sw.WriteLine(achievementString);
		}

		sw.Close();
	}

	public void LoadAchievements() {
		if (!File.Exists(dataPath + ".txt"))
			return;

		string[] loadedFile = File.ReadAllLines(dataPath + ".txt");

		for(int a = 0; a < loadedFile.Length; a++){
			string[] splitString = loadedFile[a].Split(':');

			achievements[a].objectName = splitString[0];
			achievements[a].index = int.Parse(splitString[1]);
			achievements[a].achieved = bool.Parse(splitString[2]);

			if(achievementObjects[a] == null) {
				return;
			}
			achievementObjects[a].GetComponent<Achievement>().RetrieveData();
		}
	}

	public bool Achieved(_Achievement achievement) {
		if (!achievements[achievement.index].achieved) {
			return true;
		}
		return false;
	}

	public _Achievement retrieveAchievement(int index) {
		return achievements[index];
	}
}
