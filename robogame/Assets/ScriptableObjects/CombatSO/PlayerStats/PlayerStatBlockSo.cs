using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class PlayerStatBlockSo : ScriptableObject {
	[SerializeField] public FloatValue health;
	[SerializeField] public LabelBase[] labels;
	public int moveEnergy;
	public int physicalEnergy;
	public int magicEnergy;
	

}
