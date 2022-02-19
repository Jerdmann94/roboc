using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/Attack 1 Square Card")]
public class AttackScriptBasic : CardAbs
{
	public Vector3Event  attackEvent;
	public override void Execute() {
		targetPos.items.ForEach(pos => {
			attackEvent.emit(tilemapSet.items[1].GetComponent<Tilemap>().GetCellCenterWorld(pos));
		});
		
	}
	
}
