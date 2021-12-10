using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;

public class AddMeToSets : MonoBehaviour {
	public List<RunTimeSet> sets;

	// Start is called before the first frame update
	private void Awake() {
		foreach (var VARIABLE in sets) {
			VARIABLE.add(this.gameObject);
		}
	}

	private void OnDisable() {
		foreach (var VARIABLE in sets) {
			VARIABLE.remove(this.gameObject);
		}
	}
}