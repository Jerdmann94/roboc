using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShatterContext : AttributeContext {
   public int chanceToActivate;
   public bool shouldShatter;

   public ShatterContext(int chanceToActivate, AttributeContextEnum typeEnum) {
      this.chanceToActivate = chanceToActivate;
      attributeContextTypeEnum = typeEnum;

   }
}
