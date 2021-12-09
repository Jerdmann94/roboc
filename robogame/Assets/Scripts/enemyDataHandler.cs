using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDataHandler : MonoBehaviour
{
    public     int    cost;
    public new String name;
    public     int    health;
    public     int    attack;
    public     Sprite shape;
    
    public void setUpEnemy(EnemySO enemySo) {
        this.cost = enemySo.cost;
        this.name = enemySo.name;
        this.health = enemySo.health;
        this.attack = enemySo.attack;
        this.shape = enemySo.shape;
        GetComponent<SpriteRenderer>().sprite = shape;
    }

    public void takeDamage(int doDamage) {
        this.health -= doDamage;
    }
}
