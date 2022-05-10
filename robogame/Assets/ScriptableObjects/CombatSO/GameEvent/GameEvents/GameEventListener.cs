using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.GameEvent {
	public class GameEventListener : MonoBehaviour {
		public global::GameEvent emitter;

		public UnityEvent response;

		private void OnEnable() {
			emitter.addListener(this);
		}

		private void OnDisable() {
			emitter.removeListener(this);
		}

		public void onEmit() {
			response.Invoke();
		}
	}
}