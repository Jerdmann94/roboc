using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Sets {
	[CreateAssetMenu(fileName = "new Card", menuName = "RunTimeSets/CardSet")]
	public class CardSet : ScriptableObject {
		public List<GameCard> items;

		public void add(GameCard t) {
			items.Add(t);
		}

		public void remove(GameCard t) {
			if (items.Contains(t)) items.Remove(t);
		}
	}
}