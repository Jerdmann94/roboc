using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Sets {
	[CreateAssetMenu]
	public class RunTimeSet : ScriptableObject {
		public List<GameObject> items = new List<GameObject>();

		public void add(GameObject t) {
			if (!items.Contains(t)) items.Add(t);
		}

		public void remove(GameObject t) {
			if (!items.Contains(t)) items.Remove(t);
		}
	}
}