using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;



[CreateAssetMenu(fileName = "new Card", menuName = "EnemyCards/EnemyRangedMove")]
public class EnemyRangedMove : AbsAction {
	private EnemyDataHandler _enemyDataHandler;
	
	public Tile moveTile;
	
	public override async  Task Execute(GameObject monster) {
		enemyMove(monster);
		await Task.Yield();
	}

	public override bool Check(GameObject monster) {
		_enemyDataHandler = monster.GetComponent<EnemyDataHandler>();
		if (_enemyDataHandler.getPath() == null) {
			Debug.Log(monster.name);
			return false;
		}

		if (_enemyDataHandler.getPath().Count < _enemyDataHandler.attackRange && !rangedCheck(monster)) {
			monster.GetComponent<EnemyDataHandler>().getPath().Reverse();
			Debug.Log("enemy is within range but cannot see target");
			return true;
		}

		
		if (_enemyDataHandler.getPath().Count < _enemyDataHandler.attackRange ) {
			// IS THE TARGET TOO CLOSE TO SHOOT, IF SO MOVE
			Debug.Log(_enemyDataHandler.getPath().Count + " " + _enemyDataHandler.attackRange);
			return true;
		} if(_enemyDataHandler.getPath().Count >= _enemyDataHandler.attackRange && rangedCheck(monster)) {
			Debug.Log("something weird");
			return false;
		}

		
		
		Debug.Log("something weird");
		return false;
		
		
	}

	public override void Highlight(GameObject enemy, Tile tile) {
		base.Highlight(enemy, moveTile);
	}

	private bool rangedCheck(GameObject enemy) {
		var position = enemy.transform.position;
		RaycastHit2D hit = Physics2D.Raycast(position, enemy.GetComponent<EnemyDataHandler>().target.transform.position);

		if (hit.collider.gameObject.CompareTag("Player")) {
			//Debug.Log("We found Target!");
			return true;
			
		}
		//Debug.Log("I found something else with name = " + hit.collider.name);
		return false;
		
		
	
	
	}
	private void enemyMove( GameObject enemy) {
		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		
		List<Node2D> path = enemy.GetComponent<EnemyDataHandler>().getPath();
		//GET  MOVE AMOUNT FROM ENEMY DATA
		int moveAmount = enemy.GetComponent<EnemyDataHandler>().moveAmount;
        
		
        
		//IS PATH LONGER THAN THE MOVE AMOUNT 
		if (path.Count <= moveAmount || path.Count == 1) {
			Debug.Log("path is smaller than move amount");
			return;
		}
		for (int i = 0; i <moveAmount; i++) {//MOVE THE ENEMY 1 SQUARE AT A TIME ALONG PATH FOR AS MANY MOVES AS THEY HAVE
			//ADD PATH NODES TO ENEMY PATH
			//enemy.GetComponent<EnemyDataHandler>().highlightedMoveNodes.Add(path[i]);
                
                
			Node2D startNode = grid2D.NodeFromWorldPoint(enemy.transform.position);//CURRENT  NODE
			//startNode.setEnemy(null); //SETTING CURRENT NODE TO NOT HAVE ENEMY, ALLOWING OTHER ENEMIES TO MOVE HERE - this is for multple move enemies
                
			grid2D.setEnemyAtPosition(enemy,path[i].getWorldPosition());

			if (enemy.transform.position == path[i].getWorldPosition()) {
				Debug.Log("path position is already where you are standing");
			}
			enemy.transform.position = path[i].getWorldPosition(); //MOVING THE ENEMY TO THE NEXT POSITION
			//Debug.Log(enemy.transform.position);
		}  
            
		//SETTING TILE BENEATH ENEMY TO ORIGINAL TILE
		//base.returnTileUnderEnemy(enemy);


	}

	public override void resetWithNewPosition(GameObject enemy, Vector3 dir, Tile tile) {
		base.resetWithNewPosition(enemy,dir,moveTile);
	}
}