using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AbsDeathEffect : ScriptableObject {
	
	[SerializeField] internal int    damage;
	[SerializeField] internal GoRunTimeSet playerSet;
	[SerializeField] internal GoRunTimeSet tilemapSet;
	[SerializeField]private Vector3IntSet targetPos;
	[SerializeField] internal GameObject formation;
	[SerializeField] internal Vector3IntSet possibleTargets;
	[SerializeField] internal GoRunTimeSet gridGameObject;
	[SerializeField] internal GoRunTimeSet aliveEnemies;
	[SerializeField] internal GoRunTimeSet combatManagerSet;
	[SerializeField] internal GoRunTimeSet aliveObstacles;
	internal abstract Task execute(Vector3 pos);
	
}
