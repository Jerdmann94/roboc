using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DecideWhichListsToJoin : MonoBehaviour
{
   public Vector3IntSet possibleTilePos;
   public Vector3IntSet playerAttackFormationHighlight;
   public Vector3IntSet targetPos;
   public GoRunTimeSet tileMapSet;

   public void init(WhichTilePosList whichTilePosList) {
      Tilemap tilemap = tileMapSet.items[2].GetComponent<Tilemap>();
      
      foreach (Transform tileObject in transform) {
         Vector3 position = tileObject.position;
         //Debug.Log(transform.position +" " + position);
         
         switch (whichTilePosList) {
            case WhichTilePosList.PossibleTilePos:
            
               if (tilemap.HasTile(tilemap.WorldToCell(position))) {
                  possibleTilePos.items.Add(tilemap.WorldToCell(position));
               }
               break;
            case WhichTilePosList.PlayerAttackFormationHighlight :
            
               if (tilemap.HasTile(tilemap.WorldToCell(position))) {
                  playerAttackFormationHighlight.items.Add(tilemap.WorldToCell(position));
               }
               break;
            case WhichTilePosList.TargetPos:
               
               if (tilemap.HasTile(tilemap.WorldToCell(position))) {
                  Debug.Log(position);
                  targetPos.items.Add(tilemap.WorldToCell(position));
               }

               break;
         }
      }
      
   }
}
public enum WhichTilePosList{
   PlayerAttackFormationHighlight,
   PossibleTilePos,
   TargetPos


}
