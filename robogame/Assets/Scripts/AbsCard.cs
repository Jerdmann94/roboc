using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsCard {
   [SerializeField]
   private int    cost;
   [SerializeField]
   private String name;
   [SerializeField]
   private String cardText;
   
   public abstract void playCard();
   public abstract void highlightTiles();
   

}
