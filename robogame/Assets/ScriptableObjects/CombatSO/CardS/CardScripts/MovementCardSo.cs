using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/Basic Move Card")]
public class MovementCardSo : CardAbs {
	[SerializeField] private Vector3Event emitter;
    
    public override void execute() {
        playerSet.items[0].transform.position = tilemapSet.items[2].GetComponent<Tilemap>().GetCellCenterWorld(targetPos.items[0]);
        emitter.emit(playerSet.items[0].transform.position);
    }

    public override void displayAttackFormation(Vector3Int pos) {
	    throw new System.NotImplementedException();
    }

    public override void removeAttackFormation(Vector3Int pos) {
	    throw new System.NotImplementedException();
    }
}
