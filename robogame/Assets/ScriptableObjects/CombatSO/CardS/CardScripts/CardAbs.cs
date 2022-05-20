using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public abstract class CardAbs : ScriptableObject
{
	public new String name;
	public    CardCostSo    cost;
	
	public     int    damage;
	public int targets;
	public TileBase tileColor;
	
	public GoRunTimeSet playerSet;
	public GoRunTimeSet tilemapSet;
	public Vector3IntSet targetPos;
	public GameObject formation;
	public bool hasAoeAttackFormation;
	public Vector3IntSet possibleTilePos;
	public HighlightEnum highlightEnum;
	public GoRunTimeSet gridManagerSet;
	public List<CardAttribute> cardAttributes;
	
	// card ui stuff
	public Image image;
	public string headerText;
	public string bodyText;
	public int handPosition;
	
	public abstract void execute();

	public abstract void displayAttackFormation(Vector3Int pos);
	public abstract void removeAttackFormation(Vector3Int pos);

	public virtual void highlightTiles() {
		if (possibleTilePos.items.Count != 0) {
			possibleTilePos.items.Clear();
		}
		GameObject form = Instantiate(formation, playerSet.items[0].transform.position,Quaternion.identity);
		form.GetComponent<DecideWhichListsToJoin>().init(WhichTilePosList.PossibleTilePos);
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForPlayer")?.GetComponent<Tilemap>();
		Grid2D grid = gridManagerSet.items[0].GetComponent<Grid2D>();
		var removable = new List<Vector3Int>();
		switch (highlightEnum.name) {
			case "Open":
				foreach (var pos in possibleTilePos.items) {
					if (tilemap != null 
					    && grid.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).getEnemy() == null
					    && !grid.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos)).obstacle) {
						tilemap.SetTile(pos, tileColor);
					}
					else {
						removable.Add(pos);
					}
					
					
				}
				
				break;
			case "All":
				foreach (var pos in possibleTilePos.items) {
					
					if (tilemap != null) {
						tilemap.SetTile(pos, tileColor);
					}
					else {
						removable.Add(pos);
					}
					
					
				}
			
				break;
			case "Enemies":
				foreach (var pos in possibleTilePos.items) {
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
				
				break;
			case "Obstacles":
				break;
			case "Pushables":
				foreach (var pos in possibleTilePos.items) {
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
				
				break;
		}
		
		
		//REMOVE ANY UNUSED TARGETS FROM POSSIBLE TARGETS
		foreach (var vector3Int in removable.Where(vector3Int => possibleTilePos.items.Contains(vector3Int))) {
			possibleTilePos.items.Remove(vector3Int);
		}
		
		Destroy(form);
	}
	

	public virtual int doDamage() {
		return damage;
	}
}

