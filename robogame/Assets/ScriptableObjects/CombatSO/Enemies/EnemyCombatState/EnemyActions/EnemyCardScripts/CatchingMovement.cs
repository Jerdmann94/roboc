using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "EnemyCards/CatchingMove")]
public class CatchingMovement : AbsAction {
	private EnemyDataHandler _enemyDataHandler;
	//public MoveType moveType;
	[SerializeField] private bool ignoreObstacle;
	public Tile moveTile;
	[SerializeField] private GoRunTimeSet aliveEnemies;
	[SerializeField] private MoveType moveType;	
	[SerializeField] private Vector3Event emitter;

	//private EnemyTileHighlighter enemyTileHighlighter;
	
	//MONSTER NEEDS TO ALREADY BE STORING THE TARGET ON ITSELF FOR THIS TO WORK
	// NEED A TARGET SETTER ACTION

	public override async  Task execute(GameObject monster) {
		enemyMove(monster);
		_enemyDataHandler = monster.GetComponent<EnemyDataHandler>();
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
		if (tilemap.WorldToCell(_enemyDataHandler.specialTarget.transform.position) == tilemap.WorldToCell(monster.transform.position)) {
			_enemyDataHandler.specialTarget.GetComponent<EnemyDataHandler>().doLateDeath();
			aliveEnemies.items.ForEach(e => e.GetComponent<EnemyDataHandler>().takeDamage(-20));
		}
		await Task.Yield();
	}

	public override async Task<bool> check(GameObject monster) {
		
		getPathForTargetType(monster,moveType,ignoreObstacle);
		_enemyDataHandler = monster.GetComponent<EnemyDataHandler>();
		if (_enemyDataHandler.getPath() == null || _enemyDataHandler.specialTarget == null) {
			//Debug.Log(monster.name);
			return false;
		}
		await Task.Yield();
		return true;

	}

	public override async Task highlight(GameObject enemy, Tile tile) {
		await base.highlight(enemy, moveTile);
	}

	public override void unHighlight(GameObject enemy) {
		base.unHighlight(enemy);
	}
	

	private void enemyMove( GameObject enemy) {
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
		List<Node2D> path = enemy.GetComponent<EnemyDataHandler>().getPath();
		//GET  MOVE AMOUNT FROM ENEMY DATA
		int moveAmount = enemy.GetComponent<EnemyDataHandler>().moveAmount;
        
		
        
		//IS PATH LONGER THAN THE MOVE AMOUNT 
		// if (path.Count <= moveAmount || path.Count == 1) {
		// 	Debug.Log("path is smaller than move amount");
		// 	return;
		// }
		for (int i = 0; i <moveAmount; i++) {//MOVE THE ENEMY 1 SQUARE AT A TIME ALONG PATH FOR AS MANY MOVES AS THEY HAVE
			//ADD PATH NODES TO ENEMY PATH
			//enemy.GetComponent<EnemyDataHandler>().highlightedMoveNodes.Add(path[i]);
			if (path.Count < i + 1) {
				Debug.Log("breaking movement because path is less than movement");
				break;
			}
                
			Node2D startNode = grid2D.nodeFromWorldPoint(enemy.transform.position);//CURRENT  NODE
			//startNode.setEnemy(null); //SETTING CURRENT NODE TO NOT HAVE ENEMY, ALLOWING OTHER ENEMIES TO MOVE HERE - this is for multple move enemies
                
			grid2D.setEnemyAtPosition(enemy,path[i].getWorldPosition());

			if (enemy.transform.position == path[i].getWorldPosition()) {
				Debug.Log("path position is already where you are standing");
			}
			enemy.transform.position = path[i].getWorldPosition(); //MOVING THE ENEMY TO THE NEXT POSITION
			emitter.emit(enemy.transform.position);
			//Debug.Log(enemy.transform.position);
			
		}  
            
		//SETTING TILE BENEATH ENEMY TO ORIGINAL TILE
		//base.returnTileUnderEnemy(enemy);
		

	}

	public override void resetWithNewPosition(GameObject enemy, Vector3 dir, Tile tile) {
		base.resetWithNewPosition(enemy,dir,moveTile);
	}
}
