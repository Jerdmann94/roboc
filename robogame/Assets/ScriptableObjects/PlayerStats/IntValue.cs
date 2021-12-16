using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntValue : ScriptableObject
{

    private int value;

    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            changer.emit(); 
            this.value = value;
        }
    }
    

    public GameEvent changer;

    
}