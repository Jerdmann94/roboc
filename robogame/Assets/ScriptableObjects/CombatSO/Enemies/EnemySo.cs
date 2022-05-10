using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName ="new Enemy", menuName = "Enemy")]
public class EnemySo : TileMapSo {
    // public     int         cost;
    public int moveAmount;

    // public int attackRange;
    public List<AbsAction> actions;

    public bool hasDeathEffect;

    [ConditionalField("hasDeathEffect")]public AbsDeathEffect deathEffect;
    
    // public EnemyAttackType attackType;
    // public TargetTypeSo targetType;
}

