using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.GameEvent;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject {
	private new List<GameEventListener> listeners = new List<GameEventListener>();

	public void emit() {
		for (int i = listeners.Count - 1; i >= 0; i--) {
			listeners[i].onEmit();
		}
	}

	public void removeListener(GameEventListener gameEventListener) {
		listeners.Remove(gameEventListener);
	}

	public void addListener(GameEventListener gameEventListener) {
		listeners.Add(gameEventListener);
	}
}