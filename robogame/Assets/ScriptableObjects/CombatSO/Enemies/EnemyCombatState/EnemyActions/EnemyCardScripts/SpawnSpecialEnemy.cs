
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "SpecialEnemySpawn", menuName = "EnemyCards/SpawnEnemy")]
public class SpawnSpecialEnemy : AbsAction {
	[SerializeField] private int _timer = 4;
	[SerializeField] private GameObject specialObject;
	[SerializeField] private TileMapSo objectData;
	[SerializeField] private GameObject tempSpawnObject;
	[SerializeField] private int amountToSpawn = 1;
	[SerializeField] private Tile summonTile;
	private int counter;
	
	public override async Task execute(GameObject enemy) {
		counter = 0;
		var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		var combatManager = combatManagerSet.items.SingleOrDefault(obj => obj.name == "CombatManager")?.GetComponent<CombatManager>();
		if (combatManager != null) {
			for (int i = 0; i < amountToSpawn; i++) {
				var temp = Instantiate(tempSpawnObject,
					enemyDataHandler.highlightedNodes[i].getWorldPosition(),Quaternion.identity);
				enemyDataHandler.specialTarget = temp;
				temp.GetComponent<TempSpawnScript>().setupData(specialObject,objectData,enemyDataHandler);
				combatManager.GetComponent<EnemyStateManager>().enemiesToLateSpawnList.Add(temp);
			}
			
		}

		await Task.Yield();
	}

	public override async Task<bool> check(GameObject enemy) {
		Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		var combatManager = combatManagerSet.items.SingleOrDefault(obj => obj.name == "CombatManager")?.GetComponent<CombatManager>();

		counter++;
		if (enemyDataHandler.specialTarget == null && counter > _timer) {
			for (int i = 0; i < amountToSpawn; i++) {
				var pos = combatManager.getEmptyGridPosition();
				var node = grid2D.nodeFromWorldPoint(pos);
				enemyDataHandler.highlightedNodes.Add(node);
				grid2D.setClaimedAtPosition(enemy,pos);
			}
			await Task.Yield();
			return true;
		}
		await Task.Yield();
		return false;


	}

	public override async Task  highlight(GameObject enemy, Tile tile) {
		var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
		Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
		foreach (var node in enemyDataHandler.highlightedNodes) {
			
			var intPosition = tilemap.WorldToCell(node.getWorldPosition());
			tilemap.SetTile(intPosition,summonTile);
		}

		await Task.Yield();
	}
}
