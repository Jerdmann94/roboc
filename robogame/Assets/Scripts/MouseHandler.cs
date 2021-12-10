using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class MouseHandler : MonoBehaviour {
	//public static MouseHandler  mouseHandler = null;
	private  MouseInput mouse;
	public   Tile       selectedTile;
	public   Tile       targetTile;
	public   Tile       attackTile;
	public   Tile       moveTile;
	public   GameObject player;
	internal Vector3Int targetPos;

	internal ArrayList possibleTiles;
	internal ArrayList possibleTilesPos;
	public   Tilemap   map;

	public GameEvent cardConfirmed;

	public GameEvent cardSelected;

	//public        CardSO        selectedCard;
	public CardConfirmed cc;
	public CardSelected  cs;


	public SingleCardSet selectedCardSet;

	private Vector3Int TargetPos {
		get => targetPos;
		set {
			targetPos = value;
			print(value);
		}
	}


	private void Awake() {
		mouse = new MouseInput();
		possibleTiles = new ArrayList();
		//cc = new CardConfirmed();
		// cs = new CardSelected();
		player = Instantiate(player, new Vector3(0.75f, 0, 0), Quaternion.identity);
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
		print(gridPos);
		TargetPos = gridPos;
	}

	//CARD/TILE UTILITY METHODS

	public void confirm() {
		cc.cardConfirmed();

		resetTiles();
	}

	public void playCard(CardSO card) {
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
}