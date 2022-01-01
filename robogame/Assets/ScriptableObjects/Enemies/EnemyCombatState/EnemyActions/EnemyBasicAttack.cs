using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu]
public class EnemyBasicAttack : AbsAction
{
	public Tile attackTile;
	private EnemyDataHandler _enemyDataHandler;
	public override void Execute(GameObject enemy) {
		
		Grid2D grid2D = _enemyDataHandler.grid2D;
		grid2D.setEnemyAtPosition(enemy);
		//Debug.Log("Attacking not implemented yet");
	}
	// private void enemyAttack( GameObject enemy) {
	//     Node2D node =_grid2D.NodeFromWorldPoint(enemy.transform.position);
	//     node.enemy = enemy;
	//     // List<Node2D> path = enemy.GetComponent<EnemyDataHandler>().path;
	//     // Vector3Int gridPos = tilemap.WorldToCell(path[0].worldPosition);
	//     // tilemap.SetTile(gridPos, attackTile);
	// }

	public override bool Check(GameObject enemy) {

		bool returnable;
		_enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		switch (_enemyDataHandler.attackType.name) {
			case "Melee":
				returnable = MeleeCheck(enemy);
				break;
			case "Ranged":
				returnable = RangedCheck(enemy);
				break;
			default:
				Debug.Log("Enemy has no attack type " + enemy);
				returnable = false;
				break;
		}

		return returnable;
	}

	public override void Highlight(GameObject enemy, Tile tile) {

		base.Highlight(enemy, attackTile);

	}
	public override void unHighlight(GameObject enemy) {
		base.unHighlight(enemy);
	}

	private bool RangedCheck(GameObject enemy) {
		throw new System.NotImplementedException();
	}

	private bool MeleeCheck(GameObject enemy) {
		EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		if (enemyDataHandler.path.Count <= enemyDataHandler.attackRange) { 
			// ARE WE WITHIN RANGE OF OUR TARGET
			return true;
		}
		else {
			//ELSE GO TO NEXT ACTION
			return false;
		}
	}
	
	
}
