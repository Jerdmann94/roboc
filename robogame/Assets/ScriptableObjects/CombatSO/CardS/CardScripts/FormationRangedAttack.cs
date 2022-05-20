using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using MyBox;
using ScriptableObjects.Sets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Card", menuName = "PlayerCards/Ranged Formation Attack")]
public class FormationRangedAttack : CardAbs
{
	
	[SerializeField] private Vector3Event attackEvent;
	[ConditionalField("hasAoeAttackFormation")]
	[SerializeField] private Tile attackTile;
	[ConditionalField("hasAoeAttackFormation")]
	[SerializeField] private Vector3IntSet playerAttackHighlight;
	[ConditionalField("nonStraightForm")]
	[SerializeField] private GameObject leftRightForm;
	[ConditionalField("nonStraightForm")]
	[SerializeField] private GameObject upDownForm;
	[SerializeField] private bool spawnTileEffect;
	[ConditionalField("spawnTileEffect")] 
	[SerializeField] private GameObject leftRightTileEffectFormation;
	[ConditionalField("spawnTileEffect")] 
	[SerializeField] private GameObject upDownTileEffectFormation;
	[ConditionalField("spawnTileEffect")] 
	[SerializeField] private GameObject spawnableTileEffect;
	[ConditionalField("spawnTileEffect")]
	[SerializeField] private TileEffectSo tileEffect;
	[SerializeField] private bool shouldThisMove;
	[ConditionalField("shouldThisMove")]
	[SerializeField] private Vector3Event moveEmitter;

	public override void execute() {
		
		Grid2D grid2D = gridManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
		targetPos.items.ForEach(pos => {
			var cellCenter = tilemap.GetCellCenterWorld(pos);
			// GET CARDINAL FOR ROTAION
			Quaternion carRotation = default;
			CardinalDirection card = Cardinal.getCardinalDirection(playerSet.items[0].transform.position, cellCenter);
			carRotation = getRotation(card);
			
			// GET FOR CORRECT FOR ROTATION
			var tempForm = getFormation(card);
			
			// SPAWN FORM TO GET POSSIBLE POSITIONS
			var temp = Instantiate(tempForm,cellCenter, carRotation);
			temp.GetComponent<DecideWhichListsToJoin>().init(WhichTilePosList.PossibleTilePos);
			Destroy(temp);
			foreach (var possiblePos in possibleTilePos.items) {
				attackEvent.emit(tilemap.GetCellCenterWorld(possiblePos));
				if (shouldThisMove) {//IS THIS A MOVING ATTACK
					var node = grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(possiblePos));
					if (node.getEnemy()!= null) {
						node.getEnemy().GetComponent<EnemyDataHandler>().setStun(damage);
						return;
					}

					if (node.getObstacle()!= null) {
						node.getObstacle().setStun(damage);
						return;
					}
					playerSet.items[0].transform.position = node.getWorldPosition(); //MOVING THE ENEMY TO THE NEXT POSITION
					moveEmitter.emit(node.getWorldPosition());
				}
			}
			possibleTilePos.items.Clear();
			
			if (spawnTileEffect) { //TILE EFFECT STUFF - SHOULD BE AFTER MOVEMENT
				
				
				var effectform = Instantiate(getEffectFormation(card), cellCenter, carRotation);
				effectform.GetComponent<DecideWhichListsToJoin>().init(WhichTilePosList.PossibleTilePos);
				Destroy(effectform);
				Debug.Log(possibleTilePos.items.Count);
				foreach (var possiblePos in possibleTilePos.items) {
					var effect = Instantiate(spawnableTileEffect, tilemap.GetCellCenterWorld(possiblePos), quaternion.identity);
					var handler = effect.GetComponent<TileEffectHandler>();
					handler.setupData(tileEffect);
					handler.execute();
				}
				possibleTilePos.items.Clear();
				
			}
			
			
		});
	}

	public override void displayAttackFormation(Vector3Int pos) {
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForPlayer")?.GetComponent<Tilemap>();
		var cellCenter = tilemap.GetCellCenterWorld(pos);
		CardinalDirection card = Cardinal.getCardinalDirection(playerSet.items[0].transform.position, cellCenter);
		Quaternion carRotation = getRotation(card);
		var tempForm = getFormation(card);
		var temp = Instantiate(tempForm,cellCenter, carRotation);
		temp.GetComponent<DecideWhichListsToJoin>().init(WhichTilePosList.PlayerAttackFormationHighlight);
		Destroy(temp);
		foreach (var possiblePos in playerAttackHighlight.items) {
			tilemap.SetTile(possiblePos, attackTile);
		}
		
	}

	public override void removeAttackFormation(Vector3Int pos) {
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForPlayer")?.GetComponent<Tilemap>();
		playerAttackHighlight.items.ForEach(cell => {
			tilemap.SetTile(cell, null);
		});
		playerAttackHighlight.items.Clear();

	}

	private GameObject getFormation(CardinalDirection cardinalDirection) {
		GameObject form = default;
		if (cardinalDirection is CardinalDirection.NorthWest or CardinalDirection.SouthEast) {
				form = upDownForm;
		}
		else {
			form = leftRightForm;
		}
		return form;
	}
	private GameObject getEffectFormation(CardinalDirection cardinalDirection) {
		GameObject form = default;
		if (cardinalDirection is CardinalDirection.NorthWest or CardinalDirection.SouthEast) {
				form = upDownTileEffectFormation;
		}
		else {
			form = leftRightTileEffectFormation;
		}
		return form;
	}

	private static Quaternion getRotation(CardinalDirection card) {
		Quaternion carRotation = default;
		switch (card) {
			case CardinalDirection.NorthEast: // LR ROATION 0
				carRotation = Quaternion.Euler(0,0,0);
				break;
			case CardinalDirection.NorthWest:// UD ROTATION 0
				carRotation = Quaternion.Euler(0,0,0);
				break;
			case CardinalDirection.SouthEast: //LR ROTATION 180
				carRotation = Quaternion.Euler(0,0,180);
				break;
			case CardinalDirection.SouthWest: // UD ROTATION 180
				carRotation = Quaternion.Euler(0,0,180);
				break;
		}

		return carRotation;
	}
}
