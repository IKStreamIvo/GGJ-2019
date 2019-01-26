using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private static GameController gCont;

    // Start is called before the first frame update
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
}
