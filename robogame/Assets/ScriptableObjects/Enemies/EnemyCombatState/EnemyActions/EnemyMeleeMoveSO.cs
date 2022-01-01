using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;



[CreateAssetMenu(fileName ="new Action", menuName = "EnemyMeleeMovementSO")]
public class EnemyMeleeMoveSO : AbsAction {
	private EnemyDataHandler _enemyDataHandler;
	
	public Tile moveTile;
	
	//private EnemyTileHighlighter enemyTileHighlighter;
	
	//MONSTER NEEDS TO ALREADY BE STORING THE TARGET ON ITSELF FOR THIS TO WORK
	// NEED A TARGET SETTER ACTION

	public override void Execute(GameObject monster) {
		enemyMove(monster);
	}

	public override bool Check(GameObject monster) {
		_enemyDataHandler = monster.GetComponent<EnemyDataHandler>();
		if (_enemyDataHandler.path.Count > _enemyDataHandler.attackRange) {
			// IS PATH LONGER THAN THE DISTANCE TO THE TARGET, IF SO, MOVE TOWARDS THE TARGET
			return true;
		}
		else {
			//ELSE GO TO NEXT ACTION
			return false;
		}
		
	}

	public override void Highlight(GameObject enemy, Tile tile) {
		base.Highlight(enemy, moveTile);
	}

	public override void unHighlight(GameObject enemy) {
		base.unHighlight(enemy);
	}
	

	private void enemyMove( GameObject enemy) {
		Grid2D grid2D = _enemyDataHandler.grid2D;
		
		List<Node2D> path = enemy.GetComponent<EnemyDataHandler>().path;
		//GET  MOVE AMOUNT FROM ENEMY DATA
		int moveAmount = enemy.GetComponent<EnemyDataHandler>().moveAmount;
        
		
        
		//IS PATH LONGER THAN THE MOVE AMOUNT 
		if (path.Count <= moveAmount) return;
		for (int i = 0; i <moveAmount; i++) {//MOVE THE ENEMY 1 SQUARE AT A TIME ALONG PATH FOR AS MANY MOVES AS THEY HAVE
			//ADD PATH NODES TO ENEMY PATH
			//enemy.GetComponent<EnemyDataHandler>().highlightedMoveNodes.Add(path[i]);
                
                
			Node2D startNode = grid2D.NodeFromWorldPoint(enemy.transform.position);//CURRENT  NODE
			startNode.enemy = null; //SETTING CURRENT NODE TO NOT HAVE ENEMY, ALLOWING OTHER ENEMIES TO MOVE HERE - this is for multple move enemies
                
			grid2D.setEnemyAtPosition(enemy,path[i].worldPosition);
                
			enemy.transform.position = path[i].worldPosition; //MOVING THE ENEMY TO THE NEXT POSITION
		}  
            
		//SETTING TILE BENEATH ENEMY TO ORIGINAL TILE
		//base.returnTileUnderEnemy(enemy);


	}
}