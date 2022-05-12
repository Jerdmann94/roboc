using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class BoolValue  : ScriptableObject {
    [FormerlySerializedAs("Value")] public bool value;
}
