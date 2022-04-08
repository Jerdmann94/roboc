using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResetEnemyLocations : MonoBehaviour
{
	public static void resetEnemiesOnGrid(Grid2D grid,GoRunTimeSet enemies) {
		foreach (var node in grid.Grid) {
			node.setEnemy(null);
		}
		foreach (var enemy in enemies.items) {
			enemy.GetComponent<EnemyDataHandler>().selectedAction.highlight(enemy,new Tile());
		}
	}
}
