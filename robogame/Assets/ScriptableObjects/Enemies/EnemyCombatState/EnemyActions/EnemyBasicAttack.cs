using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu]
public class EnemyBasicAttack : AbsAction
{
	public Tile attackTile;
	private EnemyDataHandler _enemyDataHandler;
	public playerStatBlockSO stats;
	public GORunTimeSet aliveEnemies;
	
	public override void Execute(GameObject enemy) {

		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		grid2D.setEnemyAtPosition(enemy);
		Tilemap tilemap = grid2D.defaultTileMap;
		foreach (var node in _enemyDataHandler.highlightedNodes) {
			// if (tilemap.WorldToCell(node.getWorldPosition()) != tilemap.WorldToCell(_enemyDataHandler.target.transform.position)) {
			// 	return;
			// }

			if (_enemyDataHandler.target.CompareTag("Player") && 
			    tilemap.WorldToCell(node.getWorldPosition()) == tilemap.WorldToCell(_enemyDataHandler.target.transform.position)) {
				stats.health.Value -= _enemyDataHandler.attack;
			}
			else {
				foreach (var otherEnemy in aliveEnemies.items.Where(
					         otherEnemy => otherEnemy.transform.position == node.getWorldPosition())) {
					otherEnemy.GetComponent<EnemyDataHandler>().takeDamage(enemy.GetComponent<EnemyDataHandler>().attack);
				}
			}
		}

		
		

		
		//Debug.Log("Attacking not implemented yet");
	}

	private void doPlayerDamage() {
		throw new System.NotImplementedException();
	}


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
		if (enemyDataHandler.getPath().Count <= enemyDataHandler.attackRange) { 
			// ARE WE WITHIN RANGE OF OUR TARGET
			return true;
		}
		else {
			//ELSE GO TO NEXT ACTION
			return false;
		}
	}
	public override void resetWithNewPosition(GameObject enemy, Vector3 dir, Tile tile) {
		base.resetWithNewPosition(enemy,dir,attackTile);
	}
	
}
