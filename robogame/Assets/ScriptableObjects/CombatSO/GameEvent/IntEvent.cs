using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new Card", menuName = "GameEvent/IntEvent")]
public class IntEvent : ScriptableObject
{
    private new List<IntEventListener> _listeners = new List<IntEventListener>();

    public void emit(int num) {
        for (int i = _listeners.Count - 1; i >= 0; i--) {
            _listeners[i].onEmit(num);
        }
        
    }

    public void removeListener(IntEventListener gameEventListener) {
        _listeners.Remove(gameEventListener);
    }

    public void addListener(IntEventListener gameEventListener) {
        _listeners.Add(gameEventListener);
    }
}
