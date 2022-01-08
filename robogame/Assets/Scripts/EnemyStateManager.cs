using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

    [SerializeField] private GameObject enemyAtLocation;
    
    
    public void enemyOnStateChange() {
        
        switch (enemyState.CurrentRound.name) {
            
            case"EnemyPerformAction":
                foreach (var node in _grid2D.Grid) {
                    
                    node.setEnemy(null);
                    
                }
                foreach (var enemy in aliveEnemies.items) {

                    if (enemy.GetComponent<EnemyDataHandler>().selectedAction == null) {
                        Debug.Log("THIS ENEMY " + enemy + " HAS NO ACTION THIS TURN FOR SOME REASON");
                        continue;
                    }
                    
                    //PERFORM YOUR ACTION YOUR SELECTED LAST TURN;
                    //Debug.Log(enemy.name + " is performing " + enemy.GetComponent<EnemyDataHandler>().selectedAction);
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
        if (enemyDataHandler.selectedAction != null) {
            Debug.Log("This creature should have a null action " + enemy.name);
        }
//        Debug.Log("passed getting a path" + enemy.name);
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

    public void FixedUpdate() { // THIS IS ONLY FOR DEBUGGIN  THE  LINES OF THE PATHS FOR ENEMIES. JUST FOR TESTING
        
        foreach (var enemy in aliveEnemies.items) {
            if (enemy.GetComponent<EnemyDataHandler>().getPath() == null) {
                return;
            }
            List<Node2D> p = enemy.GetComponent<EnemyDataHandler>().getPath();
            Node2D n = p[0];
            for (int i = 0; i < p.Count; i++) {
                Debug.DrawLine(n.getWorldPosition(),p[i].getWorldPosition(),Color.magenta);
                n = p[i];
            }
        }

        foreach (var node in _grid2D.Grid) {
            if (node.getEnemy() == null) continue;
            Instantiate(enemyAtLocation, node.getWorldPosition(), quaternion.identity);
            Debug.Log("spawning dummy");
        }
    }
}


