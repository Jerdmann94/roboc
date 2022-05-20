using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/HealPlayer")]

public class HealPlayer : CardAbs {
	public int healAmount;
	public bool healOverTime;
	[ConditionalField("healOverTime")] 
	public int healOverTimeTurns;
	[ConditionalField("healOverTime")] 
	public int healOverTimeAmount;

	public PlayerStatBlockSo stats;
	
	public override void execute() {
		stats.health.takeDamage(healAmount);
		if (healOverTime) {
			stats.labels.Add(ScriptableObject.CreateInstance<HealOverTime>().init(healOverTimeAmount,healOverTimeTurns,stats));
		}
	}

	public override void displayAttackFormation(Vector3Int pos) {
		throw new System.NotImplementedException();
	}

	public override void removeAttackFormation(Vector3Int pos) {
		throw new System.NotImplementedException();
	}
}
