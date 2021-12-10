using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Card", menuName = "Card")]
public class CardSO : ScriptableObject {
	public     int    cost;
	public new String name;
	public     String cardDescription;
	public     String cardFlavor;
	public     int    damage;

	public int doDamage() {
		return damage;
	}
}