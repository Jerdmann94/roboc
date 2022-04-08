using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.Sets;
using UnityEngine;

public class TempSpawnScript : MonoBehaviour {
  [SerializeField] private GoRunTimeSet combatManagerSet;
  private GameObject tileMapObject;
  private TileMapSo tileMapSo;
  private EnemyDataHandler spawner;

  public void setUp(GameObject tileMapObject, TileMapSo tileMapSo, EnemyDataHandler spawner) {
    this.tileMapObject = tileMapObject;
    this.tileMapSo = tileMapSo;
    this.spawner = spawner;
    if (combatManagerSet != null) {
      var combatManager = combatManagerSet.items.SingleOrDefault(obj => obj.name == "CombatManager")
        ?.GetComponent<CombatManager>();
    }
  }
  

  public GameObject spawnEnemy() {
    GameObject temp = Instantiate(tileMapObject,
      transform.position,Quaternion.identity);
    spawner.specialTarget = temp;
    temp.GetComponent<EnemyDataHandler>().setUpData(tileMapSo as EnemySo);
    Destroy(this.gameObject,5f);
    return temp;
  }
}
