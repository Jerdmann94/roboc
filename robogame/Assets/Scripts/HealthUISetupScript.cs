using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUISetupScript : MonoBehaviour {
    public playerStatBlockSO stats;
 
    void Awake()
    {
        setText();
        
    }
    public void setText()
    {
        
        gameObject.GetComponent<TextMeshProUGUI>().SetText(stats.health.Value.ToString());
       
    }
}
