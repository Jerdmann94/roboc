using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class enemyDataHandler : MonoBehaviour {
	public     int           cost;
	public new String        name;
	public     int           health;
	public     int           attack;
	public     Sprite        shape;
	public     GORunTimeSet   playerSet;
	public     SingleCardSet selectedCard;
	public     Slider        slider;
	public     GameObject    damageText;
	private GameObject damObj;
	public Canvas canvas;


	private void Start()
	{
		canvas = GameObject.FindWithTag(
			"canvas").GetComponent<Canvas>();
	}

	public void setUpEnemy(EnemySO enemySo) {
		this.cost = enemySo.cost;
		this.name = enemySo.name;
		this.health = enemySo.health;
		this.attack = enemySo.attack;
		this.shape = enemySo.shape;
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