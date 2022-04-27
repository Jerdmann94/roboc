using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/Spawn Tile Effect")]

public class SpawnTileEffect : CardAbs {
	[SerializeField] private TileEffectSo tileEffect;
	[SerializeField] private GameObject spawnableTileEffect;
	[SerializeField] private GoRunTimeSet combatManagerSet;
	
	public override void execute() {
		
		var grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		var target = tilemapSet.items[2].GetComponent<Tilemap>().GetCellCenterWorld(targetPos.items[0]);
		var node = grid2D.nodeFromWorldPoint(target);
		//THIS NEEDS A SWITCH STATEMENT FOR DIFFERENT EFFECTS THAT CAN BE
		// SPAWNED WITH THIS CARD
		if (node.getTileEffect()!= null) {
			//Debug.Log("this tile has an effect" + node.getTileEffect().name);
			node.getTileEffect().reactWithFire();
		}
		var temp = Instantiate(spawnableTileEffect, target, quaternion.identity);
		var handler = temp.GetComponent<TileEffectHandler>();
			handler.setupData(tileEffect);
			handler.execute();
			
		

	}
}


