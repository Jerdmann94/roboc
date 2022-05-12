using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {

	}

	public void selectScene(string sceneChoice) {
		switch (sceneChoice) {
			case "Test":
				SceneManager.LoadScene("Scenes/TestEnvironment");
				break;
			case "Scrappers":
				break;
		}
	}
}
