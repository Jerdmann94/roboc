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
	public playerStatBlockSO stats;
	public GORunTimeSet aliveEnemies;
	
	public override async Task Execute(GameObject enemy) {

		Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		grid2D.setEnemyAtPosition(enemy);
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		//Debug.Log(tilemap.name);
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
					Debug.Log("didnt hit player but did attack");
				}
			}
			await Task.Delay(300);
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
				returnable = meleeCheck(enemy);
				break;
			case "Ranged":
				returnable = rangedCheck(enemy);
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
	

	

	private bool rangedCheck(GameObject enemy) {
		var position = enemy.transform.position;
		RaycastHit2D hit = Physics2D.Raycast(position, playerSet.items[0].transform.position - position);

		if (hit.collider.gameObject.CompareTag("Player")) {
			Debug.Log("We found Target!");
			return true;
			
		}
		Debug.Log("I found something else with name = " + hit.collider.name);
		return false;
		
		
	
	
	}

	private bool meleeCheck(GameObject enemy) {
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
