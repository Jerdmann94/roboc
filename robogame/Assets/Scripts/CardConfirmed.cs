using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CombatManager;
using static MouseHandler;

public class CardConfirmed 
{
	

	public void cardConfirmed(CardSO card) {
	   switch (card.name) {
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
		MouseHandler.mouseHandler.player.transform.position = MouseHandler.mouseHandler.map.GetCellCenterWorld(MouseHandler.mouseHandler.targetPos);
	}

	private void specialAttackConfirm() {
		Debug.Log("special attack");
	}

	private void basicMoveConfirm() {
	   MouseHandler.mouseHandler.player.transform.position = MouseHandler.mouseHandler.map.GetCellCenterWorld(MouseHandler.mouseHandler.targetPos);
   }
   
   private void basicAttackConfirm() {
	   foreach (GameObject e in combatManager.aliveEnemies) {
		   if (e.transform.position == mouseHandler.map.GetCellCenterWorld(mouseHandler.targetPos)) {
			   e.GetComponent<enemyDataHandler>().takeDamage(mouseHandler.selectedCard.doDamage());
		   }
	   }
	   
   }
   
}
