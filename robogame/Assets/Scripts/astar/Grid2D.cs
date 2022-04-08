using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Grid2D : MonoBehaviour
{
    public Vector3 gridWorldSize;
    public float nodeRadius;
    public Node2D[,] Grid;
   
    [SerializeField] private Tilemap defaultTileMap;
    public List<Node2D> path;
    Vector3 worldBottomLeft;
    public Canvas canvas;
    public GameObject tiletext;

    
    float nodeDiameter;
    public int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        gridSizeX = 17; //defaultTileMap.cellBounds.x;
        gridSizeY = 17; //defaultTileMap.cellBounds.y;
    }

    

    void CreateGrid()
    {
        Grid = new Node2D[gridSizeX, gridSizeY];
        worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
               
                
                //Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                Vector3 worldPoint = defaultTileMap.GetCellCenterWorld(new Vector3Int(x+(int)worldBottomLeft.x, y+(int)worldBottomLeft.y, 0));
                
                Grid[x, y] = new Node2D(false, worldPoint, x, y);
                
                
                if (!defaultTileMap.HasTile((defaultTileMap.WorldToCell(Grid[x,y].getWorldPosition())))) {
                    Grid[x, y].SetObstacle(true);
                    continue;
                }

                //|| !defaultTileMap.HasTile(defaultTileMap.WorldToCell(Grid[x,y].worldPosition))
                //if (obstaclemap.HasTile(obstaclemap.WorldToCell(Grid[x, y].worldPosition)))
                    //Grid[x, y].SetObstacle(true);
                    
                    
                    
                        Tile tile = defaultTileMap.GetTile<Tile>(defaultTileMap.WorldToCell(worldPoint));
                        if (tile != null) {
                            Grid[x, y].Tile = tile;
                            if (tile.name == "Isometric_Block_GlowLightBlue_00_1") {
                                Grid[x, y].difficultyCost += 40;
                            }
                        }
                    
                    GameObject temp = Instantiate(tiletext, worldPoint,  Quaternion.identity,canvas.transform);
                    temp.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
                    temp.GetComponent<Text>().text = x + " " + y;
                    if (Grid[x,y].obstacle) {
                        //Debug.Log(Grid[x,y].worldPosition + " " + worldPoint + " " + Grid[x,y].obstacle);
                        temp.GetComponent<Text>().color = Color.red;
                    }

                    if (Grid[x,y].difficultyCost>0) {
                        temp.GetComponent<Text>().color = Color.yellow;
                        
                    }

            }
        }
    }


    //gets the neighboring nodes in the 4 cardinal directions. If you would like to enable diagonal pathfinding, uncomment out that portion of code
    public List<Node2D> GetNeighbors(Node2D node)
    {
        List<Node2D> neighbors = new List<Node2D>();

        //checks and adds top neighbor
        if (node.GridX >= 0 && node.GridX < gridSizeX && node.GridY + 1 >= 0 && node.GridY + 1 < gridSizeY)
            neighbors.Add(Grid[node.GridX, node.GridY + 1]);

        //checks and adds bottom neighbor
        if (node.GridX >= 0 && node.GridX < gridSizeX && node.GridY - 1 >= 0 && node.GridY - 1 < gridSizeY)
            neighbors.Add(Grid[node.GridX, node.GridY - 1]);

        //checks and adds right neighbor
        if (node.GridX + 1 >= 0 && node.GridX + 1 < gridSizeX && node.GridY >= 0 && node.GridY < gridSizeY)
            neighbors.Add(Grid[node.GridX + 1, node.GridY]);

        //checks and adds left neighbor
        if (node.GridX - 1 >= 0 && node.GridX - 1 < gridSizeX && node.GridY >= 0 && node.GridY < gridSizeY)
            neighbors.Add(Grid[node.GridX - 1, node.GridY]);



        /* Uncomment this code to enable diagonal movement
        
        //checks and adds top right neighbor
        if (node.GridX + 1 >= 0 && node.GridX + 1< gridSizeX && node.GridY + 1 >= 0 && node.GridY + 1 < gridSizeY)
            neighbors.Add(Grid[node.GridX + 1, node.GridY + 1]);

        //checks and adds bottom right neighbor
        if (node.GridX + 1>= 0 && node.GridX + 1 < gridSizeX && node.GridY - 1 >= 0 && node.GridY - 1 < gridSizeY)
            neighbors.Add(Grid[node.GridX + 1, node.GridY - 1]);

        //checks and adds top left neighbor
        if (node.GridX - 1 >= 0 && node.GridX - 1 < gridSizeX && node.GridY + 1>= 0 && node.GridY + 1 < gridSizeY)
            neighbors.Add(Grid[node.GridX - 1, node.GridY + 1]);

        //checks and adds bottom left neighbor
        if (node.GridX - 1 >= 0 && node.GridX - 1 < gridSizeX && node.GridY  - 1>= 0 && node.GridY  - 1 < gridSizeY)
            neighbors.Add(Grid[node.GridX - 1, node.GridY - 1]);
        */



        return neighbors;
    }


    public Node2D nodeFromWorldPoint(Vector3 worldPosition) {
        int x = -1;
        int y = -1;
       // int x = Mathf.RoundToInt(worldPosition.x+ (gridSizeX / 2));
        //int y = Mathf.RoundToInt(worldPosition.y + (gridSizeY / 2));
       // Debug.Log(worldPosition.x + " " + worldPosition.y);
        //Debug.Log(x + " "+y);
        foreach (var VARIABLE in Grid) {
            if (VARIABLE.getWorldPosition() == worldPosition) {
                
                x = VARIABLE.GridX;
                y = VARIABLE.GridY;
                //Debug.Log(worldPosition + " " + VARIABLE.getWorldPosition()+ " " +x + " " + y + Grid[x, y].getWorldPosition());
               
                break;
            }

        }

        if (x == -1 && y == -1) {
            Debug.Log("Node not found!");
            return null;
        }
        return Grid[x, y];
        //return Grid[(int) worldPosition.x, (int) worldPosition.y];
    }


    public void setEnemyAtPosition(GameObject enemy) {
        Node2D node = nodeFromWorldPoint(enemy.transform.position);
        node.setEnemy(enemy);
    }
    public void setEnemyAtPosition(GameObject enemy,Vector3 position) {
        Node2D node = nodeFromWorldPoint(position);
        node.setEnemy(enemy);
    }
    
    //
    // Draws visual representation of grid
     void OnDrawGizmos()
     {
         // Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
         //
         // if (Grid != null)
         // {
         //     foreach (Node2D n in Grid)
         //     {
         //         if (n == null) {
         //             continue;
         //         }
         //         if (n.obstacle)
         //             Gizmos.color = Color.red;
         //         else
         //             Gizmos.color = Color.clear;
         //
         //         //Debug.Log(path.Count);
         //         if (path != null && path.Contains(n))
         //             Gizmos.color = Color.black;
         //         Gizmos.DrawCube(n.getWorldPosition(), Vector3.one * (nodeRadius));
         //
         //     }
         // }
     }

     public void setClaimedAtPosition(GameObject enemy, Vector3 position) {
         Node2D node = nodeFromWorldPoint(position);
         node.setClaimed(true);
     }

     public void removeEnemyAtPosition(Vector3 transformPosition) {
         Node2D node = nodeFromWorldPoint(transformPosition);
         node.setEnemy(null);
     }
     
}
