using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CardCost", menuName = "CardCost")]
public class CardCostSo : ScriptableObject
{
    public EnergyValue[] types;
    public int[] cost;

}
