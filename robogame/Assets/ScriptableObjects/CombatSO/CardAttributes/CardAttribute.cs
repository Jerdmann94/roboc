using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
public abstract class CardAttribute : ScriptableObject {
	public AttributeContextEnum context;
	public abstract Task<AttributeContext> execute(GameCard gameCard, AttributeContext attributeContext);
}
