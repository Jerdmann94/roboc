using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


[System.Serializable]
public class CombatManager : MonoBehaviour {

	private List<ScriptableObject> _enemiesToSpawn = new List<ScriptableObject>();
	public  EnemyList                 enemiesDeck;
	public  GameObject             enemyPrefab;
	public  Tilemap                tilemap;
	public GoRunTimeSet aliveEnemies;
	public TurnState combatState;
	public PlayerStateManager playerStateManager;
	public EnemyStateManager enemyStateManager;
	public Pathfinding2D pathfinder;
	public Grid2D grid2D;
	public MouseHandler mouseHandler;
	public GoRunTimeSet tileMapSet;
	
	
	


	private void Awake() {

		
		foreach (var variable in enemiesDeck.deck) {
			_enemiesToSpawn.Add(variable);
		}
		aliveEnemies.items = new List<GameObject>();
	}

	

	private void Start() {
		spawnEnemies();
		// SPAWNING PLAYER THEN ENEMIES;
		playerStateManager.initializePlayerState();
		pathfinder.initializePathFinder();
		enemyStateManager.initializeEnemyState();
		
		//STARTING COMBAT
		combatState.startState();
		
		
	}

	private void spawnEnemies() {
		for (int i = 0; i < _enemiesToSpawn.Count; i++) {
			var        tempPos = getEmptyGridPosition();
			GameObject enemy   = Instantiate(enemyPrefab, tempPos, Quaternion.identity);
			enemy.GetComponent<EnemyDataHandler>().setUpData((EnemySo) _enemiesToSpawn[i]);
			enemy.GetComponent<SpriteRenderer>().sortingOrder = 2;
			enemy.name = i.ToString();
			enemy.GetComponent<EnemyDataHandler>().name = i.ToString();
			aliveEnemies.items.Add(enemy);
		}
	}
	public Vector3 getEmptyGridPosition() {
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
			if (tilemap.WorldToCell(playerStateManager.player.transform.position) != pos) {
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
			else {
				//Debug.Log("this is player position " + pos);
			}
		}
		int     index = Random.Range(0, possiblePos.Count);
		Vector3 temp  = (Vector3) tilemap.GetCellCenterWorld((Vector3Int) possiblePos[index]);
		//Debug.Log(temp);
		return temp;
	}


	public void onGameStateChange() {
		switch (combatState.CurrentRound.name) {
			case"PlayerTurn":
				
				mouseHandler.resetTiles();
				playerStateManager.startState();
				break;
			case"EnemyTurn":
				mouseHandler.resetTiles();
				enemyStateManager.startState();
				break;
			default:
				break;
		}
	}
	
}