using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AbsAction:ScriptableObject {
    
    public GoRunTimeSet combatManagerSet;
    public GoRunTimeSet playerSet;
    public GoRunTimeSet tilemapSet;
    public TargetTypeSo typeSo;
    public int damage;
    public abstract Task execute(GameObject enemy);
    public abstract bool check(GameObject enemy);

    public virtual void highlight(GameObject enemy,Tile tile) {
        
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        
        
        GameObject gridOwner = combatManagerSet.items[0];
        Grid2D grid2D = gridOwner.GetComponent<Grid2D>();
        
        Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
        
        int moveAmount = enemy.GetComponent<EnemyDataHandler>().moveAmount;
        for (int i = 0; i < moveAmount; i++) {
            if (tilemap != null)
                if (enemyDataHandler.getPath().Count < i + 1) {
                    //Debug.Log("breaking movement because path is less than movement");
                    break;
                }
                tilemap.SetTile(tilemap.WorldToCell(enemyDataHandler.getPath()[i].getWorldPosition()), tile);
            //Debug.Log((tilemap.WorldToCell(enemyDataHandler.getPath()[i].getWorldPosition())) +" this cell should be changed to new tile "
                //+ enemyDataHandler.getPath()[i].getWorldPosition() + " this is the world position of the new tile" );
            enemyDataHandler.highlightedNodes.Add(enemyDataHandler.getPath()[i]);
            if (playerSet.items[0] == null) {
                Debug.Log("player is null");
                return;
            }
            if (playerSet.items[0].transform.position != enemyDataHandler.getPath()[i].getWorldPosition()) {
                //grid2D.setEnemyAtPosition(enemy,enemyDataHandler.getPath()[i].getWorldPosition());
                grid2D.setClaimedAtPosition(enemy, enemyDataHandler.getPath()[i].getWorldPosition());
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
        Grid2D grid2D = gridOwner.GetComponent<Grid2D>();

        var node = grid2D.nodeFromWorldPoint(enemy.transform.position);
        
        
        Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "TilemapForEnemies")?.GetComponent<Tilemap>();
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        
        var newPath = new List<Node2D>();
        foreach (var node2D in enemyDataHandler.getPath()) {
            var pos = tilemap.WorldToCell(node2D.getWorldPosition());
           
            var worldtogridpos = new Vector3Int((int) (pos.x + dir.x), (int) (pos.y + dir.y), 0);
            //Debug.Log(pos + " this is grid position before push. " + worldtogridpos + " this is grid position of node after push. "+ tilemap.GetCellCenterWorld(worldtogridpos) + " this is world position of node");
            newPath.Add(new Node2D(null,
                tilemap.GetCellCenterWorld(worldtogridpos),
                worldtogridpos.x,
                worldtogridpos.y));
        }
        enemyDataHandler.setPath(newPath);
        //Debug.Log(tile);
        
        highlight(enemy,tile);
        
    }

    protected async void getPathForTargetType(GameObject enemy, MoveType moveType,bool ignoreObstacles) {
        Pathfinding2D pathfinder = combatManagerSet.items[1].GetComponent<Pathfinding2D>();
		
        Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
		
        
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        //var switchVar = "";
        // if (typeSo != null) {
        //     switchVar = typeSo.name;
        // }
        //if (switchVar == null) throw new ArgumentNullException(nameof(switchVar));
        switch (moveType) {
            case MoveType.Closest:
                break;
            case MoveType.Weakest:
                break;
            case MoveType.Special:
                if (ignoreObstacles) {
                    if (enemyDataHandler.specialTarget == null) {
                        getPathWithOutObstacles(enemy.transform.position,playerSet.items[0].transform.position,playerSet.items[0],enemy);
                    }
                    else {
                        getPathWithOutObstacles(enemy.transform.position,enemyDataHandler.specialTarget.transform.position,enemyDataHandler.specialTarget,enemy);
                    }
                }
                else {
                    if (enemyDataHandler.specialTarget == null) {
                        getPathWithObstacles(enemy.transform.position,playerSet.items[0].transform.position,playerSet.items[0],enemy);
                    }
                    else {
                        getPathWithObstacles(enemy.transform.position,enemyDataHandler.specialTarget.transform.position,enemyDataHandler.specialTarget,enemy);
                    }
                }
                //enemy.GetComponent<EnemyDataHandler>().setPath(grid2D.path);
                break;
            default:
                if (ignoreObstacles) {
                    getPathWithOutObstacles(enemy.transform.position,playerSet.items[0].transform.position,playerSet.items[0],enemy);

                }
                else {
                    getPathWithObstacles(enemy.transform.position,playerSet.items[0].transform.position,playerSet.items[0],enemy);
                }
                break;
        }
        await Task.Yield();
    }

    private void getPathWithObstacles(Vector3 start, Vector3 target, GameObject setTarget,GameObject enemy) {
        Pathfinding2D pathfinder = combatManagerSet.items[1].GetComponent<Pathfinding2D>();
        Grid2D grid2D = combatManagerSet.items[0].GetComponent<Grid2D>();
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        enemyDataHandler.target = playerSet.items[0];
        pathfinder.FindPath(start,target);
        enemyDataHandler.setPath(grid2D.path);
        enemyDataHandler.target = setTarget;
    }
    private void getPathWithOutObstacles(Vector3 start, Vector3 target, GameObject setTarget,GameObject enemy) {
        Pathfinding2D pathfinder = combatManagerSet.items[1].GetComponent<Pathfinding2D>();
        EnemyDataHandler enemyDataHandler = enemy.GetComponent<EnemyDataHandler>();
        var path = pathfinder.findPathWithoutObstacles(start, target);
        enemyDataHandler.setPath(path);
        enemyDataHandler.target = setTarget;
    }
}

public enum MoveType {
    a,
    Closest,
    Weakest,
    Special
}
