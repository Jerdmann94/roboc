using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameObjectEventListener : MonoBehaviour
{
    public global::GameObjectEmitter emitter;

    public GameObjectUnityEvent response;

    private void OnEnable() {
        emitter.addListener(this);
    }

    private void OnDisable() {
        emitter.removeListener(this);
    }

    public void onEmit(GameObject go) {
        response.Invoke(go);
    }
}
