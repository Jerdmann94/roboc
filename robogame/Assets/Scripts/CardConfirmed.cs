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
	public Vector3IntSet targetPos;
	public CardUIValue[] energyUIValues;
	public PlayerStateManager playerStateManager;
	[SerializeField] private GoRunTimeSet handUIArray;
	public CardSet handSet;
	public CardSet discardSet;

	private void Awake()
	{
		selectedCard.Card = null;
		targetPos.items = new List<Vector3Int>();
	}

	public async void cardConfirmed()
	{

		if (selectedCard.Card == null)
		{
			Debug.Log("selected card is null");
			return;
		}
		adjustEnergyValue();
		selectedCard.Card.card.execute();
		selectedCard.Card.gameCardAttributesList.ForEach(async gameCardAttribute => {
			gameCardAttribute.context = await gameCardAttribute.cardAttribute.execute(selectedCard.Card, gameCardAttribute.context);
		} );

		if (selectedCard.Card.gameCardAttributesList.Count == 0) {
			discardCard();
		}
		else {
			selectedCard.Card.gameCardAttributesList.ForEach(async gameCardAttribute => {
				var context = gameCardAttribute.context;
				switch (context.attributeContextTypeEnum) {
					case ShatterContextEnum:
						var shatterContext = (ShatterContext) context;
						if (shatterContext.shouldShatter) {
							shatterCard();
						}
						break;
					default:
						discardCard();
						break;
				}
			} );
		}
		
		
	}

	private void shatterCard() {
		int r = selectedCard.Card.handPosition;
		Destroy(handUIArray.items[r]);
		handUIArray.items.RemoveAt(r);
		handSet.items.RemoveAt(r);
		playerStateManager.resetHandPosition();
		targetPos.items = new List<Vector3Int>();
		Debug.Log("Card Shattered");
		
		
	}
	private void discardCard() {
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
			for (int i = 0; i < selectedCard.Card.card.cost.types.Length; i++) {
				if (card.name != selectedCard.Card.card.cost.types[i].name) continue;
				card.Value -= selectedCard.Card.card.cost.cost[i];
			}
			
		}

		checkWhichCardsCanBePlayed();
		
	}

	public void checkWhichCardsCanBePlayed() {
		foreach (var card in handUIArray.items) {

			var cd = card.GetComponent<CardDataScript>();
			var but = card.GetComponent<Button>();

			foreach (var value in energyUIValues) {

				for (int i = 0; i < cd.card.card.cost.types.Length; i++) {
					if (value.name == cd.card.card.cost.types[i].name) {
						//Debug.Log(value.name + " " + cd.card.cost.types[i].name);
						but.interactable = value.Value >= cd.card.card.cost.cost[i];
					}
				}
			}
			
		}
	}

	public void changeEnergyValue(CardUIValue card, int value) {
		card.Value = value;
		checkWhichCardsCanBePlayed();
	}
	
}