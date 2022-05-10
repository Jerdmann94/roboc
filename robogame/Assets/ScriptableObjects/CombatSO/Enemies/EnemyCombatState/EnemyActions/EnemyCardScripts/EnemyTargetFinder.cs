using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "EnemyCards/TargetFinder")]
public class EnemyTargetFinder : AbsAction {

	public GoRunTimeSet aliveEnemies;
	

	public override async  Task execute(GameObject enemy) {
		
		Pathfinding2D pathfinder = combatManagerSet.items.SingleOrDefault(obj => obj.name == "CombatManager")?.GetComponent<Pathfinding2D>();
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();		

		EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		// switch (enemyDataHandler.targetType.name) {
		// 	case "PlayerOnly":
		// 		if (enemyDataHandler.attackType.name == "Melee") {
		// 			enemyDataHandler.target = playerSet.items[0];
		// 		
		// 			pathfinder.FindPath(enemy.transform.position,playerSet.items[0].transform.position);
		// 		
		// 			enemy.GetComponent<EnemyDataHandler>().setPath(grid2D.path);
		// 			//Debug.Log(enemy.name + "'s path is  "  + enemyDataHandler.getPath().Count);
		// 			
		// 		}
		// 		else {
		// 			//do ranged stuff for path finding
		// 			findRangedPath(enemy);
		// 		}
		// 		
		// 		break;
		//
		// 			case "Closest":
		// 		break;
		// 	case "Weakest":
		// 		break;
		// 	default:
		// 		Debug.Log("Target Type for enemy " + enemy + " not given.");
		// 		break;
		// }
		await Task.Yield();
	}

	private void findRangedPath(GameObject enemy) {
		Pathfinding2D pathfinder = combatManagerSet.items.SingleOrDefault(obj => obj.name == "CombatManager")?.GetComponent<Pathfinding2D>();
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
		EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		var tilemap = tilemapSet.items[2].GetComponent<Tilemap>();

		List<List<Node2D>> paths = new List<List<Node2D>>();
		if (paths == null) throw new ArgumentNullException(nameof(paths));

		//GET PATHS IN 4 CARDINAL DIRECTION OF ENEMY'S CURRENT POSITION
		
		enemyDataHandler.target = playerSet.items[0];
		var pp = playerSet.items[0].transform.position;
		

		//SUPER MESSY BUT SHORT TERM FIX
		var position = enemy.transform.position;
		var gridpos = tilemap.WorldToCell(position);
		var pos = new Vector3Int(gridpos.x+1, gridpos.y, gridpos.z);
		if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos))!= null) {
			//Debug.Log(grid2D.NodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).getWorldPosition());
			pathfinder.FindPath(position,pp);
			//Debug.Log("after find path");
			paths.Add(grid2D.path);
		}
		
		
		pos = new Vector3Int(gridpos.x-1, gridpos.y, gridpos.z);
		if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)) != null) {
			//Debug.Log(grid2D.NodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).getWorldPosition());
			pathfinder.FindPath(position, pp);
			paths.Add(grid2D.path);
		}


		pos = new Vector3Int(gridpos.x, gridpos.y+1, gridpos.z);
		if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos))!= null) {
			//Debug.Log(grid2D.NodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).getWorldPosition());
			pathfinder.FindPath(position,pp);
			paths.Add(grid2D.path);
		}
		
		
		pos = new Vector3Int(gridpos.x, gridpos.y-1, gridpos.z);
		if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)) != null) {
			
			pathfinder.FindPath(position, pp);
			paths.Add(grid2D.path);
		}

		//FIND LONGEST PATH THAT STILL HITS PLAYER
		

		var longestpath = paths.OrderBy(x => x.Count).First();


		longestpath.Reverse();
		if (longestpath != null) enemy.GetComponent<EnemyDataHandler>().setPath(longestpath);
	}
	
	

	public override bool check(GameObject enemy) {
		
		// EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		// if (enemyDataHandler.target == null|| !aliveEnemies.items.Contains(enemyDataHandler.target)) {
		// 	return true;
		// }
		// return false;
		return true;
	}

	
}
