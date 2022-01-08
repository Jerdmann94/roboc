using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine.UI;


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
		selectedCard.Card.Execute();
		
		int r = selectedCard.Card.handPosition;
		
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

		checkWhichCardsCanBePlayed();
		
	}

	public void checkWhichCardsCanBePlayed() {
		foreach (var card in handUIArray.items) {

			var cd = card.GetComponent<CardDataScript>();
			var but = card.GetComponent<Button>();

			foreach (var value in energyUIValues) {

				for (int i = 0; i < cd.card.cost.types.Length; i++) {
					if (value.name == cd.card.cost.types[i].name) {
						Debug.Log(value.name + " " + cd.card.cost.types[i].name);
						but.interactable = value.Value >= cd.card.cost.cost[i];
					}
				}
			}
			
		}
	}
	
}