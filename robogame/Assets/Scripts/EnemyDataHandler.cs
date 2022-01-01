using System;
using System.Collections;
using System.Collections.Generic;

using ScriptableObjects.Sets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDataHandler : MonoBehaviour {
	private int           cost;
	internal new String        name;
	internal     int           health;
	internal     int           attack;
	internal    Sprite        shape;
	internal int attackRange;
	public     GORunTimeSet   playerSet;
	public     SingleCardSet selectedCard;
	public     Slider        slider;
	public     GameObject    damageText;
	internal GameObject target;
	private GameObject damObj;
	internal Canvas canvas;
	internal int moveAmount;
	internal List<Node2D> highlightedNodes = new List<Node2D>();
	internal List<Node2D> path = null;
	public EnemyCombatState combatState;
	internal TargetTypeSO targetType;
	internal Grid2D grid2D;
	internal AbsAction selectedAction;
	internal List<AbsAction> actions;
	public EnemyTargetFinder targetCheck;
	internal EnemyAttackType attackType;

	private void Start()
	{
		canvas = GameObject.FindWithTag(
			"canvas").GetComponent<Canvas>();
		//setter.target = playerSet.items[0].transform;
	}

	public void setUpEnemy(EnemySO enemySo,Grid2D tempgrid2D) {
		this.cost = enemySo.cost;
		this.name = enemySo.name;
		this.health = enemySo.health;
		this.attack = enemySo.attack;
		this.shape = enemySo.shape;
		this.moveAmount = enemySo.moveAmount;
		this.attackRange = enemySo.attackRange;
		this.actions = enemySo.actions;
		this.targetType = enemySo.targetType;
		this.attackType = enemySo.attackType;
		this.grid2D = tempgrid2D;
		
		
		GetComponent<SpriteRenderer>().sprite = shape;
		slider.value = health;
		
	}

	public void takeDamage() {
		damObj = Instantiate(damageText, transform);
		damObj.transform.SetParent(canvas.gameObject.transform);
		damObj.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(selectedCard.Card.doDamage().ToString());
		health -= selectedCard.Card.doDamage();
		slider.value = health;
		Debug.Log(health);
		
		Destroy(damObj,5f);
	}

}