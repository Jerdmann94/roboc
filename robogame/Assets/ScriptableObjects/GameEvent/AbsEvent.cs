using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.GameEvent;
using UnityEngine;

public interface AbsEvent {
	public abstract void emit();

	public abstract void removeListener(GameEventListener gameEventListener);

	public abstract void addListener(GameEventListener gameEventListener);
}