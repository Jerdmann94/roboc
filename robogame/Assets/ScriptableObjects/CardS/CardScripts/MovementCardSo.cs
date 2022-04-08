using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/Basic Move Card")]
public class MovementCardSo : CardAbs {
  
    
    public override void execute() {
        playerSet.items[0].transform.position = tilemapSet.items[2].GetComponent<Tilemap>().GetCellCenterWorld(targetPos.items[0]);
    }
}
