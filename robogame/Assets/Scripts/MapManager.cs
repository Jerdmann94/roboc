using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;


public class MapManager : MonoBehaviour {
    public GameObject confirmationButton;
    public GameObject comabtObjects;
    public GameObject mapObjects;
    public CombatManager combatManager;
    public SceneChoiceSo sceneChoiceSo;
    private List<EnemyList> enemyLists;
    public EnemyDeckLists enemyListSo;
    public GoRunTimeSet obstacleMasterList;
    

    static Random rnd = new Random();
    public void Awake() {
        enemyLists = enemyListSo.enemyLists;
        confirmationButton.SetActive(false);
    }

    public void onConfirmationPress() {
        switch (sceneChoiceSo.choiceType) {
            case ChoiceType.Boss:
                break;
            case ChoiceType.Combat:
                combatManager.enemiesDeck = getEnemiesDeck();
                combatManager.obstacleMaster = getObstacleMaster();
                comabtObjects.SetActive(true);
                mapObjects.SetActive(false);
                break;
            case ChoiceType.Elite :
                break;
            case ChoiceType.Mystery:
                break;
        }
    }

    private GameObject getObstacleMaster() {
        int index = rnd.Next(obstacleMasterList.items.Count);
       return obstacleMasterList.items[index];
    }

    private EnemyList getEnemiesDeck() {
        
        int index = rnd.Next(enemyLists.Count);
        return enemyLists[index];

    }
}
