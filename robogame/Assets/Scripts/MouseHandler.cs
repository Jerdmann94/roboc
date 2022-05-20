using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;


[Serializable]
public class MouseHandler : MonoBehaviour {
	//public static MouseHandler  mouseHandler = null;
	private MouseInput    _mouse;
	
	public  Tile          targetTile;
	
	//public  GameObject    player;
	public  Vector3IntSet targetPos;
	public Vector3IntSet possibleTilesPos;
	public   Tilemap   playerMap;
	public TileRunTimeSet possibleTileSet;


	//public        CardSO        selectedCard;
	public CardConfirmed cc;
	public CardSelected  cs;

	[SerializeField] private BoolValue playPhase;

	public SingleCardSet selectedCardSet;

	private void Awake() {
		_mouse = new MouseInput();
		possibleTileSet.items = new List<Tile>();
		targetPos.items = new List<Vector3Int>();

	}

	private void OnEnable() {
		_mouse.Enable();
	}

	private void OnDisable() {
		_mouse.Disable();
	}

	void Start() {
		if (_mouse != null) _mouse.Mouse.MouseClick.performed += data => mouseClick();
	}

	private void mouseClick() {
		Vector2 mousePosition = _mouse.Mouse.MousePosition.ReadValue<Vector2>();
		if (Camera.main != null) mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		Vector3Int gridPos = playerMap.WorldToCell(mousePosition);
		Tile       tile    = playerMap.GetTile<Tile>(gridPos);
		// if (tile == null) {
		// 	Debug.Log("tile = null " + playerMap.name);
		// 	return;
		// }

		if (!possibleTilesPos.items.Contains(gridPos)) {
			//Debug.Log("possible tiles does not contain tile " + possibleTileSet.items.Count + " grid position is " + gridPos);

			return;
		}

		if (selectedCardSet.Card.card.hasAoeAttackFormation) {
			removeAttackFormation(gridPos);
			displayAttackFormation(gridPos);
		}
		Vector3Int temp = new Vector3Int(gridPos.x, gridPos.y, 0);
		playerMap.SetTile(temp, targetTile);
		targetPos.items.Insert(0,gridPos);
		checkLastTile();
	}

	private void removeAttackFormation(Vector3Int pos) {
		selectedCardSet.Card.card.removeAttackFormation(pos);
	}

	private void displayAttackFormation(Vector3Int pos) {
		selectedCardSet.Card.card.displayAttackFormation(pos);
	}


	//CARD/TILE UTILITY METHODS

	public void confirm() {
		if (!playPhase.value) return;
		if (targetPos.items.Count < 1) return;
		cc.cardConfirmed();
		resetTiles();
		targetPos.items = new List<Vector3Int>();
	}

	public void playCard(GameCard gameCard) {
		if (!playPhase.value) return;
		
		resetTiles();
		//selectedCard = card;
		selectedCardSet.Card = gameCard;
   
		cs.cardSelected();
	}


	public void resetTiles() {
		// for (int i = 0; i < possibleTileSet.items.Count; i++) {
		// 	playerMap.SetTile((Vector3Int) possibleTilesPos.items[i], (TileBase) possibleTileSet.items[i]);
		// }
		playerMap.ClearAllTiles();

		//Debug.Log("reseting tiles");
		possibleTileSet.items = new List<Tile>();
		possibleTilesPos.items = new List<Vector3Int>();
		
	}
	private void checkLastTile() {
		//Debug.Log(selectedCardSet.Card);
		//Debug.Log(targetPos.items);
		if (selectedCardSet.Card.card.targets >= targetPos.items.Count) {return;}
		for (int i = targetPos.items.Count-1; i >= selectedCardSet.Card.card.targets; i--) {
			playerMap.SetTile(targetPos.items[i], selectedCardSet.Card.card.tileColor);
			targetPos.items.RemoveAt(i);
		}
	}
}