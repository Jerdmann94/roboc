using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleDataHandler :TileMapObject {
	public bool killable;
	[SerializeField]
	private ObstacleSO ifNotDefined;
	private void Start() {
		if (ifNotDefined != null) {
			setUpData(ifNotDefined);
		}
		grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		Tilemap tilemap = tilemapSet.items[2].GetComponent<Tilemap>();
		transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
		grid2D.NodeFromWorldPoint(tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position))).obstacle = true;
	}

	public override void setStun(int damage) {
		if (killable) {
			takeDamage(attack);
		}
		
	}

	public  void setUpData(ObstacleSO obstacleSo) {
		killable = obstacleSo.killable;
		
		base.setUpData(obstacleSo);
		
		GetComponent<SpriteRenderer>().sprite = shape;
		
	}

	private void OnDestroy() {
		grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		Tilemap tilemap = tilemapSet.items[2].GetComponent<Tilemap>();
		grid2D.NodeFromWorldPoint(tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position))).obstacle = false;
	}
}


