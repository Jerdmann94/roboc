using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;



[CreateAssetMenu(fileName = "new Card", menuName = "EnemyCards/EnemyMeleeMove")]
public class EnemyMeleeMoveSo : AbsAction {
	private EnemyDataHandler _enemyDataHandler;
	public Tile moveTile;
	public PlayerStatBlockSo stats;
	public override async  Task execute(GameObject monster) {
		enemyMove(monster);
		await Task.Yield();
	}

	public override bool check(GameObject monster) {
		getPathForTargetType(monster);
		_enemyDataHandler = monster.GetComponent<EnemyDataHandler>();
		return _enemyDataHandler.getPath() != null;
	}

	public override void highlight(GameObject enemy, Tile tile) {
		base.highlight(enemy, moveTile);
	}

	private void enemyMove( GameObject enemy) {
		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		List<Node2D> path = enemy.GetComponent<EnemyDataHandler>().getPath();
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		//GET  MOVE AMOUNT FROM ENEMY DATA
		var moveAmount = enemy.GetComponent<EnemyDataHandler>().moveAmount;
		for (var i = 0; i <moveAmount; i++) {//MOVE THE ENEMY 1 SQUARE AT A TIME ALONG PATH FOR AS MANY MOVES AS THEY HAVE
			if (path[i].getEnemy()) {
				enemy.GetComponent<EnemyDataHandler>().setStun(damage);
				break;
			}

			if (tilemap.WorldToCell(path[i].getWorldPosition()) ==
			    tilemap.WorldToCell(_enemyDataHandler.target.transform.position)) {
				stats.health.Value -= damage;
				break;
			}

			grid2D.removeEnemyAtPosition(enemy.transform.position);
			grid2D.setEnemyAtPosition(enemy,path[i].getWorldPosition());
			enemy.transform.position = path[i].getWorldPosition(); //MOVING THE ENEMY TO THE NEXT POSITION
		}
	}

	public override void resetWithNewPosition(GameObject enemy, Vector3 dir, Tile tile) {
		base.resetWithNewPosition(enemy,dir,moveTile);
	}
}