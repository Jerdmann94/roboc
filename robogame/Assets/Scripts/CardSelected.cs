using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.GameEvent;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CardSelected : MonoBehaviour {
	private readonly int[,] vectorChanger4cardinal = new int[,] {{1, 0}, {0, -1}, {0, 1}, {-1, 0}};

	private readonly int[,] vectorChanger16 = new int[,] {
		                                                     {1, 0}, {2, 0}, {0, -1}, {0, -2}, {0, 1}, {0, 2}, {-1, 0},
		                                                     {-2, 0},
		                                                     {1, -1}, {2, -1}, {-1, -1}, {-1, -2}, {1, 1}, {1, 2},
		                                                     {-1, 1}, {-2, 1},
		                                                     {1, -2}, {2, 1}, {-2, -1}, {-1, 2}
	                                                     };

	public SingleCardSet selectedCard;
	public MouseHandler  mouseHandler;

	public void cardSelected() {
		Debug.Log("card selected");
		switch (selectedCard.Card.name) {
			case "Move":

				basicMove();
				break;
			case "Attack":

				basicAttack();
				break;
			case "Special Move":
				specialMove();
				break;

			case "Special Attack":
				specialAttack();
				break;

			default:
				Debug.Log("default case");
				break;
		}
	}

	private void specialAttack() {
		highlight20Tiles(mouseHandler.attackTile);
	}

	private void specialMove() {
		highlight20Tiles(mouseHandler.moveTile);
	}

	private void basicMove() {
		highlightCells4Cardinals(mouseHandler.moveTile);
	}

	private void basicAttack() {
		highlightCells4Cardinals(mouseHandler.attackTile);
	}

	//HIGHLIGHTING TILES METHODS
	private void highlightCells4Cardinals(Tile tile) {
		mouseHandler.possibleTiles = new ArrayList();
		mouseHandler.possibleTilesPos = new ArrayList();
		Vector3 pos = mouseHandler.player.transform.position;
		for (int i = 0; i < 4; i++) {
			Vector3Int gridPos = mouseHandler.map.WorldToCell(pos);
			gridPos.x = gridPos.x + vectorChanger4cardinal[i, 0];
			gridPos.y = gridPos.y + vectorChanger4cardinal[i, 1];
			if (mouseHandler.map.HasTile(gridPos)) {
				mouseHandler.possibleTiles.Add(mouseHandler.map.GetTile<Tile>(gridPos));
				mouseHandler.possibleTilesPos.Add(gridPos);
				mouseHandler.map.SetTile(gridPos, tile);
			}
			else {
				Debug.Log("tile not found");
			}
		}
	}

	private void highlight20Tiles(Tile tile) {
		mouseHandler.possibleTiles = new ArrayList();
		mouseHandler.possibleTilesPos = new ArrayList();
		Vector3 pos = mouseHandler.player.transform.position;
		for (int i = 0; i < 20; i++) {
			Vector3Int gridPos = mouseHandler.map.WorldToCell(pos);
			gridPos.x = gridPos.x + vectorChanger16[i, 0];
			gridPos.y = gridPos.y + vectorChanger16[i, 1];
			if (mouseHandler.map.HasTile(gridPos)) {
				mouseHandler.possibleTiles.Add(mouseHandler.map.GetTile<Tile>(gridPos));
				mouseHandler.possibleTilesPos.Add(gridPos);
				mouseHandler.map.SetTile(gridPos, tile);
			}
			else {
				Debug.Log("tile not found");
			}
		}
	}
}