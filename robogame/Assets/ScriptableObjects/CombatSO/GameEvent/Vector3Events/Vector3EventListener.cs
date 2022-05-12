using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vector3EventListener : MonoBehaviour {
	public global::Vector3Event emitter;
	public UnityEvent           response;

	private void OnEnable() {
		emitter.addListener(this);
	}

	private void OnDisable() {
		emitter.removeListener(this);
	}

	public void onEmit(Vector3 vector3) {
		if (vector3 == transform.position) {
			response.Invoke();
		}
	}
}