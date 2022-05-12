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
	public MoveType moveType;
	public bool ignoreObstacles;
	[SerializeField] private Vector3Event emitter;

	public override async Task execute(GameObject enemy) {
		enemyMove(enemy);
		await Task.Yield();
	}

	public override async Task<bool> check(GameObject enemy) {
		getPathForTargetType(enemy,moveType,ignoreObstacles);
		_enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		//Debug.Log(_enemyDataHandler.getPath());
		await Task.Yield();
		return _enemyDataHandler.getPath() != null;
	}

	public override async Task  highlight(GameObject enemy, Tile tile) {
		await base.highlight(enemy, moveTile);
	}

	private void enemyMove( GameObject enemy) {
		
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
		List<Node2D> path = enemy.GetComponent<EnemyDataHandler>().getPath();
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		//GET  MOVE AMOUNT FROM ENEMY DATA
		var moveAmount = enemy.GetComponent<EnemyDataHandler>().moveAmount;
		for (var i = 0; i <moveAmount; i++) {//MOVE THE ENEMY 1 SQUARE AT A TIME ALONG PATH FOR AS MANY MOVES AS THEY HAVE
			if (i >= path.Count) {
				break;
			}
			if (path[i].getEnemy()!= null) {
				enemy.GetComponent<EnemyDataHandler>().setStun(damage);
				break;
			}

			if (tilemap.WorldToCell(path[i].getWorldPosition()) ==
			    tilemap.WorldToCell(_enemyDataHandler.target.transform.position)) {
				stats.health.Value -= damage;
				break;
			}

			var position = enemy.transform.position;
			grid2D.removeEnemyAtPosition(position);
			grid2D.setEnemyAtPosition(enemy,path[i].getWorldPosition());
			position = path[i].getWorldPosition(); //MOVING THE ENEMY TO THE NEXT POSITION
			enemy.transform.position = position;
			emitter.emit(position);
		}
	}

	public override void resetWithNewPosition(GameObject enemy, Vector3 dir, Tile tile) {
		base.resetWithNewPosition(enemy,dir,moveTile);
	}
}