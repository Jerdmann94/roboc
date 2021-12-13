using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using static CombatManager;
using static MouseHandler;

public class CardConfirmed : MonoBehaviour {
	public SingleCardSet selectedCard;
	public MouseHandler  mouseHandler;
	public Vector3Event  attackEvent;

	public void cardConfirmed() {
		switch (selectedCard.Card.name) {
			case "Move":
				basicMoveConfirm();
				break;
			case "Attack":
				basicAttackConfirm();
				break;
			case "Special Attack":
				specialAttackConfirm();
				break;
			case "Special Move":
				specialMoveConfirm();
				break;
			default:
				break;
		}
	}

	private void specialMoveConfirm() {
		mouseHandler.player.transform.position = mouseHandler.map.GetCellCenterWorld(mouseHandler.targetPos);
	}

	private void specialAttackConfirm() {
		Debug.Log("special attack");
	}

	private void basicMoveConfirm() {
		mouseHandler.player.transform.position = mouseHandler.map.GetCellCenterWorld(mouseHandler.targetPos);
	}

	private void basicAttackConfirm() {
		attackEvent.emit(mouseHandler.map.GetCellCenterWorld(mouseHandler.targetPos));
		Debug.Log(mouseHandler.targetPos);

		// foreach (GameObject e in combatManager.aliveEnemies) {
		//  
		//  if (e.transform.position == mouseHandler.map.GetCellCenterWorld(mouseHandler.targetPos)) {
		//   //e.GetComponent<enemyDataHandler>().takeDamage(mouseHandler.selectedCard.doDamage());
		//  }
		// }
	}
}