using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


[System.Serializable]
public class CombatManager : MonoBehaviour {
    public static CombatManager combatManager = null;
    private       ArrayList     enemiesToSpawn = new ArrayList();
    public       ArrayList     aliveEnemies = new ArrayList();
    public        DeckSO        enemiesDeck;
    public        GameObject    enemyPrefab;
    public        Tilemap       tilemap;


    private void Awake() {
        combatManager = this;
        foreach (var VARIABLE in enemiesDeck.deck) {
            enemiesToSpawn.Add(VARIABLE);
        }
        
    }

    private void Start() {
        spawnEnemies();
    }

    private void spawnEnemies() {
        for (int i = 0; i < enemiesToSpawn.Count; i++) {
            var        tempPos = getEmptyGridPosition();
            GameObject enemy   = Instantiate(enemyPrefab, tempPos, Quaternion.identity);
            enemy.GetComponent<enemyDataHandler>().setUpEnemy((EnemySO)enemiesToSpawn[i]);
            aliveEnemies.Add(enemy);
            
        }
        
    }

    private Vector3 getEmptyGridPosition() {
        ArrayList allPos = new ArrayList();
        ArrayList possiblePos = new ArrayList();
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {   
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3    place      = tilemap.CellToWorld(localPlace);
            if (tilemap.GetTile<Tile>(localPlace)!= null) {
                allPos.Add(localPlace);
            }
        }
        
       
        Debug.Log(allPos.Count);

        foreach (Vector3Int pos in allPos) {
            bool shouldAdd = true;
            if (MouseHandler.mouseHandler.player.transform.position != pos) {
                
                foreach (GameObject e in aliveEnemies) {
                    var eGridPos = tilemap.WorldToCell(e.transform.position);
                    if (eGridPos == pos) {
                        shouldAdd = false;
                        Debug.Log("enemy at this position already");
                    }
                }

                if (shouldAdd) {
                   
                    possiblePos.Add(pos);
                    
                }
            }
        }


        int     index = Random.Range(0, possiblePos.Count);
        Vector3 temp  = (Vector3) tilemap.GetCellCenterWorld((Vector3Int)possiblePos[index]);
        return new Vector3(temp.x,temp.y,2);
    }
}


