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
    }

	void LoadNewScene(int sceneToLoad) {
		SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
	}

	void LoadNewScene(string sceneToLoad) {
		SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
	}

	public void CoroutinePasser(IEnumerator func)
    {
        StartCoroutine(func);
	}
}
