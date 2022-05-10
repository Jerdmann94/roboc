using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.GameEvent;
using UnityEngine;

[CreateAssetMenu(fileName = "new Card", menuName = "GameEvent/Vector3Event")]
public class Vector3Event : ScriptableObject {
	private new List<Vector3EventListener> _listeners = new List<Vector3EventListener>();

	public void emit(Vector3 vector3) {
		for (int i = _listeners.Count - 1; i >= 0; i--) {
			_listeners[i].onEmit(vector3);
		}
	}

	public void removeListener(Vector3EventListener gameEventListener) {
		_listeners.Remove(gameEventListener);
	}

	public void addListener(Vector3EventListener gameEventListener) {
		_listeners.Add(gameEventListener);
	}
}