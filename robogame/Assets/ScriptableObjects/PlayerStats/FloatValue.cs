using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu]
public class FloatValue : ScriptableObject {
	private float _value;
	public GameObject damText;
	public GoRunTimeSet playerSet;
	public GameEvent changeEmitter;
	public float Value
	{
		get => _value;
		set {
			//Debug.Log(playerSet.items[0].transform.position);
			var tmp = Instantiate(damText, playerSet.items[0].transform.position, quaternion.identity);
			//this next line makes no sense, does not do what i thought it did
			tmp.transform.GetChild(0).GetComponent<TextMeshPro>().color = value > this._value ? Color.red : Color.green;
			tmp.transform.GetChild(0).GetComponent<TextMeshPro>().text = (this._value - value).ToString();
			this._value = value;
			changeEmitter.emit();
		
		}
	}
	
}