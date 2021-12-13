using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PlayerScript : MonoBehaviour {
	public DeckSO     deckData;
	public GameObject cardUI;
	public GameObject canvas;
	public GameObject handPoint1;

	public GameObject handPoint2;

	//private ArrayList  deck;
	//private ArrayList  hand;
	//private ArrayList  discard;
	private int        handSize = 5;
	static  Random     _random  = new Random();
	private GameObject combatManager;
	public  FloatValue hp;

	public CardSet handSet;
	public CardSet deckSet;
	public CardSet discardSet;


	// Start is called before the first frame update
	void Start() {
		combatManager = GameObject.Find("CombatManager");
		handPoint1 = GameObject.Find("handPoint1");
		handPoint2 = GameObject.Find("handPoint2");
		canvas = GameObject.Find("Canvas");
		//deck = new ArrayList();
		deckSet.items = new List<CardSO>();
		handSet.items = new List<CardSO>();
		discardSet.items = new List<CardSO>();
		foreach (var cardSo in deckData.deck) {
			//deck.Add(cardSo);
			deckSet.add((CardSO) cardSo);
		}

		//hand = new ArrayList();
		//discard = new ArrayList();
		//Shuffle<CardSO>(deck);
		deckSet.items = Shuffle<CardSO>(deckSet.items);
		drawHand();
	}

	// Update is called once per frame
	void Update() { }

	public void drawHand() {
		for (int i = 0; i < handSize; i++) {
			createCardUI(i);
		}
	}

	public void createCardUI(int i) {
		handSet.add((CardSO) deckSet.items[0]);
		//hand.Add(deck[0]);

		deckSet.remove((CardSO) deckSet.items[0]);
		//deck.RemoveAt(0);

		float xvalue = (handPoint1.transform.position.x - handPoint2.transform.position.x) / handSize;
		Vector3 temp = new Vector3(handPoint1.transform.position.x + Mathf.Abs((xvalue * i)) + Mathf.Abs(xvalue / 2),
		                           handPoint1.transform.position.y, 0);
		GameObject card = Instantiate(cardUI, temp, quaternion.identity);
		card.GetComponent<CardDataScript>().setUpCard((CardSO) handSet.items[i], card.transform.position);
		card.transform.SetParent(canvas.transform);
		card.transform.localScale = Vector3.one;
		card.GetComponent<CardDataScript>().startPosition = temp;

		card.GetComponent<Button>().onClick.AddListener(() =>
			                                                combatManager.GetComponent<MouseHandler>()
			                                                             .playCard(card.GetComponent<CardDataScript>()
				                                                             .card));
	}


	static List<CardSO> Shuffle<T>(List<CardSO> array) {
		int n = array.Count;
		for (int i = 0; i < (n - 1); i++) {
			// Use Next on random instance with an argument.
			// ... The argument is an exclusive bound.
			//     So we will not go past the end of the array.
			int r = i + _random.Next(n - i);
			var t = array[r];
			array[r] = array[i];
			array[i] = t;
		}

		return array;
	}
}