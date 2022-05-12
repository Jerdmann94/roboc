using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SceneChoiceSo : ScriptableObject {
	public ChoiceType choiceType;
}

public enum ChoiceType {
	Combat,
	Elite,
	Shop,
	Scavenger,
	Mystery,
	Boss,
	Smith,
	Void
}
