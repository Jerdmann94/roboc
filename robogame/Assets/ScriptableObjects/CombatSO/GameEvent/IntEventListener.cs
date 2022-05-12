using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntEventListener : MonoBehaviour
{
    public global::IntEvent emitter;

    public IntUnityEvent response;

    private void OnEnable() {
        emitter.addListener(this);
    }

    private void OnDisable() {
        emitter.removeListener(this);
    }

    public void onEmit(int i) {
        response.Invoke(i);
    }
}
