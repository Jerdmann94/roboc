using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Sets {
	[CreateAssetMenu(fileName = "new Card", menuName = "RunTimeSets/CardSet")]
	public class CardSet : ScriptableObject {
		public List<CardAbs> items = new List<CardAbs>();

		public void add(CardAbs t) {
			items.Add(t);
		}

		public void remove(CardAbs t) {
			if (items.Contains(t)) items.Remove(t);
		}
	}
}