using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public static class Cardinal {
   public static CardinalDirection getCardinalDirection(Vector3 start, Vector3 target) { 
      Vector3 inDir = (target - start).normalized; 
      inDir = inDir.normalized;
      CardinalDirection dir = CardinalDirection.NorthEast;
      float closest = -Mathf.Infinity;
      float dotProd = 0f;
      var dotProdNS = Vector3.Dot(inDir, Vector3.up);
      var dotProdEW = Vector3.Dot(inDir, Vector3.right);
      if (dotProdNS >= 0) { // north side
         if (dotProdEW >= 0) {// east side - north east  - CORRECT

            dir = CardinalDirection.NorthEast;

         }
         else {//north west - CORRECT

            dir = CardinalDirection.NorthWest;
         }
      }
      else if (dotProdNS < 0) { // south side
         if (dotProdEW >= 0) {// east side - south east - CORRECT
            dir = CardinalDirection.SouthEast;
         }
         else {//south west - CORRECT

            dir = CardinalDirection.SouthWest;
         }
      }
      return dir;
   }

   public static Vector3 getCardinalVector3(CardinalDirection cardinalDirection) {
      //Debug.Log(cardinalDirection);
      Vector3 dir = Vector3.zero;
      switch (cardinalDirection) {
         case CardinalDirection.NorthWest:
            dir = new Vector3(-.75f, .5f, 0);
            break;
         case CardinalDirection.NorthEast:
            dir = new Vector3(.75f, .5f, 0);
            break;
         case CardinalDirection.SouthEast:
            dir = new Vector3(.75f, -.5f, 0);
            break;
         case CardinalDirection.SouthWest:
            dir = new Vector3(-.75f, -.5f, 0);
            break;
         
      }
      return dir;
   }

   public static Quaternion getCardinalVector3(Vector3 start, Vector3 target) {
      var dir = Quaternion.identity;
      Vector3 inDir = (target - start).normalized; 
      inDir = inDir.normalized;
      //Debug.DrawRay(start,inDir,Color.red,4f);
      float closest = -Mathf.Infinity;
      float dotProd = 0f;
      var dotProdNS = Vector3.Dot(inDir, Vector3.up);
      var dotProdEW = Vector3.Dot(inDir, Vector3.right);
      //Debug.Log(dotProd);
     //  if (dotProd > closest) {
     //     //Debug.Log(closest + "north");
     //     closest = dotProd;
     //     dir = Quaternion.Euler(0,0,120);
     //     // z = 120
     //
     //  }
     //  dotProd = Vector3.Dot(inDir, Vector3.down);
     //  Debug.Log(dotProd);
     //  if (dotProd > closest) {
     //     Debug.Log(closest + "south");
     //     closest = dotProd;
     //     dir = Quaternion.Euler(0,0,300);
     //     //z = 300
     //
     //  }
     //  dotProd = Vector3.Dot(inDir, Vector3.left);
     // Debug.Log(dotProd);
     //  if (dotProd > closest) {
     //     Debug.Log(closest + "west");
     //     closest = dotProd;
     //     dir = dir = Quaternion.Euler(0,0,180);
     //     // z = 180
     //
     //  }
     //  dotProd = Vector3.Dot(inDir, Vector3.right);
     //  //Debug.Log(dotProd);
     //  if (dotProd > closest) {
     //     //Debug.Log(closest + "east");
     //     dir = Quaternion.Euler(0, 0, 0);
     //     //z = 0
     //
     //  }

     if (dotProdNS >= 0) { // north side
        if (dotProdEW >= 0) {// east side - north east  - CORRECT
           
           dir = Quaternion.Euler(0, 0, 0);
           
        }
        else {//north west - CORRECT
           
           dir = Quaternion.Euler(0,0,120);
        }
     }
     else if (dotProdNS < 0) { // south side
        if (dotProdEW >= 0) {// east side - south east - CORRECT
           dir = Quaternion.Euler(0,0,300);
        }
        else {//south west - CORRECT
           
           dir = Quaternion.Euler(0,0,180);
        }
     }
      
      
      return dir;
      
   }
}
public enum CardinalDirection 
{
   NorthEast,
   SouthWest,
   SouthEast,
   NorthWest
}
