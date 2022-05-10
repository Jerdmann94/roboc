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
	[SerializeField] private Tile attackTile;
	private EnemyDataHandler _enemyDataHandler;
	[SerializeField] private PlayerStatBlockSo stats;
	[SerializeField] private GoRunTimeSet aliveEnemies;
	[SerializeField] private MoveType moveType;
	[SerializeField] private bool ignoreObstacle;
	
	
	
	public override async Task execute(GameObject enemy) {

		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
		grid2D.setEnemyAtPosition(enemy);
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		foreach (var node in _enemyDataHandler.highlightedNodes) {
		
			//Debug.Log(_enemyDataHandler.highlightedNodes.Count);
			Debug.Log(tilemap.WorldToCell(node.getWorldPosition()) + " " + tilemap.WorldToCell(playerSet.items[0].transform.position));
			if (tilemap.WorldToCell(node.getWorldPosition()) == tilemap.WorldToCell(playerSet.items[0].transform.position)) {
				stats.health.Value -= damage;
			}
			else {
				foreach (var otherEnemy in aliveEnemies.items.Where(
					         otherEnemy =>  tilemap.WorldToCell(otherEnemy.transform.position) ==  tilemap.WorldToCell(node.getWorldPosition()))) {
					otherEnemy.GetComponent<EnemyDataHandler>().takeDamage(damage);
					Debug.Log(_enemyDataHandler.gameObject.name + " didnt hit player but did attack");
				}
			}
			await Task.Yield();
		}

		
		

		
		//Debug.Log("Attacking not implemented yet");
	}

	


	public override bool check(GameObject enemy) {
		_enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		getPathForTargetType(enemy,moveType,ignoreObstacle);
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
			Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();			
			grid2D.setEnemyAtPosition(enemy);
			return true;
		}

		return false;
	}
	public override void resetWithNewPosition(GameObject enemy, Vector3 dir, Tile tile) {
		base.resetWithNewPosition(enemy,dir,attackTile);
		
	}
	
}
