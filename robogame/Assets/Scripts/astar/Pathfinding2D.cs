using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Pathfinding2D : MonoBehaviour {

    
    public Vector3 seeker, target;
    Grid2D grid;
    Node2D seekerNode, targetNode;
    public GameObject gridOwner;
    public Tilemap tilemap;


    public void initializePathFinder() {
       
        grid = gridOwner.GetComponent<Grid2D>();
        
    }

    public List<Node2D> findPathForVision(Vector3 startPos, Vector3 targetPos) {
        //get player and target position in grid coords
        
        //Debug.Log("start pos " + startPos + " target pos " + targetPos);
        seekerNode = grid.nodeFromWorldPoint(startPos);
        targetNode = grid.nodeFromWorldPoint(targetPos);

        List<Node2D> openSet = new List<Node2D>();
        HashSet<Node2D> closedSet = new HashSet<Node2D>();
        openSet.Add(seekerNode);
        
        //calculates path for pathfinding
        while (openSet.Count > 0)
        {

            //iterates through openSet and finds lowest FCost
            Node2D node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].getFCostNoDifficultyOrEnemy() <= node.getFCostNoDifficultyOrEnemy())
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            //If target found, retrace path
            if (node == targetNode)
            {
                return returnRetracePath(seekerNode, targetNode);
                
                
            }
            
            //adds neighbor nodes to openSet
            foreach (Node2D neighbour in grid.GetNeighbors(node))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }


                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
            if (openSet.Count < 1 && node != targetNode) {
                //Debug.Log("path  could not be completed");
                //Debug.Log(closedSet.Count);
                //RetracePath(seekerNode, closedSet.Last());
                //maybe they should heal or something here instead of just wandering back and forth
                return null;
            }
        }

        return null;
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //get player and target position in grid coords
        
        //Debug.Log("start pos " + startPos + " target pos " + targetPos);
        seekerNode = grid.nodeFromWorldPoint(startPos);
        targetNode = grid.nodeFromWorldPoint(targetPos);

        List<Node2D> openSet = new List<Node2D>();
        HashSet<Node2D> closedSet = new HashSet<Node2D>();
        openSet.Add(seekerNode);
        
        //calculates path for pathfinding
        while (openSet.Count > 0)
        {

            //iterates through openSet and finds lowest FCost
            Node2D node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost <= node.FCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            //If target found, retrace path
            if (node == targetNode)
            {
                RetracePath(seekerNode, targetNode);
                
                return;
            }
            
            //adds neighbor nodes to openSet
            foreach (Node2D neighbour in grid.GetNeighbors(node))
            {
                if (neighbour.obstacle || closedSet.Contains(neighbour) || neighbour.getClaimed())
                {
                    continue;
                }

                if (neighbour.getEnemy()!= null) {
                    if (!neighbour.getEnemy().CompareTag("Player")) {
                        continue;
                    }
                    
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
            if (openSet.Count < 1 && node != targetNode) {
                //Debug.Log("path  could not be completed");
                //Debug.Log(closedSet.Count);
                //RetracePath(seekerNode, closedSet.Last());
                //maybe they should heal or something here instead of just wandering back and forth
                //make them move random
                doRandomMove(startPos);
                return;
            }
        }

        
    }

    private void doRandomMove(Vector3 startPos) {
        List<Node2D> possibleSpaces = new List<Node2D>();
        Grid2D grid2D = gridOwner.GetComponent<Grid2D>();
        //Tilemap tilemap = tilemapSet.items.SingleOrDefault(obj => obj.name == "Tilemap")?.GetComponent<Tilemap>();
        var pos = tilemap.WorldToCell(startPos);
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
            if (node.getEnemy() != null || node.getClaimed() || node.obstacle) continue;
            // grid2D.nodeFromWorldPoint(enemy.transform.position).setEnemy(null);
            // enemy.transform.position = node.getWorldPosition();
            //grid2D.setClaimedAtPosition(enemy, node.getWorldPosition());
            var path = new List<Node2D>();
            path.Add(node);
            grid.path = path;
            break;
        }

    }

    //reverses calculated path so first node is closest to seeker
    void RetracePath(Node2D startNode, Node2D endNode)
    {
        List<Node2D> path = new List<Node2D>();
        Node2D currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

       
        grid.path = path;

    }
    List<Node2D> returnRetracePath(Node2D startNode, Node2D endNode)
    {
        List<Node2D> path = new List<Node2D>();
        Node2D currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

       
        return path;

    }

    //gets distance between 2 nodes for calculating cost
    int GetDistance(Node2D nodeA, Node2D nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}