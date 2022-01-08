using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node2D
{
	public int gCost, hCost,difficultyCost,enemyCost;
	public bool obstacle;
	private Vector3 worldPosition;
	

	public int GridX, GridY;
	public Node2D parent;

	public Tile tile;
	private GameObject enemy = null;


	public void setEnemy(GameObject obj) {
		enemy = obj;
	}

	public GameObject getEnemy() {
		return enemy;
	}


	public Node2D(bool _obstacle, Vector3 _worldPos, int _gridX, int _gridY)
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
				Debug.Log(worldPosition + "has enemy");
			}

			// if (difficultyCost > 0) {
			// 	Debug.Log(gCost + hCost + difficultyCost + enemyCost);
			// }
			return gCost + hCost + difficultyCost + enemyCost;
		}

	}
    

	public void SetObstacle(bool isOb)
	{
		
		obstacle = isOb;
	}

	public Vector3 getWorldPosition() {
		return worldPosition;
	}
}