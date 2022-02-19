using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AbsAction:ScriptableObject {
    
    public GORunTimeSet combatManagerSet;
    public GORunTimeSet playerSet;
    public GORunTimeSet tilemapSet;
    public   abstract Task Execute(GameObject enemy);
    public abstract bool Check(GameObject enemy);

    public virtual void Highlight(GameObject enemy,Tile tile) {
        
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        
        
        GameObject gridOwner = combatManagerSet.items[0];
        Grid2D grid2D = gridOwner.GetComponent<Grid2D>();
        
        Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
        
        int moveAmount = enemy.GetComponent<EnemyDataHandler>().moveAmount;
        for (int i = 0; i < moveAmount; i++) {
            tilemap.SetTile(tilemap.WorldToCell(enemyDataHandler.getPath()[i].getWorldPosition()),tile);
            //Debug.Log((tilemap.WorldToCell(enemyDataHandler.getPath()[i].getWorldPosition())) +" this cell should be changed to new tile "
                //+ enemyDataHandler.getPath()[i].getWorldPosition() + " this is the world position of the new tile" );
            enemyDataHandler.highlightedNodes.Add(enemyDataHandler.getPath()[i]);
            if (playerSet.items[0] == null) {
                Debug.Log("player is null");
                return;
            }
            if (playerSet.items[0].transform.position != enemyDataHandler.getPath()[i].getWorldPosition()) {
                grid2D.setEnemyAtPosition(enemy,enemyDataHandler.getPath()[i].getWorldPosition());
            }
            
            //Debug.Log(tilemap.GetTile(tilemap.WorldToCell(enemyDataHandler.getPath()[i].getWorldPosition())).name);
        }
    }

    public virtual void unHighlight(GameObject enemy) {
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
        // Debug.Log(tilemap.name);
        foreach (var node in enemyDataHandler.highlightedNodes) {
            tilemap.SetTile(tilemap.WorldToCell(node.getWorldPosition()),null);
            
        }
        

        enemyDataHandler.highlightedNodes = new List<Node2D>();
    }
    
   

    public virtual void resetWithNewPosition(GameObject enemy, Vector3 dir,Tile tile) {    
        unHighlight(enemy);
        
        GameObject gridOwner = combatManagerSet.items[0];
        
        Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();

        var newPath = new List<Node2D>();
        foreach (var node2D in enemyDataHandler.getPath()) {
            var pos = tilemap.WorldToCell(node2D.getWorldPosition());
           
            var worldtogridpos = new Vector3Int((int) (pos.x + dir.x), (int) (pos.y + dir.y), 0);
            //Debug.Log(pos + " this is grid position before push. " + worldtogridpos + " this is grid position of node after push. "+ tilemap.GetCellCenterWorld(worldtogridpos) + " this is world position of node");
            newPath.Add(new Node2D(false,
                tilemap.GetCellCenterWorld(worldtogridpos),
                worldtogridpos.x,
                worldtogridpos.y));
        }
        enemyDataHandler.setPath(newPath);
        //Debug.Log(tile);
        Highlight(enemy,tile);
    }
}
