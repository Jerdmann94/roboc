using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.GameEvent;
using ScriptableObjects.Sets;
using UnityEngine;



public class CardSelected : MonoBehaviour {
	public SingleCardSet selectedCard;
	
	public void cardSelected() {
		selectedCard.Card.highlightTiles();
		
	}

}