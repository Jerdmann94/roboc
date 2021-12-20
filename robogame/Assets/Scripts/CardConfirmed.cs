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
	public CardUIValue[] energyUIValues;
	public PlayerStateManager playerStateManager;
	[SerializeField] private GORunTimeSet handUIArray;
	public CardSet handSet;
	public CardSet deckSet;
	public CardSet discardSet;

	private void Awake()
	{
		selectedCard.Card = null;
		targetPos.items = new List<Vector3Int>();
	}

	public void cardConfirmed()
	{

		if (selectedCard.Card == null)
		{
			Debug.Log("selected card is null");
			return;
		}
		adjustEnergyValue();
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


		int r = selectedCard.Card.handPosition;
		Debug.Log(r);
		Destroy(handUIArray.items[r]);
		handUIArray.items.RemoveAt(r);
		discardSet.items.Add(handSet.items[r]);
		handSet.items.RemoveAt(r);
		playerStateManager.resetHandPosition();
		targetPos.items = new List<Vector3Int>();
	}

	private void adjustEnergyValue()
	{
		foreach (var card in energyUIValues)
		{
			for (int i = 0; i < selectedCard.Card.cost.types.Length; i++) {
				if (card.name != selectedCard.Card.cost.types[i].name) continue;
				card.Value -= selectedCard.Card.cost.cost[i];
			}
			
		}
	}

	private void specialMoveConfirm() {
		playerStateManager.player.transform.position = mouseHandler.map.GetCellCenterWorld(targetPos.items[0]);
	}

	private void specialAttackConfirm() {
		targetPos.items.ForEach(pos => {
			attackEvent.emit(mouseHandler.map.GetCellCenterWorld(pos));
		});
	}

	private void basicMoveConfirm() {
		playerStateManager.player.transform.position = mouseHandler.map.GetCellCenterWorld(targetPos.items[0]);
	}

	private void basicAttackConfirm() {
		targetPos.items.ForEach(pos => {
			                       // Debug.Log(mouseHandler.map.GetCellCenterWorld(pos));
			                        attackEvent.emit(mouseHandler.map.GetCellCenterWorld(pos));
		                        });
		
	}
}