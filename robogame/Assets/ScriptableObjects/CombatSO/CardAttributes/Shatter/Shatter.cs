using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScriptableObjects.Sets;
using UnityEngine;

[CreateAssetMenu(fileName ="new Deck", menuName = "CardAttributes/Shatter")]
public class Shatter : CardAttribute {
    
   

    public override async Task<AttributeContext> execute(GameCard gameCard,AttributeContext attributeContext) {
        int index = Random.Range(0, 101);
        var shatterContext = (ShatterContext) attributeContext;
        if (index < shatterContext.chanceToActivate) {

            shatterContext.shouldShatter = true;
        }
        else {
            shatterContext.chanceToActivate += 10;
            
        }
        Debug.Log(shatterContext.chanceToActivate);
        await Task.Yield();
        return shatterContext;
    }


   
}
