using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class EnemyTargetFinder : AbsAction {

	public GORunTimeSet playerSet;
	public GORunTimeSet aliveEnemies;
	

	public override void Execute(GameObject enemy) {
		
		Pathfinding2D pathfinder = combatManagerSet.items[1].GetComponent<Pathfinding2D>();
		
		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		// int i = 0;
		// foreach (var node in grid2D.Grid) {
		// 	if (node.enemy != null) {
		// 		i++;
		// 	}
		// }

		// Debug.Log(i);

		EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		switch (enemyDataHandler.targetType.name) {
			case "PlayerOnly":
				enemyDataHandler.target = playerSet.items[0];
				pathfinder.FindPath(enemy.transform.position,playerSet.items[0].transform.position);
				enemy.GetComponent<EnemyDataHandler>().path = grid2D.path;
				break;
			case "Closest":
				break;
			case "Weakest":
				break;
			default:
				Debug.Log("Target Type for enemy " + enemy + " not given.");
				break;
		}
	}

	public override bool Check(GameObject enemy) {
		
		EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		if (enemyDataHandler.target == null|| !aliveEnemies.items.Contains(enemyDataHandler.target)) {
			return true;
		}
		return false;
	}

	
}
