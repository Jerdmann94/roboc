using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/PushingAttack")]
public class PushingAttack : CardAbs
{
	public Vector3Event  attackEvent;
	public GORunTimeSet aliveEnemies;
	public GORunTimeSet combatManagerSet;
	public GORunTimeSet aliveObstacles;
	private const float TOLERANCE = .5f;
	public override void Execute() {
		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		targetPos.items.ForEach(pos => {
			var tilemap = tilemapSet.items[0].GetComponent<Tilemap>();
			Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
			attackEvent.emit(worldPos);

			
			foreach (var enemy in aliveEnemies.items) {
				if (enemy.transform.position == worldPos) {
					Vector3 vec = worldPos -playerSet.items[0].transform.position   ;
					var x = vec.x;
					var y = vec.y;
					var z = vec.z;
					if (x < 0 && y < 0) {
						//Debug.Log("both less than 0");
						// do absolute check
						if (Mathf.Abs(y)> Mathf.Abs(x)) { 
						
						
							if (y ==0) {
								y = 1;
							}
							y /= Mathf.Abs(y);
							x = 0;
						
						}
						else {
						
							if (x ==0) {
								x= 1;
							}
							x /= Mathf.Abs(x);
							y = 0;
						
						}
					}
					else if (x> 0 && y < 0) {
						//Debug.Log("x > 0 and y< 0");
						if (y< x) {
							if (y ==0) {
								y = 1;
							}
							y /= Mathf.Abs(y);
							x = 0;
						
						}
						else {
							if (x ==0) {
								x= 1;
							}
							x /= Mathf.Abs(x);
							y = 0;
						}
					}
					else if (x<0 && y >0)
					{
						//Debug.Log("x < 0 and y> 0");
						if (y> x) {
							if (y ==0) {
								y = 1;
							}
							y /= Mathf.Abs(y);
							x = 0;
						
						}
						else {
						
							if (x ==0) {
								x= 1;
							}
							x /= Mathf.Abs(x);
							y = 0;
						
						}
					}
					else {
						//Debug.Log("x > 0 and y> 0");
						if (y> x) {
							if (y ==0) {
								y = 1;
							}
							y /= Mathf.Abs(y);
							x = 0;
						
						}
						else {
						
							if (x ==0) {
								x= 1;
							}
							x /= Mathf.Abs(x);
							y = 0;
						
						}
					}
					
					
					//Debug.Log(temp);
					var dir = new Vector3(x, y, 0);
					var worldtogridpos = new Vector3Int((int) (pos.x + x), (int) (pos.y + y), 0);
					//Debug.Log(dir + " " + worldtogridpos);
					if (grid2D.NodeFromWorldPoint(tilemap.GetCellCenterWorld(worldtogridpos)).obstacle) {
						
						foreach (var obs in aliveObstacles.items) {
							if (Math.Abs(obs.transform.position.x - tilemap.GetCellCenterWorld(worldtogridpos).x) > TOLERANCE &&  Math.Abs(obs.transform.position.y - tilemap.GetCellCenterWorld(worldtogridpos).y) > TOLERANCE) {
								//Debug.Log(obs.transform.position + " "  + tilemap.GetCellCenterWorld(worldtogridpos));
								continue;
							}
							obs.GetComponent<ObstacleDataHandler>().setStun(enemy.GetComponent<EnemyDataHandler>().attack);
							enemy.GetComponent<EnemyDataHandler>().setStun(obs.GetComponent<ObstacleDataHandler>().attack);
							
							return;
						}
					}
					enemy.transform.position = tilemap.GetCellCenterWorld(worldtogridpos);
					enemy.GetComponent<EnemyDataHandler>().selectedAction.resetWithNewPosition(enemy,dir,new Tile());
				}
			}
			
		});
	}

	
}
