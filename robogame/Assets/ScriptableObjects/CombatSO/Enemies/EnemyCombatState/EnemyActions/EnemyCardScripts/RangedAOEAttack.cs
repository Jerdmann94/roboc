using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MyBox;
using ScriptableObjects.Sets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "EnemyCards/AOEAttack")]
public class RangedAOEAttack : AbsAction {
	[SerializeField] private GameObject formation;
	[SerializeField] private Vector3IntSet possibleTargets;
	[SerializeField] private Tile attackTile;
	[SerializeField] private PlayerStatBlockSo stats;
	[SerializeField] private MoveType moveType;
	[SerializeField] private bool directional;
	[SerializeField] private bool nonStraightForm;
	[ConditionalField("nonStraightForm")]
	[SerializeField] private GameObject leftRightForm;
	[ConditionalField("nonStraightForm")]
	[SerializeField] private GameObject upDownForm;
	[SerializeField] private bool ignoreObstacle;
	[SerializeField] private bool spawnTileEffect;
	[ConditionalField("spawnTileEffect")]
	[SerializeField] private GameObject spawnableTileEffect;
	[ConditionalField("spawnTileEffect")]
	[SerializeField] private TileEffectSo tileEffect;
	[SerializeField] private bool shouldThisMove;
	[ConditionalField("shouldThisMove")]
	[SerializeField] private int moveAmount;
	[ConditionalField("shouldThisMove")]
	[SerializeField] private Tile moveTile;
	[ConditionalField("shouldThisMove")]
	[SerializeField] private Vector3Event moveEmitter;
	[SerializeField] private bool doesThisUseATimer;
	[ConditionalField("doesThisUseATimer")]
	[SerializeField] private int timer;
	private int counter;


	public override async Task execute(GameObject enemy) {
		var _enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
		grid2D.setEnemyAtPosition(enemy);
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		counter = 0;
		foreach (var node in _enemyDataHandler.highlightedNodes) {
			
			
			if (shouldThisMove) { //IS THIS A MOVING ATTACK
				if (node.getEnemy()!= null) {
					node.getEnemy().GetComponent<EnemyDataHandler>().setStun(damage);
					enemy.GetComponent<EnemyDataHandler>().setStun(damage);
					
					break;
				}

				if (node.getObstacle()!= null) {
					node.getObstacle().setStun(damage);
					enemy.GetComponent<EnemyDataHandler>().setStun(damage);
					break;
				}
				
				var position = enemy.transform.position;
				grid2D.removeEnemyAtPosition(position);
				grid2D.setEnemyAtPosition(enemy,node.getWorldPosition());
				position = node.getWorldPosition(); //MOVING THE ENEMY TO THE NEXT POSITION
				enemy.transform.position = position;
				moveEmitter.emit(position);
			}
			if (spawnTileEffect) { //TILE EFFECT STUFF - SHOULD BE AFTER MOVEMENT
				
				var temp = Instantiate(spawnableTileEffect, node.getWorldPosition(), quaternion.identity);
				var handler = temp.GetComponent<TileEffectHandler>();
				handler.setupData(tileEffect);
				handler.execute();
			}
			// if (_enemyDataHandler.target.CompareTag("Player") &&
			//     tilemap.WorldToCell(node.getWorldPosition()) ==
			//     tilemap.WorldToCell(_enemyDataHandler.target.transform.position)) {
			// 	stats.health.Value -= damage;
			// }
			if (tilemap.WorldToCell(playerSet.items[0].transform.position) == tilemap.WorldToCell(node.getWorldPosition())) {
				stats.health.Value -= damage;
				
			}
			else if (node.getEnemy()!= null && node.getEnemy()!= enemy) {
				node.getEnemy().GetComponent<EnemyDataHandler>()
					.takeDamage(damage);
			}

			await Task.Delay(100);
		}
	}

