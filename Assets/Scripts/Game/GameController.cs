using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController gCont;

    void Start()
    {
		gCont = this;

		Achievements.me.LoadAchievements();
    }

	void LoadNewScene(int sceneToLoad) {
		SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
	}

	void LoadNewScene(string sceneToLoad) {
		SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
	}

	void LoadNewScene(int sceneToLoad, bool additive) {
		SceneManager.LoadScene(sceneToLoad, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
	}

	void LoadNewScene(string sceneToLoad, bool additive) {
		SceneManager.LoadScene(sceneToLoad, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
	}

	public void CoroutinePasser(IEnumerator func)
    {
        StartCoroutine(func);
	}

	public void OnApplicationQuit() {
		Achievements.me.SaveAchievements();
	}
}
