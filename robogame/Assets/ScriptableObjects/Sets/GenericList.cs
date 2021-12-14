using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Sets {
	[CreateAssetMenu]
	public class GenericList<T> : ScriptableObject {
		public List<T> items = new List<T>();

		public void add(T t) {
			if (!items.Contains(t)) items.Add(t);
		}

		public void remove(T t) {
			if (!items.Contains(t)) items.Remove(t);
		}
	}
}