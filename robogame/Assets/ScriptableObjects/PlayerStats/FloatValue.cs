using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu]
public class FloatValue : ScriptableObject {
	public float value;

	public GameObject damText;
	public GORunTimeSet playerSet;
	public float Value
	{
		get => value;
		set {
			var tmp = Instantiate(damText, playerSet.items[0].transform.position, quaternion.identity);
			//stunObj.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.black;
			tmp.transform.GetChild(0).GetComponent<TextMeshPro>().color = value > this.value ? Color.red : Color.green;
			tmp.transform.GetChild(0).GetComponent<TextMeshPro>().text = (this.value - value).ToString();
			this.value = value;
			
		}
	}
	
}