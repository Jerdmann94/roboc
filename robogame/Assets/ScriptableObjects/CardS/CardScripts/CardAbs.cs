using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class CardAbs : ScriptableObject
{
	public    CardCostSo    cost;
	public new String name;
	public     String cardDescription;
	public     String cardFlavor;
	public     int    damage;
	public int targets;
	public TileBase tileColor;
	internal int handPosition;
	public GoRunTimeSet playerSet;
	public GoRunTimeSet tilemapSet;
	public Vector3IntSet targetPos;
	public GameObject formation;
	public Vector3IntSet possibleTargets;
	public HighlightEnum highlightEnum;
	public GoRunTimeSet gridManagerSet;
	public abstract void execute();

	public virtual void highlightTiles() {
		if (possibleTargets.items.Count != 0) {
			possibleTargets.items.Clear();
		}
		GameObject form = Instantiate(formation, playerSet.items[0].transform.position,Quaternion.identity);
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForPlayer")?.GetComponent<Tilemap>();
		Grid2D grid = gridManagerSet.items[0].GetComponent<Grid2D>();
		var removable = new List<Vector3Int>();
		


		switch (highlightEnum.name) {
			case "Open":
				foreach (var pos in possibleTargets.items) {
					if (tilemap != null 
					    && grid.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).getEnemy() == null
					    && !grid.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).obstacle) {
						tilemap.SetTile(pos, tileColor);
					}
					else {
						removable.Add(pos);
					}
					
					
				}
				Destroy(form);
				break;
			case "All":
				foreach (var pos in possibleTargets.items) {
					
					if (tilemap != null) {
						tilemap.SetTile(pos, tileColor);
					}
					else {
						removable.Add(pos);
					}
					
					
				}
				Destroy(form);
				break;
			case "Enemies":
				foreach (var pos in possibleTargets.items) {
					if (tilemap != null && grid.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).getEnemy() != null) {
						tilemap.SetTile(pos, tileColor);
				
					}
					else if (tilemap != null && grid.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).getObstacle() != null) {
						if (grid.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).getObstacle().pushable) {
							tilemap.SetTile(pos, tileColor);
						}
					}
					else {
						removable.Add(pos);
					}
					
					
				}
				Destroy(form);
				break;
			case "Obstacles":
				break;
			case "Pushables":
				foreach (var pos in possibleTargets.items) {
					if (tilemap != null && grid.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).getEnemy() != null) {
						tilemap.SetTile(pos, tileColor);
				
					}
					
					else {
						removable.Add(pos);
					}
				}
				break;
			default:
				Debug.Log("no case worked");
				Destroy(form);
				break;
		}
		
		
		//REMOVE ANY UNUSED TARGETS FROM POSSIBLE TARGETS
		foreach (var vector3Int in removable.Where(vector3Int => possibleTargets.items.Contains(vector3Int))) {
			possibleTargets.items.Remove(vector3Int);
		}
		
		Destroy(form);
	}
	

	public virtual int doDamage() {
		return damage;
	}
}

