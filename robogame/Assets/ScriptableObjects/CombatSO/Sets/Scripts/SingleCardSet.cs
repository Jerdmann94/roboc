using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Sets {
	
	[CreateAssetMenu(fileName = "new Card", menuName = "RunTimeSets/SingleCardSet")]
	public class SingleCardSet : ScriptableObject {
		public GameCard Card { get; set; }
	}
}