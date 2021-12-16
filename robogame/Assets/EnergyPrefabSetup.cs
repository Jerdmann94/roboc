using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyPrefabSetup : MonoBehaviour

{
    public IntValue val;
    void Awake()
    {
        gameObject.GetComponent<TextMeshProUGUI>().SetText(val.Value.ToString());
        Debug.Log(val.Value);
    }
}