	public override async Task<bool> check(GameObject enemy) {
		getPathForTargetType(enemy,moveType,ignoreObstacle);

		possibleTargets.items.Clear();
		
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		//determine cardinal toward target
		
		var position = enemy.transform.position;
		var targetPos = enemy.GetComponent<EnemyDataHandler>().target.transform.position;
		
		var dir = Cardinal.getCardinalVector3(position, targetPos);
		//spawn attack formation, check if target is within
		
		
		var rotation = enemy.transform.rotation;
		var temp = rotation * dir;
		//NEED TO SPAWN FORM 1 SPACE IN THE DIRECTION OF THE CARDINAL STILL    
		//SPAWNING OBJECT BASED ON DIRECTIONAL BOOL
		var tempForm = getFormation(Cardinal.getCardinalDirection(position,targetPos));
		var form = Instantiate(tempForm,   directional  ? 
			position + Cardinal.getCardinalVector3(Cardinal.getCardinalDirection(position,targetPos ))
			:position , temp);
		Destroy(form);
		var target = enemyDataHandler.target;
		
		//return true if formation contains target
		var returnable = false;
		
		foreach (var tilePos in possibleTargets.items) {
			if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilePos)) == null) {
				continue;
			}
			enemyDataHandler.highlightedNodes.Add(grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilePos)));
			if (tilemap.WorldToCell(target.transform.position) ==tilePos) {
				returnable = true;
				//Debug.Log(tilePos + "  " + tilemap.WorldToCell(target.transform.position));
			}
		}

		if (shouldThisMove) {
			foreach (var tilePos in possibleTargets.items) {
				if (tilemap.GetTile(tilePos)!= null) {
					if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilePos)).getEnemy()== null ) {
						returnable = true;

					}
				}
				else {
					returnable = false;
					break;
				}
				
			}
		}
		if (counter < timer && doesThisUseATimer) {
			counter++;
			returnable = false;
		}
		possibleTargets.items.Clear();
		if (!returnable) {
			enemyDataHandler.highlightedNodes.Clear();
		}

		await Task.Yield();
		return returnable;
	}
	public override async Task highlight(GameObject enemy, Tile tile) {
		
		//RESPAWN FORM TO RESET ENEMY TARGET TILES
		var position = enemy.transform.position;
		var targetPos = enemy.GetComponent<EnemyDataHandler>().target.transform.position;
		var dir = Cardinal.getCardinalVector3(position, targetPos);
		var rotation = enemy.transform.rotation;
		var temp = rotation * dir;
		var tempForm = getFormation(Cardinal.getCardinalDirection(position,targetPos));
		var form = Instantiate(tempForm,   directional  ? 
			position + Cardinal.getCardinalVector3(Cardinal.getCardinalDirection(position,targetPos ))
			:position ,directional ? temp : Quaternion.identity);
		Destroy(form);
		
		
		EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        
        
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
        
		Tilemap tilemapForEnemies = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
		Tilemap tilemap  = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		var counter = 0;
		foreach (var node in enemyDataHandler.highlightedNodes) {
			var tilePos = tilemap.WorldToCell(node.getWorldPosition());
			counter++;
			if (counter == moveAmount && shouldThisMove) {
				tilemapForEnemies.SetTile(tilePos, moveTile);
			}
			else {
				tilemapForEnemies.SetTile(tilePos, attackTile);
			}
			if (tilemap.WorldToCell(playerSet.items[0].transform.position) != tilePos) {
				grid2D.setClaimedAtPosition(enemy, tilemap.GetCellCenterWorld(tilePos));
			}
		}

		await Task.Yield();

	}

	public override void resetWithNewPosition(GameObject enemy, Vector3 movedDir, Tile tile) {
		unHighlight(enemy);
		//RESPAWN FORM TO RESET ENEMY TARGET TILES
		EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		GameObject gridOwner = combatManagerSet.items[0];
		Grid2D grid2D = gridOwner.GetComponent<Grid2D>();
		Tilemap tilemapForEnemies = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
		Tilemap tilemap  = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();

		var position = enemy.transform.position;
		var targetPos = enemy.GetComponent<EnemyDataHandler>().target.transform.position;
		var dir = Cardinal.getCardinalVector3(position, targetPos);
		var rotation = enemy.transform.rotation;
		var temp = rotation * dir;
		var ePosInt = tilemap.WorldToCell(position);
		Vector3Int tempPos = new Vector3Int(
			(int) (movedDir.x + ePosInt.x),
			(int) (movedDir.y + ePosInt.y),
			0);
		var vector3Temp = tilemap.GetCellCenterWorld(tempPos);
		possibleTargets.items.Clear();
		enemyDataHandler.highlightedNodes.Clear();
		var tempForm = getFormation(Cardinal.getCardinalDirection(position,targetPos));

		var form = Instantiate(tempForm,   directional  ? 
			vector3Temp + Cardinal.getCardinalVector3(Cardinal.getCardinalDirection(vector3Temp,targetPos ))
			:position ,directional ? temp : Quaternion.identity);
		Destroy(form);
		foreach (var tilePos in possibleTargets.items) {
			enemyDataHandler.highlightedNodes.Add(grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilePos)));

		}
		
		
		//RE HIGHLIGHT AND SET TAKE TILES
		Debug.Log(enemyDataHandler.highlightedNodes.Count);
		foreach (var node in enemyDataHandler.highlightedNodes) {
			var tilePos = tilemap.WorldToCell(node.getWorldPosition());
		
			tilemapForEnemies.SetTile(tilePos, attackTile);
			
			if (playerSet.items[0] == null) {
				Debug.Log("player is null");
				return;
			}
			if (tilemap.WorldToCell(playerSet.items[0].transform.position) != tilePos) {
				grid2D.setClaimedAtPosition(enemy, tilemap.GetCellCenterWorld(tilePos));
			}
            
			
		}
	}

	private GameObject getFormation(CardinalDirection cardinalDirection) {
		var form = formation;
		if (directional && nonStraightForm) {
			if (cardinalDirection == CardinalDirection.NorthWest || cardinalDirection == CardinalDirection.SouthEast) {
				form = upDownForm;
			}
			else {
				form = leftRightForm;
			}
		}

		//SDebug.Log(form.name + " "+ cardinalDirection);
		return form;
	}
}
