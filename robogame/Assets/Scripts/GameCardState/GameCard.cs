using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCard {
   public int handPosition;
   public readonly CardAbs card;
   public List<GameCardAttributes> gameCardAttributesList = new List<GameCardAttributes>();
   
   // EITHER NEED TO BUILD THE CONTEXTS HERE USING A CONTEXT SCRIPTABLE OBJECT AS THE BASE 
   // OR FIND A WAY TO INHERIT CORRECTLY THE CONTEXT FROM THE CARD ATTRIBUTES
   // PROBABLY JUST USE A SWITCH STATEMENT AND ENUM + SCRIPTABLE OBEJCTS TO BUILD A BASE SET
   // OF CONTEXTS FOR EACH POSSIBLE ATTRIBUTE, AND THEN ASSIGN THEM TO EACH SPECIFIC INSTANCE
   public GameCard(CardAbs cardAbs) {
      card = cardAbs; 
      
      foreach (var cardAtt in cardAbs.cardAttributes) {
         getContextForAttribute(cardAtt);

      }
   }

   private void getContextForAttribute(CardAttribute cardAtt) {
      var enumTypeStart = cardAtt.context;
      AttributeContext context = default;
      switch (cardAtt.context) {
         case ShatterContextEnum:
            var enumType = (ShatterContextEnum) enumTypeStart;
            context = new ShatterContext(enumType.shatterChance,enumType);
            break;
      }
      gameCardAttributesList.Add(new GameCardAttributes(context,cardAtt));
   }
}

public class GameCardAttributes {
   public AttributeContext context;
   public readonly CardAttribute cardAttribute;
   
   public GameCardAttributes(AttributeContext context, CardAttribute cardAttribute) {
      this.context = context;
      this.cardAttribute = cardAttribute;
   }
}
