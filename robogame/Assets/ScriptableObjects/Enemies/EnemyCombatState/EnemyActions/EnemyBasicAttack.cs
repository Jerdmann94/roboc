using System.Collections;
using System.Collections.Generic;
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
	
	public override void Execute(GameObject enemy) {
		
		Grid2D grid2D = _enemyDataHandler.grid2D;
		grid2D.setEnemyAtPosition(enemy);
		Tilemap tilemap = grid2D.defaultTileMap;
		foreach (var node in _enemyDataHandler.highlightedNodes) {
			if (tilemap.WorldToCell(node.getWorldPosition()) != tilemap.WorldToCell(_enemyDataHandler.target.transform.position)) {
				return;
			}

			if (_enemyDataHandler.target.CompareTag("Player")) {
				stats.health.Value -= _enemyDataHandler.attack;
			}
			else {
				_enemyDataHandler.target.GetComponent<EnemyDataHandler>().takeDamage(_enemyDataHandler.attack);
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
	
	
}
