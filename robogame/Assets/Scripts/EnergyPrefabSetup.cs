using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyPrefabSetup : MonoBehaviour

{
    public CardUIValue val;
    void Awake()
    {
        setText();
        gameObject.GetComponent<TextMeshProUGUI>().color = val.color;
    }

    public void setText()
    {
        
        gameObject.GetComponent<TextMeshProUGUI>().SetText(val.Value.ToString());
       
    }
}
