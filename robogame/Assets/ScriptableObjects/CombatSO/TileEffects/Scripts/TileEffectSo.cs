using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Serialization;

public abstract  class TileEffectSo : ScriptableObject {
	[SerializeField] internal PlayerStatBlockSo stats;
	[SerializeField] internal GoRunTimeSet aliveEnemies;
	[SerializeField] internal GoRunTimeSet aliveObstacles;
	[SerializeField] internal GoRunTimeSet tilemapSet;
	[SerializeField] internal GoRunTimeSet playerSet;
	[SerializeField] internal GoRunTimeSet combatManagerSet;
	[SerializeField] internal bool playerCounter;
	[SerializeField] internal int counter;
	[SerializeField] internal int reactWithFireDamage;
	[SerializeField] internal Color color;
	[SerializeField] internal LabelBase[] labelBases;
	public abstract Task execute(Vector3 vector3);
	public abstract Task reactWithFire(Vector3 vector3);

	public void setEffectNull(Vector3 vector3) {
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();var node = grid2D.nodeFromWorldPoint(vector3);
		if (node.getTileEffect()!= null) {
			Destroy(node.getTileEffect().gameObject);
		}
		node.setTileEffect(null);

	}
	public void setEffectNew(Vector3 vector3, TileEffectHandler tileEffectHandler) {
		
		var grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		var node = grid2D.nodeFromWorldPoint(vector3);
		if (node.getTileEffect()!= null) {
			Destroy(node.getTileEffect().gameObject);
		}
		node.setTileEffect(tileEffectHandler);

	}
}
