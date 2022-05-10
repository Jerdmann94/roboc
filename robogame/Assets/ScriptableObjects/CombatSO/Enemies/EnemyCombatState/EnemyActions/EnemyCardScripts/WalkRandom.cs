using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "new Card", menuName = "EnemyCards/RandomMove")]
public class WalkRandom : AbsAction {
    [SerializeField] private int chance;
    [SerializeField]private Tile moveTile;
    [SerializeField] private Vector3Event emitter;

    public override async Task execute(GameObject enemy) {
        List<Node2D> possibleSpaces = new List<Node2D>();
        Grid2D grid2D = combatManagerSet.items.SingleOrDefault(obj => obj.name == "GridOwner")?.GetComponent<Grid2D>();
        Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
        var pos = tilemap.WorldToCell(enemy.transform.position);
        var pos1 = new Vector3Int(pos.x+1, pos.y, 0);
        var pos2 = new Vector3Int(pos.x-1, pos.y, 0);
        var pos3 = new Vector3Int(pos.x, pos.y+1, 0);
        var pos4 = new Vector3Int(pos.x, pos.y-1, 0);
        possibleSpaces.Add(grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos1)));
        possibleSpaces.Add(grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos2)));
        possibleSpaces.Add(grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos3)));
        possibleSpaces.Add(grid2D.nodeFromWorldPoint(tilemap.GetCellCenterWorld(pos4)));
        
        
        var rnd = new System.Random();
        var randomized = possibleSpaces.OrderBy(item => rnd.Next());
        foreach (var node in randomized) {
            //Debug.Log(node.getWorldPosition());
            if (node.getEnemy() != null || node.getClaimed() || node.obstacle) continue;
            var position = enemy.transform.position;
            grid2D.nodeFromWorldPoint(position).setEnemy(null);
            position = node.getWorldPosition();
            enemy.transform.position = position;
            emitter.emit(position);
            //grid2D.setClaimedAtPosition(enemy, node.getWorldPosition());
            
            break;
        }

        await Task.Delay(100);
    }

    public override bool check(GameObject enemy) {
        return Random.Range(0, 100) <= chance;
    }
    public override void highlight(GameObject enemy, Tile tile) {
        
    }
}
