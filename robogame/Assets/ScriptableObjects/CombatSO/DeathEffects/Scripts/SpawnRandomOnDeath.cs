using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "new Card", menuName = "DeathEffects/Spawn Enemies Random Locations")]

public class SpawnRandomOnDeath : AbsDeathEffect
{

	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private EnemySo[] enemyData;
	[SerializeField] private GameObject tempSpawnObject;
	[SerializeField] private int amountToSpawn = 1;
	[SerializeField] private bool areTheEnemiesAllTheSame;
	internal override async Task execute(Vector3 pos) {
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
		var combatManager = combatManagerSet.items[1].GetComponent<CombatManager>();
		var spawnerDataHandler = grid2D.nodeFromWorldPoint(pos).getEnemy().GetComponent<EnemyDataHandler>();
		for (int i = 0; i < amountToSpawn; i++) {
			var toSpawn = areTheEnemiesAllTheSame ? enemyData[0] : enemyData[i];
			var node = grid2D.nodeFromWorldPoint(combatManager.getEmptyGridPosition());
			var temp = Instantiate(tempSpawnObject,
				node.getWorldPosition(),Quaternion.identity);
			temp.GetComponent<TempSpawnScript>().setupData(enemyPrefab,toSpawn,spawnerDataHandler);
			combatManager.GetComponent<EnemyStateManager>().enemiesToLateSpawnList.Add(temp);
		}
		
		await Task.Yield();
	}
}
