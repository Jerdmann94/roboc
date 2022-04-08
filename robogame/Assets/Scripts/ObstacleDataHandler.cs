using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleDataHandler :TileMapObject {
	public bool killable;
	[SerializeField]
	private ObstacleSo ifNotDefined;
	private void Start() {
		if (ifNotDefined != null) {
			setUpData(ifNotDefined);
		}
		grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		Tilemap tilemap = tilemapSet.items[2].GetComponent<Tilemap>();
		transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position));
		grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position))).obstacle = true;
	}

	public override void takeDamage() {
		damObj = Instantiate(damageText, transform);
		damObj.transform.SetParent(canvas.gameObject.transform);
		damObj.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(selectedCard.Card.doDamage().ToString());
		health += selectedCard.Card.doDamage();
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

	public override void doDeath() {
		Destroy(gameObject);
	}

	public override void setStun(int damage) {
		if (killable) {
			takeDamage(attack);
		}
		
	}

	public  void setUpData(ObstacleSo obstacleSo) {
		killable = obstacleSo.killable;
		
		base.setUpData(obstacleSo);
		
		GetComponent<SpriteRenderer>().sprite = shape;
		
	}

	private void OnDestroy() {
		grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		Tilemap tilemap = tilemapSet.items[2].GetComponent<Tilemap>();
		grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(tilemap.WorldToCell(transform.position))).obstacle = false;
	}
}


