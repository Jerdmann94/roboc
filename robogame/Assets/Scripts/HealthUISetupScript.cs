using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class HealthUISetupScript : MonoBehaviour {
    public PlayerStatBlockSo stats;
 
    void Awake()
    {
        setText();
        
    }
    public void setText()
    {
        
        gameObject.GetComponent<TextMeshProUGUI>().SetText(stats.health.Value.ToString(CultureInfo.CurrentCulture));
       
    }
}
