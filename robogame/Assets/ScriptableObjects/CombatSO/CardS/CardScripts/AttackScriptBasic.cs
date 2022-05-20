using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/Attack 1 Square Card")]
public class AttackScriptBasic : CardAbs
{
	public Vector3Event  attackEvent;
	public override void execute() {
		var tilemap = tilemapSet.items[1].GetComponent<Tilemap>();

		if (targets > targetPos.items.Count) { // MORE COMPLICATED STUFF FOR MORE THAN 1 TARGET
			if (targetPos.items.Count == 1) { // IF MORE THAN 1 TARGET BUT ONLY CHOSE 1 SPACE
				for (int i = 0; i < targets; i++) {
					attackEvent.emit(tilemap.GetCellCenterWorld(targetPos.items[0]));

				}
			}
			else { // IF MORE THAN 1 TARGET BUT CHOSE MULTIPLE SPACES 
				for (int i = 0; i < targets; i++) {
					var counter = 0;
					if (i >= targetPos.items.Count) {
						counter = Random.Range(0, targetPos.items.Count);
					}
					else {
						counter = i;
					}
					attackEvent.emit(tilemap.GetCellCenterWorld(targetPos.items[counter]));

				}
			}
		}
		else { // BASIC IMPLEMENTATION FOR 1 TARGET
			Debug.Log("doing basic implementation");
			targetPos.items.ForEach(pos => {
				attackEvent.emit(tilemap.GetCellCenterWorld(pos));
			});
		}
		
		
	}

	public override void displayAttackFormation(Vector3Int pos) {
		throw new System.NotImplementedException();
	}

	public override void removeAttackFormation(Vector3Int pos) {
		throw new System.NotImplementedException();
	}
}
