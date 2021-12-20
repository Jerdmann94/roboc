using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class MouseHandler : MonoBehaviour {
	//public static MouseHandler  mouseHandler = null;
	private MouseInput    mouse;
	
	public  Tile          targetTile;
	public  Tile          attackTile;
	public  Tile          moveTile;
	//public  GameObject    player;
	public  Vector3IntSet targetPos;
	
	internal List<Tile> possibleTiles;
	internal List<Vector3Int> possibleTilesPos;
	public   Tilemap   map;


	//public        CardSO        selectedCard;
	public CardConfirmed cc;
	public CardSelected  cs;

	[SerializeField] private BoolValue playPhase;

	public SingleCardSet selectedCardSet;

	private void Awake() {
		mouse = new MouseInput();
		possibleTiles = new List<Tile>();
		
	}

	private void OnEnable() {
		mouse.Enable();
	}

	private void OnDisable() {
		mouse.Disable();
	}

	void Start() {
		if (mouse != null) mouse.Mouse.MouseClick.performed += data => MouseClick();
	}

	private void MouseClick() {
		Vector2 mousePosition = mouse.Mouse.MousePosition.ReadValue<Vector2>();
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		Vector3Int gridPos = map.WorldToCell(mousePosition);
		Tile       tile    = map.GetTile<Tile>(gridPos);
		if (tile == null || possibleTiles.Contains(tile)) return;
		Vector3Int temp = new Vector3Int(gridPos.x, gridPos.y, 0);
		map.SetTile(temp, targetTile);
		targetPos.items.Insert(0,gridPos);
		checkLastTile();
	}

	

	//CARD/TILE UTILITY METHODS

	public void confirm() {
		if (!playPhase.Value) return;
		cc.cardConfirmed();
		resetTiles();
		targetPos.items = new List<Vector3Int>();
	}

	public void playCard(CardSO card) {
		if (!playPhase.Value) return;
		
		resetTiles();
		//selectedCard = card;
		selectedCardSet.Card = card;
   
		cs.cardSelected();
	}


	private void resetTiles() {
		for (int i = 0; i < possibleTiles.Count; i++) {
			map.SetTile((Vector3Int) possibleTilesPos[i], (TileBase) possibleTiles[i]);
		}
	}
	private void checkLastTile() {
		if (selectedCardSet.Card.targets >= targetPos.items.Count) {return;}
		for (int i = targetPos.items.Count-1; i >= selectedCardSet.Card.targets; i--) {
			map.SetTile(targetPos.items[i], selectedCardSet.Card.tileColor);
			targetPos.items.RemoveAt(i);
		}
	}
}