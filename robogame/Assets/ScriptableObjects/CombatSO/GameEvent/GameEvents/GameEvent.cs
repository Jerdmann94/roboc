using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.GameEvent;
using UnityEngine;

[CreateAssetMenu(fileName = "new Card", menuName = "GameEvent/GameEvent")]
public class GameEvent : ScriptableObject {
	private new List<GameEventListener> _listeners = new List<GameEventListener>();

	public void emit() {
		for (int i = _listeners.Count - 1; i >= 0; i--) {
			_listeners[i].onEmit();
		}
	}

	public void removeListener(GameEventListener gameEventListener) {
		_listeners.Remove(gameEventListener);
	}

	public void addListener(GameEventListener gameEventListener) {
		_listeners.Add(gameEventListener);
	}
}