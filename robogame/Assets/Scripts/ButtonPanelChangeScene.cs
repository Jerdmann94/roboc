using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanelChangeScene : MonoBehaviour {
    public SceneChoiceSo sceneChoiceSo;
    public ChoiceType choiceType;
    public GameObject confirmationButton;
    public void changeSceneChoice() {
        sceneChoiceSo.choiceType = choiceType;
        confirmationButton.SetActive(true);
    }
}
