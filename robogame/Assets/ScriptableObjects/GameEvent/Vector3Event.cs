using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.GameEvent;
using UnityEngine;

[CreateAssetMenu]
public class Vector3Event : ScriptableObject {
	private new List<Vector3EventListener> listeners = new List<Vector3EventListener>();

	public void emit(Vector3 vector3) {
		for (int i = listeners.Count - 1; i >= 0; i--) {
			listeners[i].onEmit(vector3);
		}
	}

	public void removeListener(Vector3EventListener gameEventListener) {
		listeners.Remove(gameEventListener);
	}

	public void addListener(Vector3EventListener gameEventListener) {
		listeners.Add(gameEventListener);
	}
}