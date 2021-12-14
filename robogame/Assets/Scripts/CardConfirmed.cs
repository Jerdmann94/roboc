using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;




public class CardConfirmed : MonoBehaviour {
	public SingleCardSet selectedCard;
	public MouseHandler  mouseHandler;
	public Vector3Event  attackEvent;
	public Vector3IntSet targetPos;
	public GORunTimeSet   aliveEnemies;

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
		mouseHandler.player.transform.position = mouseHandler.map.GetCellCenterWorld(targetPos.items[0]);
	}

	private void specialAttackConfirm() {
		Debug.Log("special attack");
	}

	private void basicMoveConfirm() {
		mouseHandler.player.transform.position = mouseHandler.map.GetCellCenterWorld(targetPos.items[0]);
	}

	private void basicAttackConfirm() {
		Debug.Log("basic attack confirm");
		targetPos.items.ForEach(pos => {
			                        Debug.Log(mouseHandler.map.GetCellCenterWorld(pos));
			                        attackEvent.emit(mouseHandler.map.GetCellCenterWorld(pos));
		                        });

		//attackEvent.emit(mouseHandler.map.GetCellCenterWorld(targetPos.items[0]));


		// foreach (GameObject e in combatManager.aliveEnemies) {
		//  
		//  if (e.transform.position == mouseHandler.map.GetCellCenterWorld(mouseHandler.targetPos)) {
		//   //e.GetComponent<enemyDataHandler>().takeDamage(mouseHandler.selectedCard.doDamage());
		//  }
		// }
	}
}