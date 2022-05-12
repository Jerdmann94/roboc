using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBox;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

[CreateAssetMenu(fileName = "ETB/RandomAreaAttack/Spawn", menuName = "EnemyCards/ETBorRandAttack")]

public class RandomTileTargetAttack : AbsAction {
    // public AIState State = AIState.None;
    // [ConditionalField(nameof(State), false, AIState.Walk, AIState.Run)] public float Speed;
    [ConditionalField(nameof(_spawnType),false,spawnType.tileEffect)]
    [SerializeField] private TileEffectSo tileEffect;
    [ConditionalField(nameof(_spawnType),false,spawnType.tileEffect)]
    [SerializeField] private GameObject spawnableTileEffect;
    [ConditionalField(nameof(_spawnType),false,spawnType.creature)]
    [SerializeField] private EnemySo enemySo;
    [ConditionalField(nameof(_spawnType),false,spawnType.creature)]
    [SerializeField] private GameObject specialEnemyObject;
    [ConditionalField(nameof(_spawnType),false,spawnType.creature)]
    [SerializeField] private GameObject enemyPrefab;
    [ConditionalField(nameof(_spawnType),false,spawnType.obstacle)]
    [SerializeField] private ObstacleSo obstacleSo;
    [ConditionalField(nameof(_spawnType),false,spawnType.tileEffect)]
    [SerializeField] private GameObject spawnableObstacle;
    [SerializeField] private spawnType _spawnType;
    [SerializeField] private int amountToSpawn;
    [SerializeField] private int oneInX;
    [SerializeField] private Tile summonTile;
    private int _counter = -1;
    

    public override async Task execute(GameObject enemy) {
        _counter = 1;
       Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
        var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        var combatManager = combatManagerSet.items[1].GetComponent<CombatManager>();
        for (int i = 0; i < enemyDataHandler.highlightedNodes.Count; i++) {
            var vec3 = combatManager.getEmptyGridPosition();
            var node = grid2D.nodeFromWorldPoint(vec3);
            spawnObject(node,enemyDataHandler);
        }
        await Task.Yield();
    }

    private void spawnObject(Node2D node,EnemyDataHandler enemyDataHandler) {
        switch (_spawnType) {
            case spawnType.creature:
                spawnCreature(node,enemyDataHandler);
                break;
            case spawnType.obstacle:
                spawnObstacle(node);
                break;
            case spawnType.tileEffect:
                spawnTileEffect(node);
                break;
        }
    }

    private void spawnCreature(Node2D node,EnemyDataHandler enemyDataHandler) {
        var combatManager = combatManagerSet.items[1].GetComponent<CombatManager>();
        var temp = Instantiate(specialEnemyObject,
                    node.getWorldPosition(),Quaternion.identity);
        temp.GetComponent<TempSpawnScript>().setupData(enemyPrefab,enemySo,enemyDataHandler);
        combatManager.GetComponent<EnemyStateManager>().enemiesToLateSpawnList.Add(temp);
    }
    private void spawnObstacle(Node2D node) {
        var temp = Instantiate(spawnableObstacle,
            node.getWorldPosition(),Quaternion.identity);
        temp.GetComponent<ObstacleDataHandler>().setUpData(obstacleSo);
    }
    private void spawnTileEffect(Node2D node) {
        var temp = Instantiate(spawnableTileEffect,
            node.getWorldPosition(),Quaternion.identity);
        temp.GetComponent<TileEffectHandler>().setupData(tileEffect);
    }

    public override async Task<bool> check(GameObject enemy) {
         Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();        var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        var combatManager = combatManagerSet.items.SingleOrDefault(obj => obj.name == "CombatManager")?.GetComponent<CombatManager>();

        var random = new Random();
        if (random.Next(1,oneInX+1) != 1) return false;
        for (int i = 0; i < amountToSpawn; i++) {
            Debug.Log("inside check positive");
            var pos = combatManager.getEmptyGridPosition();
            var node = grid2D.nodeFromWorldPoint(pos);
            enemyDataHandler.highlightedNodes.Add(node);
            grid2D.setClaimedAtPosition(enemy,pos);
        }
        await Task.Yield();
        return true;

    }
    public override async Task  highlight(GameObject enemy, Tile tile) {
        var enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        var tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
        foreach (var node in enemyDataHandler.highlightedNodes) {
            var intPosition = tilemap.WorldToCell(node.getWorldPosition());
            tilemap.SetTile(intPosition,summonTile);
        }

        await Task.Yield();
    }
}

enum spawnType {
    creature,
    obstacle,
    tileEffect
}