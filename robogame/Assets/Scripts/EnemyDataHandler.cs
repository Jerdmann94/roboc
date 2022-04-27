using System;
using System.Collections;
using System.Collections.Generic;

using ScriptableObjects.Sets;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDataHandler :TileMapObject {
	
	internal GameObject target;
	internal int moveAmount;
	internal List<Node2D> highlightedNodes = new List<Node2D>();
	private List<Node2D> _path  = null;
	
	internal AbsAction selectedAction;
	internal List<AbsAction> actions;

	internal EnemyAttackType attackType;
	private bool _stunnable = true;
	public bool stunned = false;
	public GameObject stunnedText;
	public GameObject specialTarget;
	public GameObjectEmitter lateDeathEmitter;
	

	public void setPath(List<Node2D> path) {
		//Debug.Log(this.gameObject.name + "'s path is changing to " + path.Count + " data's name " + name );
		this._path = path;
	}

	public List<Node2D> getPath (){
		return _path;
	}



	public  void setUpData(EnemySo enemySo) {
		
		this.moveAmount = enemySo.moveAmount;
		
		this.actions = enemySo.actions;
		
		
		base.setUpData(enemySo);
		
		GetComponent<SpriteRenderer>().sprite = shape;
	
	}

	public override void takeDamage() {
		if (canvas == null) {
			canvas = GameObject.FindWithTag(
				"canvas").GetComponent<Canvas>();
		}
		damObj = Instantiate(damageText, transform);
		damObj.transform.SetParent(canvas.gameObject.transform);
		damObj.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(selectedCard.Card.doDamage().ToString());
		health += selectedCard.Card.doDamage();
		slider.value = health;
		
		
		Destroy(damObj,5f);
		if (health <= 0) {
			doLateDeath();
		}
	}
	public void takeDamage(int damage) {
		if (canvas == null) {
			canvas = GameObject.FindWithTag(
				"canvas").GetComponent<Canvas>();
		}
	   
		if ( health - damage >= slider.maxValue) {
			Debug.Log("changing damage to so that you heal to max health " + gameObject.name + " " + 
			          damage + " " + health + " " + slider.maxValue );
			damage = (int) (slider.maxValue - health);
		}
		damObj = Instantiate(damageText, transform);
		damObj.transform.SetParent(canvas.gameObject.transform);
		damObj.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());

	   
		health -= damage;
		slider.value = health;
		Destroy(damObj,5f);
		if (health <= 0) {
			doLateDeath();
		}
	}

	public override void doDeath() {
		aliveEnemies.remove(this.gameObject);
		if (selectedAction != null) {
			selectedAction.unHighlight(gameObject);
		}
		Debug.Log("doing death");
		Destroy(this.gameObject);
		
	}

	public void doLateDeath() {
		if (selectedAction != null) {
			selectedAction.unHighlight(gameObject);
			selectedAction = null;
		}

		GetComponent<SpriteRenderer>().enabled = false;
		GetComponentInChildren<Canvas>().enabled = false;
		lateDeathEmitter.emit(this.gameObject);
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