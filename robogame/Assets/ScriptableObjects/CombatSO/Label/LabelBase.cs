using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LabelBase", menuName = "Label/Base")]

public class LabelBase : ScriptableObject {
	public LabelType labelType;
}

public enum LabelType {
	SludgeDrinker,
	Flammable,
	Stunned,
	Igniting,
	Bleeding,
	Armored,
	DoubleHealer,

}