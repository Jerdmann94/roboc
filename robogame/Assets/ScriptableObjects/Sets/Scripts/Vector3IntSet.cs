using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Sets {
	[CreateAssetMenu]
	public class Vector3IntSet : ScriptableObject {
		public List<Vector3Int> items = new List<Vector3Int>();

		public void add(Vector3Int t) {
			if (!items.Contains(t)) items.Add(t);
		}

		public void remove(Vector3Int t) {
			if (items.Contains(t)) items.Remove(t);
		}
	}
}