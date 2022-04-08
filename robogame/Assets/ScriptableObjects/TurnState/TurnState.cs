using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class TurnState : ScriptableObject
{
    [SerializeField]
    private List<Round> rounds;

    private Round _currentRound = null;
    [SerializeField]
    private GameEvent roundEmitter;

    public Round CurrentRound {
        get { return _currentRound;}
        set {
            _currentRound = value;
            roundEmitter.emit();
        }
    }

    public void startState() {
        CurrentRound = rounds[0];
    }

    public void nextState() {
        int roundCount = rounds.IndexOf(_currentRound) + 1;
        bool outOfBoundsCheck = roundCount > rounds.Count-1;
        CurrentRound = outOfBoundsCheck ? rounds[0] : rounds[roundCount];
    }
}
