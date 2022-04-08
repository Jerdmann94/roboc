
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "SpecialEnemySpawn", menuName = "EnemyCards/SpawnEnemy")]
public class SpawnSpecialEnemy : AbsAction {
	private int _timer = 4;
	[SerializeField] private GameObject specialObject;
	[SerializeField] private TileMapSo objectData;
	[SerializeField] private GameObject tempSpawnObject;
	
	public override async Task execute(GameObject enemy) {
		_timer = 0;
		var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		var combatManager = combatManagerSet.items.SingleOrDefault(obj => obj.name == "CombatManager")?.GetComponent<CombatManager>();
		if (combatManager != null) {
			var temp = Instantiate(tempSpawnObject,
				combatManager.getEmptyGridPosition(),Quaternion.identity);
			enemyDataHandler.specialTarget = temp;
			temp.GetComponent<TempSpawnScript>().setUp(specialObject,objectData,enemyDataHandler);
			combatManager.GetComponent<EnemyStateManager>().enemiesToLateSpawnList.Add(temp);
		}

		await Task.Delay(100);
	}

	public override bool check(GameObject enemy) {
		var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		_timer++;
		return enemyDataHandler.specialTarget == null && _timer > 3;
	}
}
