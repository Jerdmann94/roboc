using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.UI;

public class enemyDataHandler : MonoBehaviour {
	public     int           cost;
	public new String        name;
	public     int           health;
	public     int           attack;
	public     Sprite        shape;
	public     RunTimeSet    playerSet;
	public     SingleCardSet selectedCard;
	public     Slider        slider;


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
		health -= selectedCard.Card.doDamage();
		slider.value = health;
	}
}