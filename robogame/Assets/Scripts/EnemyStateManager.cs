using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ScriptableObjects.Sets;

using Unity.Mathematics;

using UnityEngine;

using UnityEngine.Tilemaps;

public class EnemyStateManager : MonoBehaviour {
    public TurnState enemyState;
    public CombatManager combatManager;
    public GoRunTimeSet aliveEnemies;
    public GameObject gridOwner;
    private Grid2D _grid2D;

    [SerializeField] private GameObject enemyAtLocation;
    [SerializeField] private GameObject claimedAtLocation;
    [SerializeField] private GameEvent enemyTileEffectCounterEmitter;
    [SerializeField] private GameEvent playerTileEffectCounterEmitter;
    public List<GameObject> enemiesToLateSpawnList;
    public List<GameObject> enemiesToLateKillList;


    public async void enemyOnStateChange() {
        
        switch (enemyState.CurrentRound.name) {
            
            case"EnemyPerformAction":
                enemyTileEffectCounterEmitter.emit();
                foreach (var enemy in aliveEnemies.items) {

                    if (enemiesToLateKillList.Contains(enemy)) {
                        continue;
                    }
                    if (enemy.GetComponent<EnemyDataHandler>().selectedAction == null) {
                        //Debug.Log("THIS ENEMY " + enemy + " HAS NO ACTION THIS TURN FOR SOME REASON");
                        continue;
                    }

                    if (enemy.GetComponent<EnemyDataHandler>().stunned) {
                        enemy.GetComponent<EnemyDataHandler>().stunned = false;
                        continue;
                    }
                    
                    //PERFORM YOUR ACTION YOUR SELECTED LAST TURN;
                    //Debug.Log(enemy.name + " is performing " + enemy.GetComponent<EnemyDataHandler>().selectedAction);
                     await enemy.GetComponent<EnemyDataHandler>().selectedAction.execute(enemy);

                    if (enemy.GetComponent<EnemyDataHandler>().selectedAction == null) continue;
                    enemy.GetComponent<EnemyDataHandler>().selectedAction.unHighlight(enemy);
                    enemy.GetComponent<EnemyDataHandler>().selectedAction = null;



                }

                foreach (var e in enemiesToLateSpawnList) {
                   aliveEnemies.add( e.GetComponent<TempSpawnScript>().spawnEnemy());
                    
                    
                }
                enemiesToLateKillList.ForEach(e => {
                    
                    e.GetComponent<EnemyDataHandler>().doDeath();
                    
                });
                foreach (var node in _grid2D.Grid) {
                    
                    //node.setEnemy(null);
                    node.setClaimed(false);
                    
                }
                enemiesToLateKillList.Clear();
                enemiesToLateSpawnList.Clear();
                enemyState.nextState();
                break;
            case "EnemyDecideNextTurn":
                
                foreach (var enemy in aliveEnemies.items) {

                    await decideNextTurn(enemy);

                }
                enemyState.nextState();
                break;
            case"EndTurn":

                _grid2D = gridOwner.GetComponent<Grid2D>();
                playerTileEffectCounterEmitter.emit();
                combatManager.combatState.nextState();
                break;
            default:
                break;
        }
    }


    public void startState() {
        
        enemyState.startState();
        InvokeRepeating("debugger", 0.0f, 1f);
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

    private async Task decideNextTurn(GameObject enemy) {
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        
        // if (enemyDataHandler.targetCheck.check(enemy)) { //IF THIS ENEMY DOES NOT HAVE A TARGET, FIND ONE
        //     await enemyDataHandler.targetCheck.execute(enemy);
        // }
        if (enemyDataHandler.selectedAction != null) {
            Debug.Log("This creature should have a null action " + enemy.name);
        }
//        Debug.Log("passed getting a path" + enemy.name);
        foreach (var action in enemyDataHandler.actions) {
            if (!action.check(enemy)) {
                //Debug.Log(action.name + " has failed check");
                continue;
            }
//            Debug.Log("setting next action as " + action.name);
            enemyDataHandler.selectedAction = action;
            break;
        }
        if (enemy.GetComponent<EnemyDataHandler>().selectedAction == null) {
            //Debug.Log("THIS ENEMY " + enemy + " HAS NO ACTION THIS TURN FOR SOME REASON");
            return;
        }

        
        //enemyDataHandler.selectedAction.unHighlight(enemy);
        enemyDataHandler.selectedAction.highlight(enemy,ScriptableObject.CreateInstance<Tile>());
        await Task.Delay(100);
    }

    public void debugger() {
        foreach (var enemy in aliveEnemies.items) {
            if (enemy.GetComponent<EnemyDataHandler>().getPath() == null) {
                continue;
            }
            List<Node2D> p = enemy.GetComponent<EnemyDataHandler>().getPath();
            // Debug.Log(p.Count);
            Node2D n = p[0];
            foreach (var t in p) {
                Debug.DrawLine(n.getWorldPosition(),t.getWorldPosition(),Color.magenta,1);
                n = t;
            }
        }

        foreach (var node in _grid2D.Grid) {
            if (node.getEnemy() != null) {
                Instantiate(enemyAtLocation, node.getWorldPosition(), quaternion.identity);
                //Debug.Log("enemy at location");
            }

            if (node.getClaimed()) {
                var temp = Instantiate(claimedAtLocation, node.getWorldPosition(), quaternion.identity);
               
               
            }
            
            //Debug.Log("spawning dummy");
        }
    }

    public void addToLateKill(GameObject go) {
        //Debug.Log(go);
        enemiesToLateKillList.Add(go);
    }
    
}


