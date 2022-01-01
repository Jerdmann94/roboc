using System;
using System.Collections.Generic;
using System.IO;
using ScriptableObjects.Sets;

using Unity.Mathematics;
using UnityEditor.Tilemaps;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

public class EnemyStateManager : MonoBehaviour {
    public TurnState enemyState;
    public CombatManager combatManager;
    public GORunTimeSet aliveEnemies;
    //public GORunTimeSet playerSet;
    //public Pathfinding2D pathfinder;
    public GameObject gridOwner;
    private Grid2D _grid2D;
    
    
    public void enemyOnStateChange() {
        
        switch (enemyState.CurrentRound.name) {
            
            case"EnemyPerformAction":
                
                foreach (var enemy in aliveEnemies.items) {

                    if (enemy.GetComponent<EnemyDataHandler>().selectedAction == null) {
                        Debug.Log("THIS ENEMY " + enemy + " HAS NO ACTION THIS TURN FOR SOME REASON");
                        continue;
                    }
                    //PERFORM YOUR ACTION YOUR SELECTED LAST TURN;
                    enemy.GetComponent<EnemyDataHandler>().selectedAction.Execute(enemy);
                    enemy.GetComponent<EnemyDataHandler>().selectedAction = null;

                }
                enemyState.nextState();
                break;
            case "EnemyDecideNextTurn":
                
                foreach (var enemy in aliveEnemies.items) {

                    decideNextTurn(enemy);

                }
                enemyState.nextState();
                break;
            case"EndTurn":

                _grid2D = gridOwner.GetComponent<Grid2D>();
                foreach (var node in _grid2D.Grid) {
                    if (node.enemy != null) {
                        //Debug.Log(node.worldPosition + " has an enemy");
                        node.enemy = null;
                    }
                }
                combatManager.combatState.nextState();
                break;
            default:
                break;
        }
    }


    public void startState() {
        
        enemyState.startState();
    }

    public void initializeEnemyState() {
        _grid2D = gridOwner.GetComponent<Grid2D>();
        foreach (var enemy in aliveEnemies.items) {
            _grid2D.setEnemyAtPosition(enemy);
            decideNextTurn(enemy);
            // pathfinder.FindPath(enemy.transform.position,playerSet.items[0].transform.position);
            // enemy.GetComponent<EnemyDataHandler>().path = _grid2D.path;
        }
        
    }

    private void decideNextTurn(GameObject enemy) {
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        if (enemyDataHandler.targetCheck.Check(enemy)) { //IF THIS ENEMY DOES NOT HAVE A TARGET, FIND ONE
            enemyDataHandler.targetCheck.Execute(enemy);
        }
        foreach (var action in enemyDataHandler.actions) {
            if (action.Check(enemy)) {
                //Debug.Log("setting next action as " + action.name);
                enemyDataHandler.selectedAction = action;
                break;
            }
        }
        if (enemy.GetComponent<EnemyDataHandler>().selectedAction == null) {
            Debug.Log("THIS ENEMY " + enemy + " HAS NO ACTION THIS TURN FOR SOME REASON");
            return;
        }

        
        enemyDataHandler.selectedAction.unHighlight(enemy);
        enemyDataHandler.selectedAction.Highlight(enemy,ScriptableObject.CreateInstance<Tile>());
    }
}


