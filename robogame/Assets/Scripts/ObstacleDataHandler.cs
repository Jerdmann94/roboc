using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleDataHandler :TileMapObject {
	public bool killable;
	[SerializeField]
	private ObstacleSo ifNotDefined;
	public AbsDeathEffect deathEffect;
	public bool pushable;
	public int damageWhenPushed;

	private void Start() {
		if (ifNotDefined != null) {
			Debug.Log(ifNotDefined);
			setUpData(ifNotDefined);
		}
		
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();Tilemap tilemap = tilemapSet.items[2].GetComponent<Tilemap>();
		Transform transform1;
		(transform1 = transform).position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
		grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform1.position))).obstacle = this;
	}

	public override void takeDamage() {
		if (canvas == null) {
			canvas = GameObject.FindWithTag(
				"canvas").GetComponent<Canvas>();
		}
		damObj = Instantiate(damageText, transform);
		//Debug.Log(canvas);
		damObj.transform.SetParent(canvas.gameObject.transform);
		damObj.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(selectedCard.Card.card.doDamage().ToString());
		health += selectedCard.Card.card.doDamage();
		slider.value = health;
		
		
		Destroy(damObj,5f);
		if (health <= 0) {
			doDeath();
		}
	}

	public void takeDamage(int damage) {
		if (canvas == null) {
			canvas = GameObject.FindWithTag(
				"canvas").GetComponent<Canvas>();
		}
	   
		if ( damage + health >= slider.maxValue) {
			damage = (int) (slider.maxValue - health);
		}
		damObj = Instantiate(damageText, transform);
		damObj.transform.SetParent(canvas.gameObject.transform);
		damObj.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(damage.ToString());

	   
		health += damage;
		slider.value = health;
		Destroy(damObj,5f);
		if (health <= 0) {
			doDeath();
		}
	}

	public override async void doDeath() {
		if (deathEffect!= null) {
			await deathEffect.execute(gameObject.transform.position);
		}
		Destroy(gameObject);
	}

	public override void setStun(int damage) {
		if (killable) {
			takeDamage(damage);
		}
		
	}

	internal void setUpData(ObstacleSo obstacleSo) {
		Debug.Log(shape);
		killable = obstacleSo.killable;
		pushable = obstacleSo.pushable;
		damageWhenPushed = obstacleSo.damageWhenPushed;
		deathEffect = obstacleSo.deathEffect;
		
		base.setUpData(obstacleSo);
		
		GetComponent<SpriteRenderer>().sprite = shape;
		
		
	}

	private void OnDestroy() {
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();Tilemap tilemap = tilemapSet.items[2].GetComponent<Tilemap>();
		grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position))).obstacle = null;
	}

	
}


