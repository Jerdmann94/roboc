using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "Tile Effects/Enter Damage Tile")]

public class DamageOnEnter : TileEffectSo {
	[SerializeField] private int damage;
	

	public override async Task execute(Vector3 vector3) {
		//Debug.Log("executing tile effect");
		var tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
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

		stats.health.Value -= damage;
		await Task.Yield();
	}

	public override async Task reactWithFire(Vector3 vector3) {
		//throw new System.NotImplementedException();
		await Task.Yield();
	}
}
