using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour {
    public TurnState enemyState;
    public CombatManager combatManager;
    void Start()
    {
        
    }


    public void enemyOnStateChange() {
        switch (enemyState.CurrentRound.name) {
            case"EnemyMove":
                enemyState.nextState();
                break;
            case"EndTurn":
                combatManager.combatState.nextState();
                break;
            default:
                break;
        }
    }

    public void startState() {
        enemyState.startState();
    }
}
