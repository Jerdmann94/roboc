using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffectHandler : MonoBehaviour {
	private TileEffectSo tileEffectSo;
	private int _counter;
	public new String name;
	public LabelBase[] labels;
	public void setupData(TileEffectSo tileEffect) {
		var sr = GetComponent<SpriteRenderer>();
		sr.color = tileEffect.color;
		this.tileEffectSo = tileEffect;
		this.labels = tileEffect.labelBases;
		_counter = tileEffect.counter;
		name = tileEffect.name;
		tileEffectSo.setEffectNew(this.transform.position,this);
		
	}

	public void execute() {
		tileEffectSo.execute(this.transform.position);
	}

	public void enemyCounterEmitted() {
		if (tileEffectSo.playerCounter) return;
		_counter--;
		execute();
		if (_counter < 0) {
			doDeath();
		}
	}
	public void playerCounterEmitted() {
		if (!tileEffectSo.playerCounter) return;
		_counter--;
		execute();
		if (_counter < 0) {
			doDeath();
		}
	}
	public void doDeath() {
		this.tileEffectSo.setEffectNull(this.transform.position);
		//Debug.Log("destroying " + gameObject.name);
		Destroy(gameObject);
	}

	public void reactWithFire() {
		tileEffectSo.reactWithFire(this.transform.position);
	}

	public bool checkLabel(LabelType labelType) {
		foreach (var label in labels) {
			if (label.labelType == labelType) {
				return true;
			}
		}
		return false;
	}
}
