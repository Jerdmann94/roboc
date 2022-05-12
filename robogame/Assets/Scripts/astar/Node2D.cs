using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node2D
{
	public int gCost, hCost,difficultyCost,enemyCost;
	public ObstacleDataHandler obstacle;
	private Vector3 worldPosition;
	public int GridX, GridY;
	public Node2D parent;
	private TileEffectHandler _tileEffectHandler;
	
	private Tile tile;
	public Tile Tile {
		get => tile;
		set {
			tile = value;
			
		}
	}

	private GameObject enemy = null;
	private bool claimed = false;

	public void setClaimed(bool claim) {
		claimed = claim;
	}

	public bool getClaimed() {
		return claimed;
	}

	public void setEnemy(GameObject obj) {
		enemy = obj;
	}

	public GameObject getEnemy() {
		return enemy;
	}


	public Node2D(ObstacleDataHandler _obstacle, Vector3 _worldPos, int _gridX, int _gridY)
	{
		obstacle = _obstacle;
		worldPosition = _worldPos;
		GridX = _gridX;
		GridY = _gridY;
		
	}

	public int FCost
	{
		get
		{
			enemyCost =  (enemy==null) ? 100 : 0;
			if (enemy!= null) {
				//Debug.Log(worldPosition + "has enemy");
			}

			// if (difficultyCost > 0) {
			// 	Debug.Log(gCost + hCost + difficultyCost + enemyCost);
			// }
			return gCost + hCost + difficultyCost + enemyCost;
		}

	}

	public int getFCostNoDifficultyOrEnemy() {
		return gCost + hCost;
	}

	public void setObstacle(ObstacleDataHandler isOb) {
		obstacle = isOb;
	}

	public Vector3 getWorldPosition() {
		return worldPosition;
	}

	public ObstacleDataHandler getObstacle() {
		return obstacle;
	}

	public void setTileEffect(TileEffectHandler tileEffectHandler) {
		this._tileEffectHandler = tileEffectHandler;
	}
	public TileEffectHandler getTileEffect() {
		return _tileEffectHandler;
	}
}