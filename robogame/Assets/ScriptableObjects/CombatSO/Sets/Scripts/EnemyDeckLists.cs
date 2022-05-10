using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Card", menuName = "RunTimeSets/EnemyDeckList")]
public class EnemyDeckLists : ScriptableObject {
    public List<EnemyList> enemyLists;
}
