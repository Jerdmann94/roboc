using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "Card")]
public class CardSO : ScriptableObject {
	public    CardCostSO    cost;
	public new String name;
	public     String cardDescription;
	public     String cardFlavor;
	public     int    damage;
	public int targets;
	public TileBase tileColor;
	internal int handPosition;

	public int doDamage() {
		return damage;
	}
}