using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Card", menuName = "GameEvent/GameObjectEvent")]
public class GameObjectEmitter : ScriptableObject
{
    private new List<GameObjectEventListener> _listeners = new List<GameObjectEventListener>();

    public void emit(GameObject go) {
        for (int i = _listeners.Count - 1; i >= 0; i--) {
            _listeners[i].onEmit(go);
        }
    }

    public void removeListener(GameObjectEventListener gameEventListener) {
        _listeners.Remove(gameEventListener);
    }

    public void addListener(GameObjectEventListener gameEventListener) {
        _listeners.Add(gameEventListener);
    }
}
