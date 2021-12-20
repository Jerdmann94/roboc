using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


[System.Serializable]
public class CombatManager : MonoBehaviour {

	private List<ScriptableObject> enemiesToSpawn = new List<ScriptableObject>();
	public  DeckSO                 enemiesDeck;
	public  GameObject             enemyPrefab;
	public  Tilemap                tilemap;
	public  GORunTimeSet aliveEnemies;
	public MouseHandler mouseHandler;
	public TurnState combatState;
	public PlayerStateManager playerStateManager;
	public EnemyStateManager enemyStateManager;
	
	


	private void Awake() {
	
		foreach (var VARIABLE in enemiesDeck.deck) {
			enemiesToSpawn.Add(VARIABLE);
		}
		aliveEnemies.items = new List<GameObject>();
	}

	private void Start() {
		spawnEnemies();
		
		
		
		//STARTING COMBAT
		combatState.startState();
		
		
	}

	private void spawnEnemies() {
		for (int i = 0; i < enemiesToSpawn.Count; i++) {
			var        tempPos = getEmptyGridPosition();
			GameObject enemy   = Instantiate(enemyPrefab, tempPos, Quaternion.identity);
			enemy.GetComponent<enemyDataHandler>().setUpEnemy((EnemySO) enemiesToSpawn[i]);
			enemy.GetComponent<SpriteRenderer>().sortingOrder = 2;
			aliveEnemies.items.Add(enemy);
		}
	}
	private Vector3 getEmptyGridPosition() {
		ArrayList allPos      = new ArrayList();
		ArrayList possiblePos = new ArrayList();
		foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
			Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
			Vector3    place      = tilemap.CellToWorld(localPlace);
			if (tilemap.GetTile<Tile>(localPlace) != null) {
				allPos.Add(localPlace);
			}
		}
		foreach (Vector3Int pos in allPos) {
			bool shouldAdd = true;
			if (playerStateManager.player.transform.position != pos) {
				foreach (GameObject e in aliveEnemies.items) {
					var eGridPos = tilemap.WorldToCell(e.transform.position);
					if (eGridPos == pos) {
						shouldAdd = false;
						//Debug.Log("enemy at this position already");
					}
				}
				if (shouldAdd) {
					possiblePos.Add(pos);
				}
			}
		}
		int     index = Random.Range(0, possiblePos.Count);
		Vector3 temp  = (Vector3) tilemap.GetCellCenterWorld((Vector3Int) possiblePos[index]);
		return temp;
	}


	public void onGameStateChange() {
		switch (combatState.CurrentRound.name) {
			case"PlayerTurn":
				playerStateManager.startState();
				break;
			case"EnemyTurn":
				enemyStateManager.startState();
				break;
			default:
				break;
		}
	}
}