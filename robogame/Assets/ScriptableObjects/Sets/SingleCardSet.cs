using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Sets {
	[CreateAssetMenu]
	public class SingleCardSet : ScriptableObject {
		public CardSO Card { get; set; }
	}
}