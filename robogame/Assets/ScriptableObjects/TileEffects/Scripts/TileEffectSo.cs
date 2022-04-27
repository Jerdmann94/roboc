using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Serialization;

public abstract  class TileEffectSo : ScriptableObject {
	[SerializeField] internal PlayerStatBlockSo stats;
	[SerializeField] internal GoRunTimeSet aliveEnemies;
	[SerializeField] internal GoRunTimeSet aliveObstacles;
	[SerializeField] internal GoRunTimeSet tilemapSet;
	[SerializeField] internal GoRunTimeSet combatManagerSet;
	[SerializeField] internal bool playerCounter;
	[SerializeField] internal int counter;
	[SerializeField] internal int reactWithFireDamage;
	[SerializeField] internal Color color;
	public abstract Task execute(Vector3 vector3);
	public abstract Task reactWithFire(Vector3 vector3);

	public void setEffectNull(Vector3 vector3) {
		var grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		var node = grid2D.nodeFromWorldPoint(vector3);
		node.setTileEffect(null);

	}
	public void setEffectNew(Vector3 vector3, TileEffectHandler tileEffectHandler) {
		var grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		var node = grid2D.nodeFromWorldPoint(vector3);
		node.setTileEffect(tileEffectHandler);

	}
}
