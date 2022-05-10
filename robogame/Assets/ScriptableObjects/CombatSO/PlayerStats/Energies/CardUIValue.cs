using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardUIValue : ScriptableObject
{
    [SerializeField]
    private int value;

    public new string name;
    public Color color;
    public GameEvent changer;

    public int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            changer.emit();
        }
    }
    

    

    
}