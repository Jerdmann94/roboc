using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "new Card", menuName = "EnemyCards/Attack 1 Square Card")]
public class EnemyBasicAttack : AbsAction
{
	public Tile attackTile;
	private EnemyDataHandler _enemyDataHandler;
	public PlayerStatBlockSo stats;
	public GoRunTimeSet aliveEnemies;
	
	public override async Task execute(GameObject enemy) {

		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		grid2D.setEnemyAtPosition(enemy);
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		//Debug.Log(tilemap.name);
		foreach (var node in _enemyDataHandler.highlightedNodes) {
		
			if (_enemyDataHandler.target.CompareTag("Player") && 
			    tilemap.WorldToCell(node.getWorldPosition()) == tilemap.WorldToCell(_enemyDataHandler.target.transform.position)) {
				stats.health.Value -= damage;
			}
			else {
				foreach (var otherEnemy in aliveEnemies.items.Where(
					         otherEnemy =>  tilemap.WorldToCell(otherEnemy.transform.position) ==  tilemap.WorldToCell(node.getWorldPosition()))) {
					otherEnemy.GetComponent<EnemyDataHandler>().takeDamage(damage);
					Debug.Log(_enemyDataHandler.gameObject.name + " didnt hit player but did attack");
				}
			}
			await Task.Delay(300);
		}

		
		

		
		//Debug.Log("Attacking not implemented yet");
	}

	


	public override bool check(GameObject enemy) {
		_enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		getPathForTargetType(enemy);
		return meleeCheck(enemy);
	}

	public override void highlight(GameObject enemy, Tile tile) {

		base.highlight(enemy, attackTile);

	}
	

	

	

	private bool meleeCheck(GameObject enemy) {
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		_enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		if (tilemap.WorldToCell(_enemyDataHandler.target.transform.position)
		    == tilemap.WorldToCell(_enemyDataHandler.getPath()[0].getWorldPosition())) {
			Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
			grid2D.setEnemyAtPosition(enemy);
			return true;
		}

		return false;
	}
	public override void resetWithNewPosition(GameObject enemy, Vector3 dir, Tile tile) {
		base.resetWithNewPosition(enemy,dir,attackTile);
		
	}
	
}
