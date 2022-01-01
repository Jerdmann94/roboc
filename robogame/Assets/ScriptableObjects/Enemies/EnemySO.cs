using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName ="new Enemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject {
    public     int         cost;
    public new String      name;
    public     int         health;
    public     int         attack;
    public     Sprite      shape;
    public int moveAmount;
    public int attackRange;
    public List<AbsAction> actions;
    public EnemyAttackType attackType;
    public TargetTypeSO targetType;
}

