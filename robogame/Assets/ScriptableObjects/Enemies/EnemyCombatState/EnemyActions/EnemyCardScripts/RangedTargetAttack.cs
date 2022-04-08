using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "EnemyCards/RangedTargetAttack")]
public class RangedTargetAttack : AbsAction {
	private Pathfinding2D pathfinder;
	public Tile attackTile;
	[SerializeField] private GameObject formation;
	[SerializeField] private Vector3IntSet possibleTargets;
	[SerializeField] private PlayerStatBlockSo stats;
	[SerializeField] private GoRunTimeSet aliveEnemies;
	[SerializeField] private int timer;
	private int i;
	public override async Task execute(GameObject enemy) {
		var _enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		grid2D.setEnemyAtPosition(enemy);
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		//Debug.Log(tilemap.name);
		foreach (var node in _enemyDataHandler.highlightedNodes) {
			if (node.getEnemy()!= null) {
				node.getEnemy().GetComponent<EnemyDataHandler>()
					.takeDamage(damage);
			}
			else if (node.getWorldPosition() == playerSet.items[0].transform.position) {
				stats.health.Value -= damage;
			}
			

			
		}

		foreach (var node in _enemyDataHandler.highlightedNodes) {
			foreach (var e in aliveEnemies.items) {
				if (tilemap.WorldToCell(node.getWorldPosition()) ==
				                        tilemap.WorldToCell(e.transform.position)) {
					Debug.Log("found an Enemy in formation");
				}
			}
		}
		await Task.Delay(100);
	}

	public override bool check(GameObject enemy) {
		getPathForTargetType(enemy);
		i++;
		if (i <= timer) return false;
		i = 0;
		pathfinder = combatManagerSet.items[1].GetComponent<Pathfinding2D>();
		var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		var target = enemyDataHandler.target;
		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		//GET PATH FROM PATHERFINDER THAT IGNORES ALL TERRAIN,
		//Debug.Log(pathfinder + " " + target);
		var path = pathfinder.findPathForVision(enemy.transform.position, target.transform.position);
		//USE PATH TO CHECK EACH TILE FOR OBSTACLE
		Node2D n = path[0];
		foreach (var t in path) {
			//Debug.DrawLine(n.getWorldPosition(), t.getWorldPosition(), Color.blue, 10);
			n = t;
		}

		foreach (var node in path) {
			if (node.obstacle) {
				return false;
			}
		}
		//IF PATH CONTAINS NO OBSTACLES THEN THERE IS A VISIBLE LINE TO TARGET
		//SPAWN FORM FOR HIGHLIGHT

		var form = Instantiate(formation, target.transform.position, Quaternion.identity);
		Destroy(form);
		foreach (var tilePos in possibleTargets.items) {
			enemyDataHandler.highlightedNodes.Add(grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilePos)));

		}

		return true;

	}

	public override void highlight(GameObject enemy, Tile tile) {
		
		EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		//RESPAWN FORM TO RESET ENEMY TARGET TILES
		var form = Instantiate(formation, enemyDataHandler.target.transform.position, Quaternion.identity);
		Destroy(form);
		
		
        
        
		GameObject gridOwner = combatManagerSet.items[0];
		Grid2D grid2D = gridOwner.GetComponent<Grid2D>();
        
		Tilemap tilemapForEnemies = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
		Tilemap tilemap  = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		//Debug.Log(enemyDataHandler.highlightedNodes.Count);
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
	public override void resetWithNewPosition(GameObject enemy, Vector3 dir, Tile tile) {
		unHighlight(enemy);
		Tilemap tilemapForEnemies = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
		Tilemap tilemap  = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		//RESPAWN FORM TO RESET ENEMY TARGET TILES
		var ePosInt = tilemap.WorldToCell(enemyDataHandler.target.transform.position);
		Vector3Int tempPos = new Vector3Int(
			(int) (dir.x + ePosInt.x),
			(int) (dir.y + ePosInt.y),
			0);
		possibleTargets.items.Clear();
        enemyDataHandler.highlightedNodes.Clear();
		var form = Instantiate(formation, tilemap.GetCellCenterWorld(tempPos), Quaternion.identity);
		Destroy(form);
		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		foreach (var tilePos in possibleTargets.items) {
			enemyDataHandler.highlightedNodes.Add(grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilePos)));

		}
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
}