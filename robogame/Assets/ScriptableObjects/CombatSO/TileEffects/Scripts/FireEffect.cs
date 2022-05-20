using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "Tile Effects/Fire Effect")]

public class FireEffect : TileEffectSo {
	[SerializeField] private int damage;
	public override async Task execute(Vector3 vector3) {

		var tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		// var grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		// var node = grid2D.nodeFromWorldPoint(vector3);
		// if (node.getTileEffect()!= null) {
		// 	Debug.Log("this tile has an effect" + node.getTileEffect().name);
		// 	node.getTileEffect().reactWithFire();
		// }
		foreach (var enemy in aliveEnemies.items.Where(enemy => tilemap.WorldToCell(enemy.transform.position) ==tilemap.WorldToCell(vector3))) {
			var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
			enemyDataHandler.takeDamage(damage);
			await Task.Yield();
			return;
		}
		foreach (var obstacle in aliveObstacles.items.Where(obstacle => tilemap.WorldToCell(obstacle.transform.position) ==tilemap.WorldToCell(vector3))) {
			var obstacleDataHandler = obstacle.GetComponent<ObstacleDataHandler>();
			obstacleDataHandler.takeDamage(damage);
			await Task.Yield();
			return;
		}

		if (tilemap.WorldToCell(playerSet.items[0].transform.position) == tilemap.WorldToCell(vector3)) {
			stats.health.takeDamage(damage);
		}
		
		await Task.Yield();
	}

	public override async Task reactWithFire(Vector3 vector3) {
		await Task.Yield();
	}
}
