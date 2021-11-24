using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class MouseHandler : MonoBehaviour {

    private MouseInput mouse;
    public  Tilemap    map;
    public  Tile       selectedTile;
    public  Tile       targetTile;
    public  GameObject player;
    private Vector3Int targetPos;

    private Vector3Int TargetPos {
        get => targetPos;
        set {
            targetPos = value;
            print(value);
        } 
    }

    int[,]             vectorChanger = new int[,] { { -1, -2 }, { 0, -1 }, { -2, -1 }, { -1, 0 } };
    
    private ArrayList possibleTiles;
    private ArrayList possibleTilesPos;
    private void Awake(){
        mouse = new MouseInput();
        
    }
    
    private void OnEnable(){
        mouse.Enable();
    }
    private void OnDisable(){
            mouse.Disable();
        }
    // Start is called before the first frame update
    void Start(){
        mouse.Mouse.MouseClick.performed += data =>MouseClick();
    }
    
    private void MouseClick(){
        Vector2 mousePosition = mouse.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPos = map.WorldToCell(mousePosition);
        Tile       tile    = map.GetTile<Tile>(gridPos);
        print(tile);
        if (!possibleTiles.Contains(tile)&& tile != null) {
            map.SetTile(gridPos,targetTile);
            print(gridPos);
            TargetPos = gridPos;
        }
    }

    public void basicMove() {
        highlightCells();
       
        
    }

    public void highlightCells() {

        possibleTiles = new ArrayList();
        possibleTilesPos = new ArrayList();
        Vector3    pos           =  player.transform.position;
        
       // Vector3Int gridPos = new Vector3Int((int)pos.x,(int)pos.y,(int)pos.z);
        for (int i = 0; i < 4; i++) {
            Vector3Int gridPos = map.WorldToCell(pos);
            gridPos.x = gridPos.x + vectorChanger[i,0];
            gridPos.y = gridPos.y + vectorChanger[i,1];
            if (map.HasTile(gridPos)) {
                possibleTiles.Add(map.GetTile<Tile>(gridPos));
                possibleTilesPos.Add(gridPos);
                map.SetTile(gridPos,selectedTile);
            }
            else {
                print("tile not found");
            }
        }
        
    }

    public void confirm() {
        
        print(map.GetCellCenterWorld(targetPos));
        player.transform.SetPositionAndRotation(map.GetCellCenterWorld(targetPos),player.transform.rotation);
        resetTiles();
    }

    private void resetTiles() {
        for (int i = 0; i < possibleTiles.Count; i++) {
           map.SetTile((Vector3Int) possibleTilesPos[i],(TileBase) possibleTiles[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


