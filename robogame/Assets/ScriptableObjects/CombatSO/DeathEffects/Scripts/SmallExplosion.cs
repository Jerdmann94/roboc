using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new deathEffect", menuName = "DeathEffects/SmallExplosion")]
public class SmallExplosion : AbsDeathEffect {
	[SerializeField] private PlayerStatBlockSo stats; 
	internal override async Task execute(Vector3 pos) {
		if (possibleTargets.items.Count != 0) {
			possibleTargets.items.Clear();
		}
		GameObject form = Instantiate(formation, pos,Quaternion.identity);
		Destroy(form);
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		Grid2D grid = gridGameObject.items[0].GetComponent<Grid2D>();
		var removable = new List<Vector3Int>();
		foreach (var vector3Int in possibleTargets.items) {
			
			if (tilemap.WorldToCell(playerSet.items[0].transform.position) == vector3Int) {
				stats.health.takeDamage(damage);
				//Debug.Log("player should take explosion damage");
				break;
			}
			
			foreach (var enemy in aliveEnemies.items) {
				if (tilemap.WorldToCell(enemy.transform.position) == vector3Int) {
					enemy.GetComponent<EnemyDataHandler>().takeDamage(damage);
					break;
				}
			}
			foreach (var obstacle in aliveObstacles.items) {
				if (tilemap.WorldToCell(obstacle.transform.position) == vector3Int) {
					obstacle.GetComponent<ObstacleDataHandler>().takeDamage(damage);
					break;
				}
			}

			
		}
		await Task.Yield();
	}
}
