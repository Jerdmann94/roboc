using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;


public class AddMeToPossibleTarget : MonoBehaviour {
    public List<Vector3IntSet> sets;
    public List<TileRunTimeSet> tiles;
    public GORunTimeSet tileMapSet;
    public GORunTimeSet gridGameObject;

    // Start is called before the first frame update
    private void Awake() {
        Grid2D grid = gridGameObject.items[0].GetComponent<Grid2D>();
        Tilemap tilemap = tileMapSet.items[2].GetComponent<Tilemap>();
        //Debug.Log(grid);
        foreach (var VARIABLE in sets) {
            Vector3 temp = tilemap.GetCellCenterWorld((tilemap.WorldToCell(transform.position)));
            if (!grid.NodeFromWorldPoint(temp).obstacle && grid.NodeFromWorldPoint(temp).getEnemy() == null) {
                VARIABLE.add(tilemap.WorldToCell(transform.position));
                // Debug.Log(grid.NodeFromWorldPoint(transform.position).getWorldPosition() + " " + transform.position);
            }
           
            
           // Debug.Log(transform.position);
            
        }

        foreach (var VARIABLE in tiles) {
            Vector3 temp = tilemap.GetCellCenterWorld((tilemap.WorldToCell(transform.position)));
            if (!grid.NodeFromWorldPoint(temp).obstacle) {
                
                VARIABLE.items.Add(tilemap.GetTile<Tile>(tilemap.WorldToCell(transform.position)));
                
            }
        }
    }

    // private void OnDisable() {
    //     Tilemap tilemap = tileMapSet.items[0].GetComponent<Tilemap>();
    //     foreach (var VARIABLE in sets) {
    //         VARIABLE.remove(tilemap.WorldToCell(this.transform.position));
    //     }
    //
    //     foreach (var VARIABLE in tiles) {
    //         VARIABLE.items.Remove(tilemap.GetTile<Tile>(tilemap.WorldToCell(transform.position)));
    //     }
    // }
}
