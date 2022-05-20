using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "Tile Effects/Sludge")]

public class SludgeEffect : TileEffectSo {
	[SerializeField] private GameObject formation;
	[SerializeField] private Vector3IntSet possibleTargets;
	[SerializeField] private GameObject tileEffectPrefab;
	[SerializeField] private TileEffectSo reactionToFireEffect;
	[SerializeField] private CardUIValue moveCard;

	private Vector3Int[] checkArray = {
		new Vector3Int(0,-1,0),
		new Vector3Int(0,1,0),
		new Vector3Int(1,0,0),
		new Vector3Int(-1,0,0)
	};
	
	
	public override async Task execute(Vector3 vector3) {
		
		//Debug.Log("doing sludge effect");
		var tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		foreach (var enemy in aliveEnemies.items) {
			if (tilemap.WorldToCell(enemy.transform.position) != tilemap.WorldToCell(vector3)) continue;
			var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
			if (enemyDataHandler.labelBases != null)
				foreach (var labelBase in enemyDataHandler.labelBases) {
					if (labelBase.labelType == LabelType.SludgeDrinker) {
						enemyDataHandler.takeDamage(-4);
					}
				}

			await Task.Yield();
			//Debug.Log("enemy on sludge");
			return;
		}
		if (aliveObstacles.items.Any(obstacle => tilemap.WorldToCell(obstacle.transform.position) ==tilemap.WorldToCell(vector3))) {
			await Task.Yield();
			Debug.Log("obstacle on sludge");
			return;
		}
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();var node = grid2D.nodeFromWorldPoint(vector3);
		
		if (tilemap.WorldToCell(node.getWorldPosition()) == tilemap.WorldToCell(playerSet.items[0].transform.position)) {
			var cc = GameObject.Find("CombatManager").GetComponent<CardConfirmed>();
			cc.changeEnergyValue(moveCard,0);
			cc.checkWhichCardsCanBePlayed();
			stats.health.takeDamage(-4);;
		}
		
		await Task.Yield();
	}

	public override async Task reactWithFire(Vector3 vector3) {
		//Debug.Log("inside sludge react with fire");
		var tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();var toExplode = new List<Node2D>();
		var toCheck = new List<Node2D>();
		toCheck.Add(grid2D.nodeFromWorldPoint(vector3));
		//Debug.Log(toCheck.Count);
		var count = 0;
		while (toCheck.Count > 0) {
			count++;
			if (count > 50) {
				break;
			}
			var node = toCheck[0];
			if (toCheck[0].getTileEffect().name != "Sludge") continue;
			toExplode.Add(node);
			toCheck.Remove(node);
			var intpos = tilemap.WorldToCell(node.getWorldPosition());
			foreach (var check in checkArray) {
	
				var temp = intpos + check;
				var newNode = grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(temp));
				if (newNode != null) {
					if (newNode.getTileEffect()!= null) {
						if (newNode.getTileEffect().name == "Sludge"&& !toCheck.Contains(newNode) && !toExplode.Contains(newNode)) {
							toCheck.Add(newNode);
						}
					}
					
				}
				
			}
		}

		Debug.Log(toExplode.Count);
		foreach (var node in toExplode) {
			// CHECK IF AN OBSTACLE IS ON NODE
			if (node.obstacle!= null) {
				node.obstacle.takeDamage(reactWithFireDamage);
			}
			 	//CHECK IF NODE HAS AN ENEMY
			else if (node.getEnemy() != null) {
				node.getEnemy().GetComponent<EnemyDataHandler>().takeDamage(reactWithFireDamage);
			}
			else if (tilemap.WorldToCell(node.getWorldPosition()) == tilemap.WorldToCell(playerSet.items[0].transform.position)) {
				stats.health.takeDamage(reactWithFireDamage);
			}
			//REMOVE THIS EFFECT FROM NODE
			var pos =node.getWorldPosition();
			node.getTileEffect().doDeath();
			//SPAWN NEW EFFECT WHICH AUTOMATICALLY ADDS ITSELF TO THE NODE
			if (tilemap.WorldToCell(node.getWorldPosition()) != tilemap.WorldToCell(vector3)) {
				var effectBase = Instantiate(tileEffectPrefab, pos, quaternion.identity);
				effectBase.GetComponent<TileEffectHandler>().setupData(reactionToFireEffect);
			}
		}
		await Task.Yield();
	}
}
