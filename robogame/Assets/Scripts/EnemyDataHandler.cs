using System;
using System.Collections;
using System.Collections.Generic;

using ScriptableObjects.Sets;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDataHandler :TileMapObject {
	internal int           cost;
	internal int attackRange;
	internal GameObject target;
	internal int moveAmount;
	internal List<Node2D> highlightedNodes = new List<Node2D>();
	private List<Node2D> path  = null;
	internal TargetTypeSO targetType;
	internal AbsAction selectedAction;
	internal List<AbsAction> actions;
	public EnemyTargetFinder targetCheck;
	internal EnemyAttackType attackType;
	private bool _stunnable = true;
	public bool stunned = false;
	public GameObject stunnedText;
	public void setPath(List<Node2D> path) {
		//Debug.Log(this.gameObject.name + "'s path is changing to " + path.Count + " data's name " + name );
		this.path = path;
	}

	public List<Node2D> getPath (){
		return path;
	}



	public  void setUpData(EnemySO enemySo) {
		this.cost = enemySo.cost;
		this.moveAmount = enemySo.moveAmount;
		this.attackRange = enemySo.attackRange;
		this.actions = enemySo.actions;
		this.targetType = enemySo.targetType;
		this.attackType = enemySo.attackType;
		
		base.setUpData(enemySo);
		
		GetComponent<SpriteRenderer>().sprite = shape;
	
	}

	public override void setStun(int damage) {
		if (_stunnable) {
			
			var stunObj = Instantiate(stunnedText, transform);
			stunObj.transform.SetParent(canvas.gameObject.transform);
			stunObj.transform.GetChild(0).GetComponent<TextMeshPro>().SetText("STUNNED");
			stunObj.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.black;
			Destroy(stunObj,2f);
			stunned = true;
		}
		takeDamage();
	}
}