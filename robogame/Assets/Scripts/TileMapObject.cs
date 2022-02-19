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
	private GameObject _damObj;
	public    int           health;
	public     Slider        slider;
	internal     int           attack;
	internal    Sprite        shape;
	internal new String        name;
	internal Grid2D grid2D;
	public GORunTimeSet combatManagerSet;
	public GORunTimeSet tilemapSet;
	private void Start() {

		
		canvas = GameObject.FindWithTag(
			"canvas").GetComponent<Canvas>();
		
		//setter.target = playerSet.items[0].transform;
	}
   public void takeDamage(int damage) {
	   if (canvas == null) {
		   canvas = GameObject.FindWithTag(
			   "canvas").GetComponent<Canvas>();
	   }
	   _damObj = Instantiate(damageText, transform);
	   _damObj.transform.SetParent(canvas.gameObject.transform);
	   _damObj.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());
      
	   Destroy(_damObj,5f);
   }
   public void takeDamage() {
	   _damObj = Instantiate(damageText, transform);
	   _damObj.transform.SetParent(canvas.gameObject.transform);
	   _damObj.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(selectedCard.Card.doDamage().ToString());
	   health -= selectedCard.Card.doDamage();
	   slider.value = health;
		
		
	   Destroy(_damObj,5f);
   }

   protected void setUpData(TileMapSO tileMapSo) {
	   
	   //this.name = enemySo.name;
	   this.health = tileMapSo.health;
	   this.attack = tileMapSo.attack;
	   this.shape = tileMapSo.shape;
	   slider.value = health;
	   
   }

   public abstract void setStun(int damage);

}
