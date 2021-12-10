using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Sets {
	[CreateAssetMenu]
	public class CardSet : ScriptableObject {
		public List<CardSO> items = new List<CardSO>();

		public void add(CardSO t) {
			items.Add(t);
		}

		public void remove(CardSO t) {
			if (items.Contains(t)) items.Remove(t);
		}
	}
}