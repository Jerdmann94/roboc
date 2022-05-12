using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Sets;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

using static ResetEnemyLocations;

[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/PushingAttack")]
public class PushingAttack : CardAbs {
	public Vector3Event attackEvent;
	public GoRunTimeSet aliveEnemies;
	public GoRunTimeSet combatManagerSet;
	public GoRunTimeSet aliveObstacles;

	public override void execute() {
		Pathfinding2D pathfinder = combatManagerSet.items.SingleOrDefault(obj => obj.name == "CombatManager")?.GetComponent<Pathfinding2D>();
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
		targetPos.items.ForEach(pos => {
			var tilemap = tilemapSet.items[2].GetComponent<Tilemap>();
			Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
			attackEvent.emit(worldPos);
			foreach (var enemy in aliveEnemies.items) {
				if (enemy.transform.position != worldPos) continue;
				Debug.Log("inside enemies");
				var dir = getDirection(worldPos);
				var worldtogridpos = new Vector3Int((int) (pos.x + dir.x), (int) (pos.y +dir.y), 0);
				//Debug.Log(dir + " " + worldtogridpos);
				if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(worldtogridpos)).obstacle != null) {

					// STUN DAMAGE PROBABLY NEEDS TO BE ADJUSTED
					var obs = grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(worldtogridpos)).obstacle;
					obs.setStun(1);
					enemy.GetComponent<EnemyDataHandler>().setStun(1);
					return;
				}

				if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(worldtogridpos)).getEnemy() != null) {
					var ene = grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(worldtogridpos)).getEnemy()
						.GetComponent<EnemyDataHandler>();
					ene.setStun(1);
					enemy.GetComponent<EnemyDataHandler>().setStun(1);
					return;
				}
				
				if (tilemap.GetTile<Tile>(worldtogridpos).name == null) {
					enemy.GetComponent<EnemyDataHandler>().setStun(1);
					Debug.Log("pushed enemy off tilemap");
					return;
				}

				grid2D.nodeFromWorldPoint(enemy.transform.position).setEnemy(null);
				enemy.transform.position = tilemap.GetCellCenterWorld(worldtogridpos);
				if (enemy.GetComponent<EnemyDataHandler>().selectedAction != null) {
					enemy.GetComponent<EnemyDataHandler>().selectedAction.resetWithNewPosition(enemy, dir, new Tile());

				}



				//resetEnemiesOnGrid(grid2D,aliveEnemies);
			}

			Debug.Log(tilemap.WorldToCell(pos));
			Debug.Log(aliveObstacles.items.Count);
			foreach (var obstacle in aliveObstacles.items) {
				Debug.Log(tilemap.WorldToCell(obstacle.gameObject.transform.position) + pos);
				if (tilemap.WorldToCell(obstacle.gameObject.transform.position) != pos) continue;
				Debug.Log("inside obstacles");
				var dir = getDirection(worldPos);
				var worldtogridpos = new Vector3Int((int) (pos.x + dir.x), (int) (pos.y + dir.y), 0);

				var obstacleDataHandler = obstacle.GetComponent<ObstacleDataHandler>();
				if (tilemap.GetTile<Tile>(worldtogridpos) == null ) {
					obstacleDataHandler.setStun(1);
					Debug.Log("pushed enemy off tilemap");
					return;
				}

				if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(worldtogridpos)).obstacle != null) {
					var obs = grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(worldtogridpos)).obstacle;
					obs.setStun(obstacleDataHandler.damageWhenPushed);
					obstacle.GetComponent<ObstacleDataHandler>().setStun(obstacleDataHandler.damageWhenPushed);
					Debug.Log("obstacle pushed into obstacle");
					return;
				}

				if (grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(worldtogridpos)).getEnemy() != null) {
					var ene = grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(worldtogridpos)).getEnemy()
						.GetComponent<EnemyDataHandler>();
					ene.setStun(obstacleDataHandler.damageWhenPushed);
					obstacle.GetComponent<ObstacleDataHandler>().setStun(obstacleDataHandler.damageWhenPushed);
					Debug.Log("obstacle pushed into enemy");
					return;
				}

				grid2D.nodeFromWorldPoint(obstacle.transform.position).setObstacle(null);
				obstacle.transform.position = tilemap.GetCellCenterWorld(worldtogridpos);
				Debug.Log("obstacle should have new position");
			}

		});
	}




	private Vector3 getDirection(Vector3 worldPos) {
		var pos = new Vector3();
		Vector3 vec = worldPos - playerSet.items[0].transform.position;
		var x = vec.x;
		var y = vec.y;
		var z = vec.z;
		if (x < 0 && y < 0) {
			//Debug.Log("both less than 0");
			// do absolute check
			if (Mathf.Abs(y) > Mathf.Abs(x)) {


				if (y == 0) {
					y = 1;
				}

				y /= Mathf.Abs(y);
				x = 0;

			}
			else {

				if (x == 0) {
					x = 1;
				}

				x /= Mathf.Abs(x);
				y = 0;

			}
		}
		else if (x > 0 && y < 0) {
			//Debug.Log("x > 0 and y< 0");
			if (y < x) {
				if (y == 0) {
					y = 1;
				}

				y /= Mathf.Abs(y);
				x = 0;

			}
			else {
				if (x == 0) {
					x = 1;
				}

				x /= Mathf.Abs(x);
				y = 0;
			}
		}
		else if (x < 0 && y > 0) {
			//Debug.Log("x < 0 and y> 0");
			if (y > x) {
				if (y == 0) {
					y = 1;
				}

				y /= Mathf.Abs(y);
				x = 0;

			}
			else {

				if (x == 0) {
					x = 1;
				}

				x /= Mathf.Abs(x);
				y = 0;

			}
		}
		else {
			//Debug.Log("x > 0 and y> 0");
			if (y > x) {
				if (y == 0) {
					y = 1;
				}

				y /= Mathf.Abs(y);
				x = 0;

			}
			else {

				if (x == 0) {
					x = 1;
				}

				x /= Mathf.Abs(x);
				y = 0;

			}

			
		}
		return new Vector3(x, y, 0);
	}

}
