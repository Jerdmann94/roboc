using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class TurnState : ScriptableObject
{
    [SerializeField]
    private List<Round> rounds;

    private Round currentRound = null;
    [SerializeField]
    private GameEvent roundEmitter;

    public Round CurrentRound {
        get { return currentRound;}
        set {
            currentRound = value;
            roundEmitter.emit();
        }
    }

    public void startState() {
        CurrentRound = rounds[0];
    }

    public void nextState() {
        int roundCount = rounds.IndexOf(currentRound) + 1;
        bool outOfBoundsCheck = roundCount > rounds.Count-1;
        CurrentRound = outOfBoundsCheck ? rounds[0] : rounds[roundCount];
    }
}
