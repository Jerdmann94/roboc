using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/Basic Card")]
public class CardSO : CardAbs {
	
	public override int doDamage() {
		return damage;
	}

	public override void Execute() {
		Debug.Log("STOP USING THIS CARD, USE COMMAND CARDS FROM NOW ON");
	}
}