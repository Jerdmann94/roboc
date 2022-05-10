using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="new Deck", menuName = "Decks/PlayerDeck")]
public class DeckSo : ScriptableObject {
    public CardAbs[] deck;
}
