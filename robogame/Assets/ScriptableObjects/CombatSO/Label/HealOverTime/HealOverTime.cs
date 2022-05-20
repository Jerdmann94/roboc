using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "LabelBase", menuName = "Label/HealOverTime")]
public class HealOverTime : LabelBase {
   public PlayerStatBlockSo stats;
 
   public int healTimer;
   public int healAmount;
   public int currentTurn;
   public void execute() {
      stats.health.takeDamage(healAmount);
   }
   public LabelBase init(int healAmountInit, int hea,PlayerStatBlockSo playerStats) {
      stats = playerStats;
      this.healAmount = healAmountInit;
      this.healTimer = hea;
      labelType = LabelType.HealOverTime;
      return this;
   }
}
