using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/Basic Card")]
public class CardSo : CardAbs {
	
	public override int doDamage() {
		return damage;
	}

	public override void execute() {
		Debug.Log("STOP USING THIS CARD, USE COMMAND CARDS FROM NOW ON");
	}
}