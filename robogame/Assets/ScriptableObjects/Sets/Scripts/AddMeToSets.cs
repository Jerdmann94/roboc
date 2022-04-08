using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;

public class AddMeToSets : MonoBehaviour {
	public List<GoRunTimeSet> sets;

	// Start is called before the first frame update
	private void Awake() {
		foreach (var variable in sets) {
			
			variable.add(this.gameObject);
		}
	}

	private void OnDisable() {
		foreach (var variable in sets) {
			variable.remove(this.gameObject);
		}
	}
}