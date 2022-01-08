using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class CardAbs : ScriptableObject
{
	public    CardCostSO    cost;
	public new String name;
	public     String cardDescription;
	public     String cardFlavor;
	public     int    damage;
	public int targets;
	public TileBase tileColor;
	internal int handPosition;
	public GORunTimeSet playerSet;
	public GORunTimeSet tilemapSet;
	public Vector3IntSet targetPos;
	public GameObject formation;
	public Vector3IntSet possibleTargets;
	public TileRunTimeSet possibleTargetsTiles;

	public abstract void Execute();

	public virtual void highlightTiles() {
		GameObject form = Instantiate(formation, playerSet.items[0].transform.position,Quaternion.identity);
		Tilemap tilemap = tilemapSet.items[0].GetComponent<Tilemap>();
		foreach (var pos in possibleTargets.items) {
			tilemap.SetTile(pos,tileColor);
		}
		Destroy(form);
	}
	

	public virtual int doDamage() {
		return damage;
	}
}
