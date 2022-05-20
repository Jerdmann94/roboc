using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Sets;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu]
public class FloatValue : ScriptableObject {
	public GameObject damText;
	public GoRunTimeSet playerSet;
	public GameEvent changeEmitter;
	public PlayerStatBlockSo stats;

	public float Value { get; set; }

	public void takeDamage(int damage) {
		var removeArmor = false;
		stats.labels.ForEach(lBase => {
			if (lBase.labelType == LabelType.Armored && damage > 0) {
				damage = 1;
				removeArmor = true;
			}
		});
		if (removeArmor) {
			var temp = stats.labels.FirstOrDefault(lBase => lBase.labelType == LabelType.Armored);
			stats.labels.Remove(temp);
		}
		var tmp = Instantiate(damText, playerSet.items[0].transform.position, quaternion.identity);
		//this next line makes no sense, does not do what i thought it did
		//tmp.transform.GetChild(0).GetComponent<TextMeshPro>().color = damage > this._value ? Color.red : Color.green;
		tmp.transform.GetChild(0).GetComponent<TextMeshPro>().text = (damage).ToString();
		Value += damage;
		changeEmitter.emit();
	}
}