using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="new Deck", menuName = "Decks/PlayerDeck")]
public class DeckSO : ScriptableObject {
    public CardAbs[] deck;
}
