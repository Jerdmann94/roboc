using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AbsAction:ScriptableObject {
    
    public GORunTimeSet combatManagerSet;
    public  abstract void Execute(GameObject enemy);
    public abstract bool Check(GameObject enemy);

    public virtual void Highlight(GameObject enemy,Tile tile) {
        
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        
        GameObject gridOwner = combatManagerSet.items[0];
        Grid2D grid2D = gridOwner.GetComponent<Grid2D>();
        Tilemap tilemap = grid2D.defaultTileMap;
        int moveAmount = enemy.GetComponent<EnemyDataHandler>().moveAmount;
        for (int i = 0; i < moveAmount; i++) {
            tilemap.SetTile(tilemap.WorldToCell(enemyDataHandler.getPath()[i].getWorldPosition()),tile);
            enemyDataHandler.highlightedNodes.Add(enemyDataHandler.getPath()[i]);
            //grid2D.setEnemyAtPosition(enemy,enemyDataHandler.getPath()[i].getWorldPosition());
        }
    }

    public virtual void unHighlight(GameObject enemy) {
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        
        GameObject gridOwner = combatManagerSet.items[0];
        Grid2D grid2D = gridOwner.GetComponent<Grid2D>();
        Tilemap tilemap = grid2D.defaultTileMap;
        foreach (var node in enemyDataHandler.highlightedNodes) {
            tilemap.SetTile(tilemap.WorldToCell(node.getWorldPosition()),node.tile);
        }

        enemyDataHandler.highlightedNodes = new List<Node2D>();
    }
    
    public virtual void returnTileUnderEnemy(GameObject enemy) {
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        GameObject gridOwner = combatManagerSet.items[0];
        Grid2D grid2D = gridOwner.GetComponent<Grid2D>();
        Tilemap tilemap = grid2D.defaultTileMap;
        var position = enemy.transform.position;
        Vector3Int finalPos = tilemap.WorldToCell(position);
        Node2D finalNode = grid2D.NodeFromWorldPoint(position);
        tilemap.SetTile(finalPos,finalNode.tile );
    }
    
}
