using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class TileMapObject: MonoBehaviour {
	
	public     SingleCardSet selectedCard;
	public     GameObject    damageText;
	protected Canvas canvas;
	protected GameObject damObj;
	public    int           health;
	public     Slider        slider;
	//internal     int           attack;
	internal    Sprite        shape;
	internal new String        name;
	internal Grid2D grid2D;
	public GoRunTimeSet combatManagerSet;
	public GoRunTimeSet tilemapSet;
	public GoRunTimeSet aliveEnemies;
	public LabelBase[] labelBases;
	
	
	private float TOLERANCE = .2f;
	private void Start() {

		
		canvas = GameObject.FindWithTag(
			"canvas").GetComponent<Canvas>();
		
		//setter.target = playerSet.items[0].transform;
	}
   

   public abstract void takeDamage();

	   protected void setUpData(TileMapSo tileMapSo) {
	   
	   //this.name = enemySo.name;
	   this.health = tileMapSo.health;
	   //this.attack = tileMapSo.attack;
	   this.shape = tileMapSo.shape;
	   labelBases = tileMapSo.labelBases;
	   slider.maxValue = health;
	   slider.value = health;
	   
   }

   public abstract void doDeath();
   public abstract void setStun(int damage);

}
